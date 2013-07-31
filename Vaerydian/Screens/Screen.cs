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

namespace Vaerydian.Screens
{
    /// <summary>
    /// sate of the screen
    /// </summary>
    public enum ScreenState
    {
        Activating,
        Deactivating,
        Active,
        Inactive
    }

    /// <summary>
    /// base screen class from which all screens inherit from
    /// </summary>
    public abstract class Screen
    {
        //private variables
        private ScreenManager s_screenManager;
        /// <summary>
        /// known screen manager
        /// </summary>
        public ScreenManager ScreenManager{ get { return s_screenManager; } set { s_screenManager = value; } }

        private ScreenState s_screenState;
        /// <summary>
        /// current state
        /// </summary>
        public ScreenState ScreenState { get { return s_screenState; } set { s_screenState = value; } }

        private bool s_hasFocus = false;
        /// <summary>
        /// if the current screen is the focused screen
        /// </summary>
        public bool HasFocus { get { return s_hasFocus; } set { s_hasFocus = value; } }

        /// <summary>
        /// message to be displayed during long load times
        /// </summary>
        private String s_LoadingMessage = "";

        /// <summary>
        /// message to be displayed during long load times
        /// </summary>
        public virtual String LoadingMessage
        {
            get { return s_LoadingMessage; }
            set { s_LoadingMessage = value; }
        }



        /// <summary>
        /// handles all updates that require the screen to have focus
        /// </summary>
        public virtual void hasFocusUpdate(GameTime gameTime)
        {
        }

        /// <summary>
        /// handles any screen initialization
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// handles any content loading
        /// </summary>
        public virtual void LoadContent()
        {
        }

        /// <summary>
        /// handles any content unloading
        /// </summary>
        public virtual void UnloadContent()
        {
        }

        /// <summary>
        /// handles any screen unique updates
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// handles any screen unique drawing
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public virtual void Draw(GameTime gameTime)
        {
        }
    }
}
