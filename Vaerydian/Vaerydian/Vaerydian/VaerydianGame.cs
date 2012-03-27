using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Vaerydian.Windows;
using Vaerydian.Systems;
using Vaerydian.Components;
using Vaerydian.Components.Debug;
using Vaerydian.Factories;
using Vaerydian.Screens;


using ECSFramework;

namespace Vaerydian
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class VaerydianGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GameContainer gameContainer = new GameContainer();

        private ScreenManager screenManager;
        private WindowManager windowManager;

        private int elapsed;
        private int count = 0;
        private float avg;

        public VaerydianGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = (int) (graphics.PreferredBackBufferHeight * 1.6);
            graphics.IsFullScreen = false;
            graphics.SynchronizeWithVerticalRetrace = true;
            this.IsFixedTimeStep = true;
            
            // add a gamer-services component, which is required for the storage APIs
            //Components.Add(new GamerServicesComponent(this));

            Content.RootDirectory = "Content";

            //give the fontManager a reference to Content
            FontManager.ContentManager = this.Content;

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            windowManager = new WindowManager(this);
            Components.Add(windowManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            screenManager.SpriteBatch = spriteBatch;
            screenManager.GameContainer.ContentManager = screenManager.Game.Content;
            screenManager.GameContainer.GraphicsDevice = screenManager.GraphicsDevice;
            screenManager.GameContainer.SpriteBatch = spriteBatch;

            windowManager.SpriteBatch = spriteBatch;

            screenManager.WindowManager = windowManager;

            FontManager.LoadContent();

            LoadingScreen.Load(screenManager, true, new StartScreen());

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            FontManager.Fonts.Clear();

            for(int i = 0; i < screenManager.Screens.Count;i++)
            {
                screenManager.removeScreen(screenManager.Screens[i]);
            }

            for (int i = 0; i < windowManager.Windows.Count; i++)
            {
                windowManager.removeWindow(windowManager.Windows[i]);
            }

            GC.Collect();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //update all input
            InputManager.Update();

            if (InputManager.YesExit)
            {
                UnloadContent();
                this.Exit();
            }

            //calculate ms/s
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
            count++;
            if(count > 100)
            {
                avg = (float)elapsed / (float)count;
                count = 0;
                elapsed = 0;
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);


            //begin the sprite batch
            spriteBatch.Begin();

            //display performance
            spriteBatch.DrawString(FontManager.Fonts["General"], "ms / frame: " + avg, new Vector2(0), Color.Red);

            //end sprite batch
            spriteBatch.End();
        }


    }
}
