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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vaerydian;

namespace Vaerydian.Screens
{
    /// <summary>
    /// manages all the currently running screens
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {

        private GameContainer s_GameContainer = new GameContainer();

        public GameContainer GameContainer
        {
            get { return s_GameContainer; }
            set { s_GameContainer = value; }
        }

        /// <summary>
        /// screens currently managed by the screen manager
        /// </summary>
        private List<Screen> sm_Screens = new List<Screen>();
        /// <summary>
        /// screens currently managed by the screen manager
        /// </summary>
        public List<Screen> Screens
        {
            get { return sm_Screens; }
            set { sm_Screens = value; }
        }

        /// <summary>
        /// screens that can be updated
        /// </summary>
        private List<Screen> sm_UpdatableScreens = new List<Screen>();

        
        /// <summary>
        /// screen manager constructor
        /// </summary>
        /// <param name="game"></param>
        public ScreenManager(Game game) : base(game) 
        {
            //this.GraphicsDevice = game.GraphicsDevice;
        }

        /// <summary>
        /// the spritebatch for drawing
        /// </summary>
        private SpriteBatch sm_spriteBatch;
        /// <summary>
        /// the spritebatch for drawing
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return sm_spriteBatch; }
            set { sm_spriteBatch = value; }
        }

        /// <summary>
        /// initializes the screen manager
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// loads the screen manager content
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// unloads content
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// updates the game
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < sm_Screens.Count; i++)
                sm_UpdatableScreens.Add(sm_Screens[i]);

            for (int i = 0; i < sm_UpdatableScreens.Count; i++)
            {
                if (sm_UpdatableScreens[i].ScreenState == ScreenState.Active)
                {
                    sm_UpdatableScreens[i].Update(gameTime);

                    //screen has focus, so run the focus update
                    if ((i + 1) == sm_UpdatableScreens.Count)
                        sm_UpdatableScreens[i].hasFocusUpdate(gameTime);
                }
            }

            //remove it from the list since its been updated
            sm_UpdatableScreens.Clear();
        }

        /// <summary>
        /// draws the game
        /// </summary>
        /// <param name="gameTime"></param>
        public override void  Draw(GameTime gameTime)
        {
 	        base.Draw(gameTime);

            for (int i = 0; i < sm_Screens.Count; i++)
            {
                if (sm_Screens[i].ScreenState == ScreenState.Inactive)
                    continue;

                sm_Screens[i].Draw(gameTime);
            }

        }

        /// <summary>
        /// adds a screen to the screen list
        /// </summary>
        /// <param name="screen">screen to add</param>
        public void addScreen(Screen screen)
        {
            screen.ScreenState = ScreenState.Active;
            screen.ScreenManager = this;
            screen.Initialize();
            screen.LoadContent();
            sm_Screens.Add(screen);
        }

		public void addLoadedScreen (Screen screen)
		{
			screen.ScreenState = ScreenState.Active;
			screen.ScreenManager = this;
			sm_Screens.Add(screen);
		}

        /// <summary>
        /// removes a screen from the screen list
        /// </summary>
        /// <param name="screen">screen to remove</param>
        public void removeScreen(Screen screen)
        {
            screen.UnloadContent();
            sm_Screens.Remove(screen);
            GC.Collect();
        }
    }
}
