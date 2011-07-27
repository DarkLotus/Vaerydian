using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vaerydian.Windows
{
    /// <summary>
    /// represents current state of the Window
    /// </summary>
    public enum WindowState
    {
        Active,
        Inactive
    }


    /// <summary>
    /// this class is the base class of all windows
    /// Windows can update and draw, but cannot handle input
    /// </summary>
    public abstract class Window
    {
        /// <summary>
        /// private current state of the window
        /// </summary>
        private WindowState w_WindowState = WindowState.Inactive;
        /// <summary>
        /// current state of the window
        /// </summary>
        public WindowState WindowState { get { return w_WindowState; } set { w_WindowState = value; } }

        /// <summary>
        /// private window manager
        /// </summary>
        private WindowManager w_WindowManager;
        /// <summary>
        /// current window manager
        /// </summary>
        public WindowManager WindowManager { get { return w_WindowManager; } set { w_WindowManager = value; } }

        /// <summary>
        /// initialize the window
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// any window unique content loading
        /// </summary>
        public abstract void LoadContent();

        /// <summary>
        /// any window unique content unloading
        /// </summary>
        public abstract void UnloadContent();

        /// <summary>
        /// update the window
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// draw the window
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime)
        {
        }

    }
}
