using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vaerydian.Windows;

namespace Vaerydian.Screens
{
    /// <summary>
    /// manages all the currently running screens
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {

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
        public ScreenManager(Game game) : base(game) { }

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
        /// reference to the window manager
        /// </summary>
        private WindowManager sm_WindowManager;
        /// <summary>
        /// reference to the window manager
        /// </summary>
        public WindowManager WindowManager
        {
            get { return sm_WindowManager; }
            set { sm_WindowManager = value; }
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

            foreach (Screen screen in sm_Screens)
                sm_UpdatableScreens.Add(screen);
            
            foreach (Screen screen in sm_UpdatableScreens)
            {
                if (screen.ScreenState == ScreenState.Active)
                {
                    //have the screen update itself
                    screen.Update(gameTime);

                    //if this screen is on the top of the list, allow it to handle input
                    if ((sm_UpdatableScreens.IndexOf(screen) + 1) == sm_UpdatableScreens.Count)
                        screen.handleInput(gameTime);
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

            foreach (Screen screen in sm_Screens)
            {   //only draw active screens
                if (screen.ScreenState == ScreenState.Inactive)
                    continue;

                screen.Draw(gameTime);
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
            screen.LoadContent();
            screen.Initialize();
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
        }
    }
}
