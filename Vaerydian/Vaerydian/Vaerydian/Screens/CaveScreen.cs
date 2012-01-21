using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WorldGeneration.Cave;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Vaerydian.Screens
{
    public class CaveScreen : Screen
    {

        private SpriteBatch cs_SpriteBatch;

        private int cs_xStart = 0;

        private int cs_yStart = 0;

        private int cs_xFinish = 1024;

        private int cs_yFinish = 640;

        private ViewPort cs_ViewPort = new ViewPort();

        private int cs_TileSize = 5;

        private CaveEngine cs_CaveEngine = new CaveEngine();

        private CaveTerrain cs_CaveTerrain;

        /// <summary>
        /// list of usuable textures
        /// </summary>
        private List<Texture2D> textures = new List<Texture2D>();

        public override string LoadingMessage
        {
            get
            {
                return cs_CaveEngine.StatusMessage;
            }
            set
            {
                base.LoadingMessage = value;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            cs_SpriteBatch = ScreenManager.SpriteBatch;

            cs_CaveEngine.generateCave(100, 100, new Random().Next(0,1000), 3, 0.5);

            cs_ViewPort.Dimensions = new Point(1024, 640);

            cs_ViewPort.Origin = new Point(0, 0);

            UpdateView();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\cascade"));//0
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\peak"));//1
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void handleInput(GameTime gameTime)
        {
            base.handleInput(gameTime);

            if(InputManager.isKeyToggled(Keys.Escape))
            {
                this.ScreenManager.removeScreen(this);
                LoadingScreen.Load(this.ScreenManager, false, new StartScreen());
            }
            else if (InputManager.isKeyToggled(Keys.PrintScreen))
            {
                InputManager.YesScreenshot = true;
            }else if(InputManager.isKeyToggled(Keys.G))
            {
                cs_CaveEngine.generateCave(100, 100, new Random().Next(0, 1000), 3, 0.5);
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// updates the tile indexes based on current viewport for the draw loop
        /// </summary>
        public void UpdateView()
        {
            cs_xStart = cs_ViewPort.Origin.X / cs_TileSize;
            if (cs_xStart <= 0)
                cs_xStart = 0;

            cs_xFinish = (cs_ViewPort.Origin.X + cs_ViewPort.Dimensions.X) / cs_TileSize;
            if (cs_xFinish >= 1024 - 1)
                cs_xFinish = 1024 - 1;

            cs_yStart = cs_ViewPort.Origin.Y / cs_TileSize;
            if (cs_yStart <= 0)
                cs_yStart = 0;

            cs_yFinish = (cs_ViewPort.Origin.Y + cs_ViewPort.Dimensions.Y) / cs_TileSize;
            if (cs_yFinish >= 1024 - 1)
                cs_yFinish = 1024 - 1;
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            cs_SpriteBatch.Begin();

            for (int x = cs_xStart; x <= cs_xFinish; x++)
            {
                for (int y = cs_yStart; y <= cs_yFinish; y++)
                {
                    cs_CaveTerrain = cs_CaveEngine.getTerrain(x, y);

                    if (cs_CaveTerrain == null)
                        continue;

                    if (cs_CaveTerrain.BaseCaveType == CaveType.Floor)
                    {
                        cs_SpriteBatch.Draw(textures[0], new Vector2((x * cs_TileSize), (y * cs_TileSize)),
                                null, Color.White, 0.0f, new Vector2(cs_ViewPort.Origin.X, cs_ViewPort.Origin.Y), new Vector2(1f),
                                SpriteEffects.None, 0);
                    }
                    else
                    {
                        cs_SpriteBatch.Draw(textures[1], new Vector2((x * cs_TileSize), (y * cs_TileSize)),
                                null, Color.White, 0.0f, new Vector2(cs_ViewPort.Origin.X, cs_ViewPort.Origin.Y), new Vector2(1f),
                                SpriteEffects.None, 0);
                    }
                }
            }

            cs_SpriteBatch.End();

            if (InputManager.YesScreenshot)
            {
                InputManager.YesScreenshot = false;

                saveScreenShot(cs_SpriteBatch.GraphicsDevice);
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

    }
}
