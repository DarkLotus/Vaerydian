using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using WorldGeneration;
using WorldGeneration.Terrain;
using Vaerydian.Screens;

namespace Vaerydian.Maps
{

    public class ViewPort
    {
        public Point Origin = new Point();
        public Point Dimensions = new Point();
    }

    public class ColorVal
    {
        public ColorVal(Color c, float val)
        {
            color = c;
            value = val;
        }

        public Color color;
        public float value;
    }


    /// <summary>
    /// manages and provides access to all maps
    /// </summary>
    public class MapEngine
    {

        private MapEngine() { }

        private static readonly MapEngine me_Instance = new MapEngine();

        public static MapEngine Instance { get { return me_Instance; } }

        


        /// <summary>
        /// private reference to the current map
        /// </summary>
        private Map me_CurrentMap;
        /// <summary>
        /// current map
        /// </summary>
        public Map CurrentMap { get { return me_CurrentMap; } set { me_CurrentMap = value; } }

        /// <summary>
        /// the curent view on the screen
        /// </summary>
        private ViewPort me_ViewPort = new ViewPort();
        /// <summary>
        /// the curent view on the screen
        /// </summary>
        public ViewPort ViewPort { get { return me_ViewPort; } set { me_ViewPort = value; } }

        /// <summary>
        /// World Generator
        /// </summary>
        private WorldGenerator me_WorldGenerator = new WorldGenerator();
        /// <summary>
        /// World Generator
        /// </summary>
        public WorldGenerator WorldGenerator
        {
            get { return me_WorldGenerator; }
            set { me_WorldGenerator = value; }
        }

        /// <summary>
        /// Size of Tiles
        /// </summary>
        private int me_TileSize;
        /// <summary>
        /// Size of Tiles
        /// </summary>
        public int TileSize
        {
            get { return me_TileSize; }
            set { me_TileSize = value; }
        }

        /// <summary>
        /// Number of Tiles in x Direction
        /// </summary>
        private int me_XTiles;
        /// <summary>
        /// Number of Tiles in x Direction
        /// </summary>
        public int XTiles
        {
            get { return me_XTiles; }
            set { me_XTiles = value; }
        }
        /// <summary>
        /// Number of Tiles in y Direction
        /// </summary>
        private int me_YTiles;
        /// <summary>
        /// Number of Tiles in y Direction
        /// </summary>
        public int YTiles
        {
            get { return me_YTiles; }
            set { me_YTiles = value; }
        }

        /// <summary>
        /// causes each tile to be color shaded according to its temperature
        /// </summary>
        private bool me_ShowTemperature = false;
        /// <summary>
        /// causes each tile to be color shaded according to its temperature
        /// </summary>
        public bool ShowTemperature
        {
            get { return me_ShowTemperature; }
            set { me_ShowTemperature = value; }
        }

        /// <summary>
        /// causes each tile to be color shaded according to its rainfall
        /// </summary>
        private bool me_ShowPrecipitation = false;
        /// <summary>
        /// causes each tile to be color shaded according to its rainfall
        /// </summary>
        public bool ShowPrecipitation
        {
            get { return me_ShowPrecipitation; }
            set { me_ShowPrecipitation = value; }
        }

        /// <summary>
        /// number of steps in the color gradient
        /// </summary>
        private static int me_GradientSteps = 2000;
        
        /// <summary>
        /// an array of color values that align to gradient color band
        /// </summary>
        private ColorVal[] colorDict = new ColorVal[2000];


        private int xStart;
        private int xFinish;
        private int yStart;
        private int yFinish;

        /// <summary>
        /// should a screenshot be grabbed?
        /// </summary>
        private bool me_YesScreenshot = false;

        /// <summary>
        /// should a screenshot be grabbed?
        /// </summary>
        public bool YesScreenshot
        {
            get { return me_YesScreenshot; }
            set { me_YesScreenshot = value; }
        }

        /// <summary>
        /// World Generator Status Message for Loading Messages
        /// </summary>
        public String WorldGeneratorStatusMessage
        {
            get{return me_WorldGenerator.StatusMessage;}
        }


        /// <summary>
        /// for saving maximum values
        /// </summary>
        private float maxVal = 0f;

        /// <summary>
        /// private content manager copy
        /// </summary>
        private ContentManager me_contentManager;
        /// <summary>
        /// content manager copy
        /// </summary>
        public ContentManager ContentManager { get { return me_contentManager; } set { me_contentManager = value; } }

        private List<Texture2D> textures = new List<Texture2D>();

        //texture for showing temperature
        private Texture2D temperatureTexture;
        //texture for making screenshots
        private Texture2D exportTexture;

        /// <summary>
        /// performs any needed map loading
        /// </summary>
        public void LoadContent()
        {
            textures.Add(me_contentManager.Load<Texture2D>("grass"));//0
            textures.Add(me_contentManager.Load<Texture2D>("ocean"));//1
            textures.Add(me_contentManager.Load<Texture2D>("mountains"));//2
            textures.Add(me_contentManager.Load<Texture2D>("arctic"));//3
            textures.Add(me_contentManager.Load<Texture2D>("beach"));//4
            textures.Add(me_contentManager.Load<Texture2D>("forest"));//5
            textures.Add(me_contentManager.Load<Texture2D>("grasslands"));//6
            textures.Add(me_contentManager.Load<Texture2D>("jungle"));//7
            textures.Add(me_contentManager.Load<Texture2D>("desert"));//8
            textures.Add(me_contentManager.Load<Texture2D>("swamp"));//9
            textures.Add(me_contentManager.Load<Texture2D>("tundra"));//10
            textures.Add(me_contentManager.Load<Texture2D>("foothills"));//11
            temperatureTexture = me_contentManager.Load<Texture2D>("temperature");
            exportTexture = me_contentManager.Load<Texture2D>("export");

        }

        /// <summary>
        /// performs any needed initialization
        /// </summary>
        public void Initialize()
        {
            createShadeColor();
        }

        /// <summary>
        /// draws the visible map based on the current viewport
        /// </summary>
        /// <param name="gameTime">current gameTime</param>
        /// <param name="spriteBatch">the spriteBatch to render with</param>
        public void DrawMap(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Terrain terrain;

            //iterate through the viewable indexes and draw the tiles
            for (int x = xStart; x <= xFinish; x++)
            {
                for (int y = yStart; y <= yFinish; y++)
                {
                    //get the terrain at this cell
                    terrain = me_WorldGenerator.WorldTerrainMap[x, y];

                    if (me_ShowTemperature)
                    {
                        //draw temperature texture
                        spriteBatch.Draw(temperatureTexture, new Vector2((x * me_TileSize), (y * me_TileSize)),
                            null, shadingValue(terrain.Temperature), 0.0f, new Vector2(me_ViewPort.Origin.X, me_ViewPort.Origin.Y), new Vector2(1, 1),
                            SpriteEffects.None, 0);
                    }
                    else if (me_ShowPrecipitation)
                    {
                        //draw rainfall textures
                        spriteBatch.Draw(temperatureTexture, new Vector2((x * me_TileSize), (y * me_TileSize)),
                            null, shadingValue(terrain.Rainfall), 0.0f, new Vector2(me_ViewPort.Origin.X, me_ViewPort.Origin.Y), new Vector2(1, 1),
                            SpriteEffects.None, 0);
                    }
                    else
                    {
                        //draw terrain texture
                        spriteBatch.Draw(textures[getBaseTexture(terrain)], new Vector2((x * me_TileSize), (y * me_TileSize)),
                            null, Color.White, 0.0f, new Vector2(me_ViewPort.Origin.X, me_ViewPort.Origin.Y), new Vector2(1, 1),
                            SpriteEffects.None, 0);
                    }
                }
            }

            spriteBatch.DrawString(FontManager.Instance.Fonts["General"], "MaxVal: " + maxVal, new Vector2(0, 28), Color.Blue);

            spriteBatch.End();

            //check to see if the user wanted a screenshot
            if (me_YesScreenshot)
            {
                saveScreenShot(spriteBatch.GraphicsDevice);
                me_YesScreenshot = false;
            }



            maxVal = 0f;
        }

        /// <summary>
        /// updates the tile indexes based on current viewport for the draw loop
        /// </summary>
        public void UpdateView()
        {
            xStart = me_ViewPort.Origin.X / me_TileSize;
            if (xStart <= 0)
                xStart = 0;

            xFinish = (me_ViewPort.Origin.X + me_ViewPort.Dimensions.X) / me_TileSize;
            if (xFinish >= me_XTiles - 1)
                xFinish = me_XTiles - 1;

            yStart = me_ViewPort.Origin.Y / me_TileSize;
            if (yStart <= 0)
                yStart = 0;

            yFinish = (me_ViewPort.Origin.Y + me_ViewPort.Dimensions.Y) / me_TileSize;
            if (yFinish >= me_YTiles - 1)
                yFinish = me_YTiles - 1;

        }


        /// <summary>
        /// returns the texture id for the given terrain
        /// </summary>
        /// <param name="terrain">terrain to get the texture for</param>
        /// <returns></returns>
        private int getBaseTexture(Terrain terrain)
        {
            switch (terrain.BaseTerrainType)
            {
                case BaseTerrainType.Land:
                    if (terrain.LandTerrainType == LandTerrainType.Arctic)
                        return 3;
                    if (terrain.LandTerrainType == LandTerrainType.Beach)
                        return 4;
                    if (terrain.LandTerrainType == LandTerrainType.Desert)
                        return 8;
                    if (terrain.LandTerrainType == LandTerrainType.Forest)
                        return 5;
                    if (terrain.LandTerrainType == LandTerrainType.Grassland)
                        return 6;
                    if (terrain.LandTerrainType == LandTerrainType.Jungle)
                        return 7;
                    if (terrain.LandTerrainType == LandTerrainType.Swamp)
                        return 9;
                    if (terrain.LandTerrainType == LandTerrainType.Tundra)
                        return 10;
                    return 0;
                case BaseTerrainType.Ocean:
                    return 1;
                case BaseTerrainType.Mountain:
                    if (terrain.MountainTerrainType == MountainTerrainType.Foothill)
                        return 11;
                    if(terrain.MountainTerrainType == MountainTerrainType.Cascade)
                        return 2;
                    return 2;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// gets the correct shading value
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private Color shadingValue(float val)
        {
            if (val > maxVal)
                maxVal = val;

            if (val > 1f)
                val = 1f;
            if (val < 0f)
                val = 0f;
            if (!((val >= 0f) && (val <= 1f)))
                val = 0f;
            return colorDict[(int)((float)(colorDict.Length-1) * val)].color;
        }

        private void createShadeColor()
        {
            List<ColorVal> cList = new List<ColorVal>();

            cList.Add(new ColorVal(Color.Black, 0.0f));//dark blue
            cList.Add(new ColorVal(Color.Blue, 0.2f));//light blue
            cList.Add(new ColorVal(Color.Green, 0.4f));//green
            cList.Add(new ColorVal(Color.Yellow, 0.6f));//yellow
            cList.Add(new ColorVal(Color.Orange, 0.8f));//orange
            cList.Add(new ColorVal(Color.Red, 1.0f));//red
            
            int j = 1;
            int k = 0;
            ColorVal begin = cList[0];
            ColorVal end = cList[1];
            float step = 1.0f/(float) me_GradientSteps;

            //loop 0.0 to 0.999 
            for (float i = 0.0f; i <= 1f; i += step)
            {
                if (i >= end.value)
                {
                    begin = end;
                    if (j != cList.Count)
                        end = cList[++j];
                }

                float c1r = begin.color.ToVector3().X;
                float c1g = begin.color.ToVector3().Y;
                float c1b = begin.color.ToVector3().Z;
                float c2r = end.color.ToVector3().X;
                float c2g = end.color.ToVector3().Y;
                float c2b = end.color.ToVector3().Z;

                float BegToI = i - begin.value;
                float begToEnd = end.value - begin.value;
                float perc = BegToI / begToEnd;

                float r = lerp(c1r, c2r, perc);
                float g = lerp(c1g, c2g, perc);
                float b = lerp(c1b, c2b, perc);

                colorDict[k] = new ColorVal(new Color(r, g, b), i);
                k++;
            }
            //ensure the final value is set (sometimes float loops go over)
            colorDict[me_GradientSteps-1] = new ColorVal(cList[cList.Count-1].color,1.0f);
        }

        /// <summary>
        /// linearly interpolate x between values a and b
        /// </summary>
        /// <param name="a">min value</param>
        /// <param name="b">man value</param>
        /// <param name="x">value to linearly interpolate</param>
        /// <returns></returns>
        private float lerp(float a, float b, float x)
        {
            return a * (1 - x) + b * x;
        }




        /// <summary>
        /// captures and saves the screen of the current graphics device
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public void saveScreenShot(GraphicsDevice graphicsDevice)
        {
            //setup a color buffer to get the back Buffer's data
            Color[] colors = new Color[graphicsDevice.PresentationParameters.BackBufferHeight * graphicsDevice.PresentationParameters.BackBufferWidth];

            //place the back bugger data into the color buffer
            graphicsDevice.GetBackBufferData<Color>(colors);
            
            //setup the filestream for the screenshot
            FileStream fs = new FileStream("screenshot.png", FileMode.Create);
            
            //setup the texture that will be saved
            Texture2D picTex = new Texture2D(graphicsDevice,  graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            //set the texture's color data to that of the color buffer
            picTex.SetData<Color>(colors);
            
            //save the texture to a png image file
            picTex.SaveAsPng(fs , graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            //close the file stream
            fs.Close();

            GC.Collect();
        }
    }
}
