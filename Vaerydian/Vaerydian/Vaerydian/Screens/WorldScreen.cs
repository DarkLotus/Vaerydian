using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Vaerydian.Windows;
using Vaerydian.Maps;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using WorldGeneration.World;
using WorldGeneration.Utils;

using Glimpse.Input;

namespace Vaerydian.Screens
{

    public class ScreenViewPort
    {
        public Point Origin = new Point();
        public Point Dimensions = new Point();
    }


    public class WorldScreen : Screen
    {

        private MapEngine w_MapEngine = MapEngine.Instance;

        /// <summary>
        /// world engine reference
        /// </summary>
        //private WorldEngine ws_WorldEngine = WorldEngine.Instance;

        private Map w_Map;

        /// <summary>
        /// local SpriteBatch copy
        /// </summary>
        private SpriteBatch w_SpriteBatch;

        public override string LoadingMessage
        {
            get
            {
                return w_MapEngine.WorldGeneratorStatusMessage;
            }
        }

        private int xStart = 0;

        private int yStart = 0;

        private int xFinish = 768;

        private int yFinish = 480;

        private ScreenViewPort ws_ViewPort = new ScreenViewPort();

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

            w_SpriteBatch = ScreenManager.SpriteBatch;

            ws_ViewPort.Dimensions = new Point(this.ScreenManager.GraphicsDevice.Viewport.Width, this.ScreenManager.GraphicsDevice.Viewport.Height);

            ws_ViewPort.Origin = new Point(0, 0);

            UpdateView();

            w_MapEngine.ContentManager = this.ScreenManager.Game.Content;

            w_MapEngine.TileSize = 25;

            w_MapEngine.XTiles = ws_ViewPort.Dimensions.X;

            w_MapEngine.YTiles = ws_ViewPort.Dimensions.Y;

            w_MapEngine.WorldGenerator.generateNewWorld(0, 0, w_MapEngine.XTiles, w_MapEngine.YTiles, 5f, w_MapEngine.XTiles, w_MapEngine.YTiles, new Random().Next());

            w_MapEngine.ViewPort.Dimensions = ws_ViewPort.Dimensions;

            w_MapEngine.ViewPort.Origin = ws_ViewPort.Origin;

            w_MapEngine.Initialize();

        }

        /// <summary>
        /// loads screen content
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            //load map engine content
            w_MapEngine.LoadContent();

        }

        /// <summary>
        /// unload screen content
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
            w_MapEngine.UnloadContent();

            textures.Clear();
            
        }

        /// <summary>
        /// handles all screen related input
        /// </summary>
        public override void hasFocusUpdate(GameTime gameTime)
        {
            base.hasFocusUpdate(gameTime);

            //check to see if escape was recently pressed
            if (InputManager.isKeyToggled(Keys.Escape))
            {
                this.ScreenManager.removeScreen(this);
                LoadingScreen.Load(this.ScreenManager, false, new StartScreen());
            }

            if (InputManager.isKeyToggled(Keys.R))
            {
                this.ScreenManager.removeScreen(this);
                LoadingScreen.Load(this.ScreenManager, true, new WorldScreen());
            }

            if (InputManager.isKeyPressed(Keys.Up))
            {
                w_MapEngine.ViewPort.Origin.Y -= 25;

                ws_ViewPort.Origin.Y -= 25;
                UpdateView();
            }
            if (InputManager.isKeyPressed(Keys.Down))
            {
                w_MapEngine.ViewPort.Origin.Y += 25;

                ws_ViewPort.Origin.Y += 25;
                UpdateView();
            }
            if (InputManager.isKeyPressed(Keys.Left))
            {
                w_MapEngine.ViewPort.Origin.X -= 25;
                
                ws_ViewPort.Origin.X -= 25;
                UpdateView();
            }
            if (InputManager.isKeyPressed(Keys.Right))
            {
                w_MapEngine.ViewPort.Origin.X += 25;

                ws_ViewPort.Origin.X += 25;
                UpdateView();
            }
            
            if (InputManager.isKeyToggled(Keys.PrintScreen))
            {
                InputManager.YesScreenshot = true;
            }
            
            if (InputManager.isKeyToggled(Keys.T))
            {
                if (!w_MapEngine.ShowTemperature)
                {
                    w_MapEngine.ShowTemperature = true;
                    w_MapEngine.ShowPrecipitation = false;
                }
                else
                {
                    w_MapEngine.ShowTemperature = false;
                }
            }
            if(InputManager.isKeyToggled(Keys.P))
            {
                if (!w_MapEngine.ShowPrecipitation)
                {
                    w_MapEngine.ShowPrecipitation = true;
                    w_MapEngine.ShowTemperature = false;
                }
                else
                {
                    w_MapEngine.ShowPrecipitation = false;
                }
            }
            if (InputManager.isKeyToggled(Keys.PrintScreen))
            {
                w_MapEngine.YesScreenshot = true;
            }
            

        }

        /// <summary>
        /// update the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            w_MapEngine.UpdateView();
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

        }

        #region drawing

        /// <summary>
        /// draw the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            w_MapEngine.DrawMap(gameTime, w_SpriteBatch);

        }

        #endregion

    }
}
