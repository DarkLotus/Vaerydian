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

using Vaerydian.Maps;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Vaerydian.Utils;

using Glimpse.Input;
using Vaerydian.Generators;

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

        private ScreenViewPort w_ViewPort = new ScreenViewPort();

        private int ws_TileSize = 32;

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

			w_SpriteBatch = new SpriteBatch(this.ScreenManager.GraphicsDevice);

            w_ViewPort.Dimensions = new Point(this.ScreenManager.GraphicsDevice.Viewport.Width, this.ScreenManager.GraphicsDevice.Viewport.Height);

            w_ViewPort.Origin = new Point(0, 0);

            UpdateView();

            w_MapEngine.ContentManager = this.ScreenManager.Game.Content;

            w_MapEngine.TileSize = 5;// ws_TileSize;

            w_MapEngine.XTiles = w_ViewPort.Dimensions.X;

            w_MapEngine.YTiles = w_ViewPort.Dimensions.Y;

            //quick test hack
            Map map = MapMaker.create(w_MapEngine.XTiles, w_MapEngine.YTiles);

            object[] parameters = new object[WorldGen.WORLD_PARAMS_SIZE];

            parameters[WorldGen.WORLD_PARAMS_X] = 0;
            parameters[WorldGen.WORLD_PARAMS_Y] = 0;
            parameters[WorldGen.WORLD_PARAMS_DX] = w_ViewPort.Dimensions.X;
            parameters[WorldGen.WORLD_PARAMS_DY] = w_ViewPort.Dimensions.Y;
            parameters[WorldGen.WORLD_PARAMS_Z] = 5f;
            parameters[WorldGen.WORLD_PARAMS_XSIZE] = w_MapEngine.XTiles;
            parameters[WorldGen.WORLD_PARAMS_YSIZE] = w_MapEngine.YTiles;
            parameters[WorldGen.WORLD_PARAMS_SEED] = new Random().Next();

            MapMaker.Parameters = parameters;

			MapMaker.generate(map, MapType.WORLD);

			w_MapEngine.Map = map;
            //end quick test hack

            //w_MapEngine.WorldGenerator.generateNewWorld(0, 0, w_MapEngine.XTiles, w_MapEngine.YTiles, 5f, w_MapEngine.XTiles, w_MapEngine.YTiles, new Random().Next());

            w_MapEngine.ViewPort.Dimensions = w_ViewPort.Dimensions;

            w_MapEngine.ViewPort.Origin = w_ViewPort.Origin;

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

        private int MOVE_VALUE = 15;

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
                NewLoadingScreen.Load(this.ScreenManager, false, new StartScreen());
            }

            if (InputManager.isKeyToggled(Keys.R))
            {
                this.ScreenManager.removeScreen(this);
                NewLoadingScreen.Load(this.ScreenManager, true, new WorldScreen());
            }

            if (InputManager.isKeyPressed(Keys.Up))
            {
                w_MapEngine.ViewPort.Origin.Y -= MOVE_VALUE;

                w_ViewPort.Origin.Y -= MOVE_VALUE;
                UpdateView();
            }
            if (InputManager.isKeyPressed(Keys.Down))
            {
                w_MapEngine.ViewPort.Origin.Y += MOVE_VALUE;

                w_ViewPort.Origin.Y += MOVE_VALUE;
                UpdateView();
            }
            if (InputManager.isKeyPressed(Keys.Left))
            {
                w_MapEngine.ViewPort.Origin.X -= MOVE_VALUE;

                w_ViewPort.Origin.X -= MOVE_VALUE;
                UpdateView();
            }
            if (InputManager.isKeyPressed(Keys.Right))
            {
                w_MapEngine.ViewPort.Origin.X += MOVE_VALUE;

                w_ViewPort.Origin.X += MOVE_VALUE;
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
            xStart = w_ViewPort.Origin.X / ws_TileSize;
            if (xStart <= 0)
                xStart = 0;

            xFinish = (w_ViewPort.Origin.X + w_ViewPort.Dimensions.X) / ws_TileSize;
            if (xFinish >= w_MapEngine.XTiles - 1)
                xFinish = w_MapEngine.XTiles - 1;

            yStart = w_ViewPort.Origin.Y / ws_TileSize;
            if (yStart <= 0)
                yStart = 0;

            yFinish = (w_ViewPort.Origin.Y + w_ViewPort.Dimensions.Y) / ws_TileSize;
            if (yFinish >= w_MapEngine.YTiles - 1)
                yFinish = w_MapEngine.YTiles - 1;

        }

        #region drawing

        /// <summary>
        /// draw the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

			this.ScreenManager.GraphicsDevice.Clear(Color.Black);

            w_MapEngine.DrawMap(gameTime, w_SpriteBatch);

        }

        #endregion

    }
}
