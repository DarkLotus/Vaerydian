/*
 Author:
      Thomas H. Jonell <@Net_Gnome>
 
 Copyright (c) 2013 Thomas H. Jonell

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Lesser General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Vaerydian;
using Vaerydian.Utils;
using Vaerydian.Screens;
using Vaerydian.Components.Utils;

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

		private Map map;

		public Map Map {
			get {
				return map;
			}
			set {
				map = value;
			}
		}
        Random rand = new Random();

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
            get{return MapMaker.StatusMessage;}
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

        private Texture2D terrainTexture;

        /// <summary>
        /// performs any needed map loading
        /// </summary>
        public void LoadContent()
        {
            /*
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\grass"));//0
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\ocean"));//1
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\mountains"));//2
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\arctic"));//3
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\beach"));//4
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\forest"));//5
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\grasslands"));//6
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\jungle"));//7
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\desert"));//8
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\swamp"));//9
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\tundra"));//10
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\foothills"));//11
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\steppes"));//12
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\cascade"));//13
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\peak"));//14
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\littoral"));//15
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\abyssal"));//16
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\ice"));//17
            textures.Add(me_contentManager.Load<Texture2D>("terrain\\sublittoral"));//18
            */

            terrainTexture = me_contentManager.Load<Texture2D>("terrain\\noise");
            temperatureTexture = me_contentManager.Load<Texture2D>("temperature");
            exportTexture = me_contentManager.Load<Texture2D>("export");

        }

        public void UnloadContent()
        {
            textures.Clear();
            
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
            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone);

            Terrain terrain;

            //iterate through the viewable indexes and draw the tiles
            for (int x = xStart; x <= xFinish; x++)
            {
                for (int y = yStart; y <= yFinish; y++)
                {
                    //get the terrain at this cell
                    terrain = map.Terrain[x, y];

                    if (me_ShowTemperature)
                    {
                        //draw temperature texture
                        spriteBatch.Draw(temperatureTexture, new Vector2((x * me_TileSize), (y * me_TileSize)),
                            null, shadingValue(terrain.Temperature), 0.0f, new Vector2(me_ViewPort.Origin.X, me_ViewPort.Origin.Y), new Vector2(1f),
                            SpriteEffects.None, 0);
                    }
                    else if (me_ShowPrecipitation)
                    {
                        //draw rainfall textures
                        spriteBatch.Draw(temperatureTexture, new Vector2((x * me_TileSize), (y * me_TileSize)),
                            null, shadingValue(terrain.Rainfall), 0.0f, new Vector2(me_ViewPort.Origin.X, me_ViewPort.Origin.Y), new Vector2(1f),
                            SpriteEffects.None, 0);
                    }
                    else
                    {
                        //draw terrain texture
                        /* old way
                        spriteBatch.Draw(textures[getBaseTexture(terrain)], new Rectangle((x * me_TileSize), (y * me_TileSize), me_TileSize, me_TileSize),
                            new Rectangle(0, 0, me_TileSize, me_TileSize), Color.White, 0f,
                            new Vector2(me_ViewPort.Origin.X, me_ViewPort.Origin.Y), SpriteEffects.None, 0f);
                        */

                        //new way
                        /*spriteBatch.Draw(temperatureTexture, new Rectangle((x * me_TileSize), (y * me_TileSize), me_TileSize, me_TileSize),
                            new Rectangle(0, 0, me_TileSize, me_TileSize), getColor(terrain), 0f,
                            new Vector2(me_ViewPort.Origin.X, me_ViewPort.Origin.Y), SpriteEffects.None, 0f);
						*/

						spriteBatch.Draw(terrainTexture, new Vector2((x * me_TileSize), (y * me_TileSize)) - new Vector2(me_ViewPort.Origin.X, me_ViewPort.Origin.Y),
                            new Rectangle(0, 0, me_TileSize, me_TileSize), getColorVariation(terrain), 0f, new Vector2(0,0) , new Vector2(1f), SpriteEffects.None, 0);
                    }
                }
            }

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

        private Color getColorVariation(Terrain terrain)
        {
            Color color = getColor(terrain);

            Vector3 colVec = color.ToVector3();

            colVec.X *= terrain.Variation;
            colVec.Y *= terrain.Variation;
            colVec.Z *= terrain.Variation;


            return new Color(colVec);
        }

        private Color getColor(Terrain terrain)
        {
            switch (terrain.TerrainType)
            {
                case TerrainType_Old.LAND_ARCTIC_DESERT:
                    return new Color(204,204,255);
                case TerrainType_Old.LAND_DESERT:
                    return new Color(204, 204, 0);
                case TerrainType_Old.LAND_SCORCHED:
                    return new Color(153,102,51);
                case TerrainType_Old.LAND_SNOW_PLAINS:
                    return Color.White;
                case TerrainType_Old.LAND_TUNDRA:
                    return new Color(53,111,53);
                case TerrainType_Old.LAND_TAIGA:
                    return new Color(24,72,48);
                case TerrainType_Old.LAND_TEMPERATE_GRASSLAND:
                    return new Color(153,255,102);
                case TerrainType_Old.LAND_SHRUBLAND:
                    return new Color(102,153,0);
                case TerrainType_Old.LAND_SAVANA:
                    return new Color(204,255,102);
                case TerrainType_Old.LAND_TEMPERATE_FOREST:
                    return new Color(0,153,0);
                case TerrainType_Old.LAND_TROPICAL_FOREST:
                    return new Color(102,255,51);
                case TerrainType_Old.LAND_GLACIER:
                    return new Color(153,255,204);
                case TerrainType_Old.LAND_MARSH:
                    return new Color(33,101,67);
                case TerrainType_Old.LAND_TEMPERATE_RAIN_FOREST:
                    return new Color(0,102,0);
                case TerrainType_Old.LAND_HYBOREAN_RIMELAND:
                    return new Color(204,255,255);
                case TerrainType_Old.LAND_BOG:
                    return new Color(51,51,0);
                case TerrainType_Old.LAND_SWAMP:
                    return new Color(0,51,0);
                case TerrainType_Old.LAND_TROPICAL_RAIN_FOREST:
                    return new Color(0,128,0);
                case TerrainType_Old.OCEAN_ICE:
                    return new Color(204,255,255);
                case TerrainType_Old.OCEAN_COAST:
                    return new Color(255,255,153);
                case TerrainType_Old.OCEAN_LITTORAL:
                    return new Color(51,153,255);
                case TerrainType_Old.OCEAN_SUBLITTORAL:
                    return new Color(0,102,255);
                case TerrainType_Old.OCEAN_ABYSSAL:
                    return new Color(0,51,204);
                case TerrainType_Old.MOUNTAIN_FOOTHILL:
                    return new Color(57,69,43);
                case TerrainType_Old.MOUNTAIN_LOWLAND:
                    return new Color(79,95,59);
                case TerrainType_Old.MOUNTAIN_HIGHLAND:
                    return new Color(115,123,105);
                case TerrainType_Old.MOUNTAIN_CASCADE:
                    return new Color(150,150,150);
                case TerrainType_Old.MOUNTAIN_DRY_PEAK:
                    return new Color(192,192,192);
                case TerrainType_Old.MOUNTAIN_SNOWY_PEAK:
                    return new Color(221,221,221);
                case TerrainType_Old.BASE_RIVER:
                    return new Color(0,102,102);
                case TerrainType_Old.CAVE_ENTRANCE:
                    return new Color(255, 0, 0);
                default:
                    return Color.Red;
            }
        }

        /// <summary>
        /// returns the texture id for the given terrain
        /// </summary>
        /// <param name="terrain">terrain to get the texture for</param>
        /// <returns></returns>
        private int getBaseTexture(Terrain terrain)
        {
            switch (terrain.TerrainType)
            {
                case TerrainType_Old.LAND_TUNDRA:
                        return 3;
                case TerrainType_Old.OCEAN_COAST:
                        return 4;
                case TerrainType_Old.LAND_DESERT:
                        return 8;
                case TerrainType_Old.LAND_TEMPERATE_FOREST:
                        return 5;
                case TerrainType_Old.LAND_TEMPERATE_GRASSLAND:
                        return 6;
                case TerrainType_Old.LAND_TROPICAL_FOREST:
                        return 7;
                case TerrainType_Old.LAND_SWAMP:
                        return 9;
                case TerrainType_Old.LAND_TAIGA:
                        return 10;
                case TerrainType_Old.OCEAN_LITTORAL:
                        return 15;
                case TerrainType_Old.OCEAN_ABYSSAL:
                        return 16;
                case TerrainType_Old.OCEAN_ICE:
                        return 17;
                case TerrainType_Old.OCEAN_SUBLITTORAL:
                        return 18;
                case TerrainType_Old.MOUNTAIN_FOOTHILL:
                        return 11;
                case TerrainType_Old.MOUNTAIN_LOWLAND:
                        return 12;
                case TerrainType_Old.MOUNTAIN_CASCADE:
                        return 13;
                case TerrainType_Old.MOUNTAIN_SNOWY_PEAK:
                        return 14;
                case TerrainType_Old.BASE_RIVER:
                    return 18;
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
             
            /*
            cList.Add(new ColorVal(new Color(0, 0, 48), -1.0f));//dark blue
            cList.Add(new ColorVal(new Color(0, 0, 64), -0.75f));
            cList.Add(new ColorVal(new Color(0, 0, 128), 0.05f));//blue
            cList.Add(new ColorVal(Color.LightBlue, 0.10f));//light blue
            cList.Add(new ColorVal(new Color(255, 255, 0), 0.15f));//beach yellow
            cList.Add(new ColorVal(new Color(96, 255, 0), 0.18f));//light green
            cList.Add(new ColorVal(new Color(0, 196, 0), 0.2f));//green
            cList.Add(new ColorVal(new Color(0, 128, 0), 0.25f));//green again
            cList.Add(new ColorVal(new Color(0, 64, 0), 0.4f));//dark green
            cList.Add(new ColorVal(new Color(96, 96, 64), 0.55f));//dark brown
            cList.Add(new ColorVal(new Color(96, 96, 96), 0.6f));//mountain dark grey
            cList.Add(new ColorVal(new Color(128, 128, 128), 0.65f));//mountain gray
            cList.Add(new ColorVal(new Color(160, 160, 160), 0.75f));//mountain light gray
            cList.Add(new ColorVal(new Color(255, 255, 255), 1.0f));//snow white
            */
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
			/*
            //setup a color buffer to get the back Buffer's data
            Color[] colors = new Color[graphicsDevice.PresentationParameters.BackBufferHeight * graphicsDevice.PresentationParameters.BackBufferWidth];

            //place the back bugger data into the color buffer
            graphicsDevice.GetBackBufferData<Color>(colors);

            string timestamp = ""+DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;

            //setup the filestream for the screenshot
            FileStream fs = new FileStream("screenshot_" + timestamp + ".png", FileMode.Create);
            
            //setup the texture that will be saved
            Texture2D picTex = new Texture2D(graphicsDevice,  graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            //set the texture's color data to that of the color buffer
            picTex.SetData<Color>(colors);
            
            //save the texture to a png image file
            picTex.SaveAsPng(fs , graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            //close the file stream
            fs.Close();

            GC.Collect();
            */
        }
    }
}
