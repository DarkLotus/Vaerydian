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
        /// handles all user related input
        /// </summary>
        public virtual void handleInput(GameTime gameTime)
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
