using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Vaerydian.Maps;
using Vaerydian.Windows;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using WorldGeneration.World;


namespace Vaerydian.Screens
{

    public class ViewPort
    {
        public Point Origin = new Point();
        public Point Dimensions = new Point();
    }


    public class WorldScreen : Screen
    {

        //private MapEngine ws_MapEngine = MapEngine.Instance;

        /// <summary>
        /// world engine reference
        /// </summary>
        //private WorldEngine ws_WorldEngine = WorldEngine.Instance;

        private WorldTerrain ws_Terrain;

        /// <summary>
        /// local SpriteBatch copy
        /// </summary>
        private SpriteBatch ws_SpriteBatch;

        public override string LoadingMessage
        {
            get
            {
                return "";// ws_WorldEngine.WorldGeneratorStatusMessage;
            }
        }

        private int xStart = 0;

        private int yStart = 0;

        private int xFinish = 1024;

        private int yFinish = 640;

        private ViewPort ws_ViewPort = new ViewPort();

        private int ws_TileSize = 1;

        /// <summary>
        /// list of usuable textures
        /// </summary>
        private List<Texture2D> textures = new List<Texture2D>();


        /// <summary>
        /// perform any needed initialization
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            ws_SpriteBatch = ScreenManager.SpriteBatch;

            ws_ViewPort.Dimensions = new Point(1024, 640);

            ws_ViewPort.Origin = new Point(0, 0);

            //ws_WorldEngine.createNewWorld();

            //ws_WorldEngine.Initialize();

            UpdateView();

            /*
            ws_MapEngine.TileSize = 5;
            
            ws_MapEngine.XTiles = 1024;

            ws_MapEngine.YTiles = 1024;

            ws_MapEngine.WorldGenerator.generateNewWorld(0, 0, ws_MapEngine.XTiles, ws_MapEngine.YTiles, 5f, 1024, new Random().Next());

            ws_MapEngine.ViewPort.Dimensions = new Point(1024, 640);

            ws_MapEngine.ViewPort.Origin = new Point(0, 0);

            ws_MapEngine.Initialize();
            */
        }

        /// <summary>
        /// loads screen content
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            //load map engine content
            //ws_MapEngine.LoadContent();
            
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\grass"));//0
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\ocean"));//1
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\mountains"));//2
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\arctic"));//3
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\beach"));//4
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\forest"));//5
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\grasslands"));//6
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\jungle"));//7
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\desert"));//8
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\swamp"));//9
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\tundra"));//10
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\foothills"));//11
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\steppes"));//12
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\cascade"));//13
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\peak"));//14
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\littoral"));//15
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\abyssal"));//16
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\ice"));//17
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\sublittoral"));//18
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("characters\\player"));//19player texture
        }

        /// <summary>
        /// unload screen content
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
            //ws_MapEngine.UnloadContent();

            textures.Clear();
            
        }

        /// <summary>
        /// handles all screen related input
        /// </summary>
        public override void handleInput(GameTime gameTime)
        {
            base.handleInput(gameTime);

            //check to see if escape was recently pressed
            if (InputManager.isKeyToggled(Keys.Escape))
            {
                this.ScreenManager.removeScreen(this);
                LoadingScreen.Load(this.ScreenManager, false, new StartScreen());
            }

            
            if (InputManager.isKeyPressed(Keys.Up))
            {
                ws_ViewPort.Origin.Y -= 25;
                UpdateView();
            }
            if (InputManager.isKeyPressed(Keys.Down))
            {
                ws_ViewPort.Origin.Y += 25;
                UpdateView();
            }
            if (InputManager.isKeyPressed(Keys.Left))
            {
                ws_ViewPort.Origin.X -= 25;
                UpdateView();
            }
            if (InputManager.isKeyPressed(Keys.Right))
            {
                ws_ViewPort.Origin.X += 25;
                UpdateView();
            }
            
            if (InputManager.isKeyToggled(Keys.PrintScreen))
            {
                InputManager.YesScreenshot = true;
            }
            /*
            if (InputManager.isKeyToggled(Keys.T))
            {
                if (!ws_MapEngine.ShowTemperature)
                {
                    ws_MapEngine.ShowTemperature = true;
                    ws_MapEngine.ShowPrecipitation = false;
                }
                else
                {
                    ws_MapEngine.ShowTemperature = false;
                }
            }
            if(InputManager.isKeyToggled(Keys.P))
            {
                if (!ws_MapEngine.ShowPrecipitation)
                {
                    ws_MapEngine.ShowPrecipitation = true;
                    ws_MapEngine.ShowTemperature = false;
                }
                else
                {
                    ws_MapEngine.ShowPrecipitation = false;
                }
            }
            if (InputManager.isKeyToggled(Keys.PrintScreen))
            {
                ws_MapEngine.YesScreenshot = true;
            }
            */

        }

        /// <summary>
        /// update the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //ws_MapEngine.UpdateView();
        }

        /// <summary>
        /// updates the tile indexes based on current viewport for the draw loop
        /// </summary>
        public void UpdateView()
        {
            xStart = ws_ViewPort.Origin.X / ws_TileSize;
            if (xStart <= 0)
                xStart = 0;

            xFinish = (ws_ViewPort.Origin.X + ws_ViewPort.Dimensions.X) / ws_TileSize;
            if (xFinish >= 1024 - 1)
                xFinish = 1024 - 1;

            yStart = ws_ViewPort.Origin.Y / ws_TileSize;
            if (yStart <= 0)
                yStart = 0;

            yFinish = (ws_ViewPort.Origin.Y + ws_ViewPort.Dimensions.Y) / ws_TileSize;
            if (yFinish >= 1024 - 1)
                yFinish = 1024 - 1;

            //Point p = ws_WorldEngine.updateSegments(new Point(xStart + (xFinish-xStart)/2, yStart + (yFinish - yStart)/2));
            //ws_ViewPort.Origin.X += p.X * 128;
            //ws_ViewPort.Origin.Y += p.Y * 128;
        }

        #region drawing

        /// <summary>
        /// draw the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //ws_MapEngine.DrawMap(gameTime, ws_SpriteBatch);

            ws_SpriteBatch.Begin();

            for (int x = xStart; x <= xFinish; x++)
            {
                for (int y = yStart; y <= yFinish; y++)
                {
                    //ws_Terrain = ws_WorldEngine.getTerrain(x, y);

                    if (ws_Terrain == null)
                        continue;

                    ws_SpriteBatch.Draw(textures[getBaseTexture(ws_Terrain)], new Vector2((x * ws_TileSize), (y * ws_TileSize)),
                            null, Color.White, 0.0f, new Vector2(ws_ViewPort.Origin.X, ws_ViewPort.Origin.Y), new Vector2(1f),
                            SpriteEffects.None, 0);
                }
            }


            drawPlayer();

            ws_SpriteBatch.End();

            if (InputManager.YesScreenshot)
            {
                InputManager.YesScreenshot = false;

                saveScreenShot(ws_SpriteBatch.GraphicsDevice);
            }
        }

        private void drawPlayer()
        {
            ws_SpriteBatch.Draw(textures[19], new Vector2(ws_ViewPort.Origin.X + ws_ViewPort.Dimensions.X / 2, ws_ViewPort.Origin.Y + ws_ViewPort.Dimensions.Y / 2),
                            null, Color.White, 0.0f, new Vector2(ws_ViewPort.Origin.X, ws_ViewPort.Origin.Y), new Vector2(1f),
                            SpriteEffects.None, 0);
        }

        /// <summary>
        /// returns the texture id for the given terrain
        /// </summary>
        /// <param name="terrain">terrain to get the texture for</param>
        /// <returns></returns>
        private int getBaseTexture(WorldTerrain terrain)
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
                    if (terrain.OceanTerrainType == OceanTerrainType.Littoral)
                        return 15;
                    if (terrain.OceanTerrainType == OceanTerrainType.Abyssal)
                        return 16;
                    if (terrain.OceanTerrainType == OceanTerrainType.Ice)
                        return 17;
                    if (terrain.OceanTerrainType == OceanTerrainType.Sublittoral)
                        return 18;
                    return 1;
                case BaseTerrainType.Mountain:
                    if (terrain.MountainTerrainType == MountainTerrainType.Foothill)
                        return 11;
                    if (terrain.MountainTerrainType == MountainTerrainType.Steppes)
                        return 12;
                    if (terrain.MountainTerrainType == MountainTerrainType.Cascade)
                        return 13;
                    if (terrain.MountainTerrainType == MountainTerrainType.SnowyPeak)
                        return 14;
                    return 2;
                case BaseTerrainType.River:
                    return 18;
                default:
                    return 0;
            }
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
            Texture2D picTex = new Texture2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            //set the texture's color data to that of the color buffer
            picTex.SetData<Color>(colors);

            //save the texture to a png image file
            picTex.SaveAsPng(fs, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            //close the file stream
            fs.Close();

            GC.Collect();
        }

        #endregion

    }
}
