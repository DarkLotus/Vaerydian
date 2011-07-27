using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vaerydian.Windows
{
    /// <summary>
    /// this class manages all active windows
    /// </summary>
    public class WindowManager : DrawableGameComponent
    {
        /// <summary>
        /// current list of managed windows
        /// </summary>
        private List<Window> wm_Windows = new List<Window>();
        /// <summary>
        /// current list of managed windows
        /// </summary>
        public List<Window> Windows
        {
            get { return wm_Windows; }
            set { wm_Windows = value; }
        }
        /// <summary>
        /// current list of windows that are to be updated
        /// </summary>
        private List<Window> wm_UpdatableWindows = new List<Window>();
        
        /// <summary>
        /// private sprite batch copy
        /// </summary>
        private SpriteBatch w_spriteBatch;
        /// <summary>
        /// sprite batch copy
        /// </summary>
        public SpriteBatch SpriteBatch { get { return w_spriteBatch; } set { w_spriteBatch = value; } }

        public WindowManager(Game game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// update the active windows
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Window window in wm_Windows)
                wm_UpdatableWindows.Add(window);

            foreach (Window window in wm_UpdatableWindows)
            {
                if (window.WindowState == WindowState.Active)
                    window.Update(gameTime);
            }

            wm_UpdatableWindows.Clear();
        }

        /// <summary>
        /// draw the active windows
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (Window window in wm_Windows)
            {
                if (window.WindowState == WindowState.Active)
                    window.Draw(gameTime);
            }
        }

        /// <summary>
        /// adds the window to the managed windows, activates it, loads its content, and initializes it
        /// </summary>
        /// <param name="window">the window to add</param>
        public void addWindow(Window window)
        {
            window.WindowState = WindowState.Active;
            window.WindowManager = this;
            window.LoadContent();
            window.Initialize();
            wm_Windows.Add(window);
        }

        /// <summary>
        /// removes the window from the managed windows
        /// </summary>
        /// <param name="window">the window to be removed</param>
        public void removeWindow(Window window)
        {
            window.UnloadContent();
            wm_Windows.Remove(window);
        }
    }
}
