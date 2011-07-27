using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaerydian.Maps;
using Vaerydian.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Vaerydian.Screens
{
    class WorldScreen : Screen
    {

        private MapEngine ws_MapEngine = MapEngine.Instance;

        private DialogWindow ws_DialogWindow;

        private SpriteBatch ws_SpriteBatch;

        public override string LoadingMessage
        {
            get
            {
                return ws_MapEngine.WorldGeneratorStatusMessage;
            }
        }

        /// <summary>
        /// perform any needed initialization
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            ws_SpriteBatch = ScreenManager.SpriteBatch;

            ws_MapEngine.TileSize = 5;
            
            ws_MapEngine.XTiles = 255;

            ws_MapEngine.YTiles = 255;

            ws_MapEngine.WorldGenerator.generateNewWorld(ws_MapEngine.XTiles, ws_MapEngine.YTiles, ws_MapEngine.TileSize, 6f);

            ws_MapEngine.ViewPort.Dimensions = new Point(1024, 720);

            ws_MapEngine.ViewPort.Origin = new Point(0, 0);

            ws_MapEngine.Initialize();
        }

        /// <summary>
        /// loads screen content
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            //load map engine content
            ws_MapEngine.LoadContent();

        }

        /// <summary>
        /// unload screen content
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// handles all screen related input
        /// </summary>
        public override void handleInput(GameTime gameTime)
        {
            base.handleInput(gameTime);

            //check to see if escape was recently pressed
            if (InputManager.isKeyToggled(Keys.Escape))
                InputManager.yesExit = true;

            if (InputManager.isKeyPressed(Keys.Up))
            {
                ws_MapEngine.ViewPort.Origin.Y -= 25;
                ws_MapEngine.UpdateView();
            }
            if (InputManager.isKeyPressed(Keys.Down))
            {
                ws_MapEngine.ViewPort.Origin.Y += 25;
                ws_MapEngine.UpdateView();
            }
            if (InputManager.isKeyPressed(Keys.Left))
            {
                ws_MapEngine.ViewPort.Origin.X -= 25;
                ws_MapEngine.UpdateView();
            }
            if (InputManager.isKeyPressed(Keys.Right))
            {
                ws_MapEngine.ViewPort.Origin.X += 25;
                ws_MapEngine.UpdateView();
            }
            if (InputManager.isKeyToggled(Keys.Q))
            {
                this.ScreenManager.WindowManager.addWindow(new TimedDialogWindow("This Is A Timed Dialog Box\nThis Box Stays Here 3 seconds", new Point(100, 100), new Point(400, 150), gameTime.TotalGameTime.Seconds, 3));
            }
            if (InputManager.isKeyToggled(Keys.W))
            {
                ws_DialogWindow = new DialogWindow("This is a Dialog Box\nThis Box Stays Here Permanently", new Point(200, 200), new Point(400, 150));
                this.ScreenManager.WindowManager.addWindow(ws_DialogWindow);
            }
            if (InputManager.isKeyToggled(Keys.E))
            {
                ws_DialogWindow.killWindow();
            }
            if (InputManager.isKeyToggled(Keys.R))
            {
                this.ScreenManager.WindowManager.Windows.Clear();
            }
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


        }

        /// <summary>
        /// update the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ws_MapEngine.UpdateView();
        }


        /// <summary>
        /// draw the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ws_MapEngine.DrawMap(gameTime, ws_SpriteBatch);

            ws_SpriteBatch.Begin();

            ws_SpriteBatch.DrawString(FontManager.Instance.Fonts["General"], "WorldScreen", Vector2.Zero, Color.Red);

            ws_SpriteBatch.End();
        }



    }
}
