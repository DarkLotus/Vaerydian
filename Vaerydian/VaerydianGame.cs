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
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Vaerydian.Systems;
using Vaerydian.Components;
using Vaerydian.Components.Dbg;
using Vaerydian.Factories;
using Vaerydian.Screens;

using ECSFramework;

using Glimpse.Input;
using Glimpse.Managers;

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

        private int elapsed;
        private int count = 0;
        private float avg;
		private bool changeRez = true;
		private int height = 480;
		private int width = 854;

        public VaerydianGame()
        {
            graphics = new GraphicsDeviceManager(this);
            
			this.Window.AllowUserResizing = true;
			
			graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;
            graphics.IsFullScreen = false;
            graphics.SynchronizeWithVerticalRetrace = false;
            //this.IsFixedTimeStep = false;
			//this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 1);
            graphics.PreferMultiSampling = false;
            graphics.ApplyChanges();
			
			
			this.IsMouseVisible = false;

            // add a gamer-services component, which is required for the storage APIs
            //Components.Add(new GamerServicesComponent(this));

            Content.RootDirectory = "Content";

            //give the fontManager a reference to Content
            FontManager.ContentManager = this.Content;

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            FontManager.FontsToLoad.Add("General");
            FontManager.FontsToLoad.Add("Loading");
            FontManager.FontsToLoad.Add("StartScreen");
            FontManager.FontsToLoad.Add("Damage");
            FontManager.FontsToLoad.Add("DamageBold");

            //InputManager.initialize();

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

            //windowManager.SpriteBatch = spriteBatch;

            //screenManager.WindowManager = windowManager;

            FontManager.LoadContent();

			if (!GameConfig.loadConfig ())
				InputManager.YesExit = true;

            NewLoadingScreen.Load(screenManager, false, new StartScreen());
#if DEBUG
            Console.Out.WriteLine("GAME LOADED...");
#endif
            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            FontManager.Fonts.Clear();

            for(int i = 0; i < screenManager.Screens.Count;i++)
            {
                screenManager.removeScreen(screenManager.Screens[i]);
            }
#if DEBUG
            Console.Out.WriteLine("GAME QUITTING...");
#endif
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
                //UnloadContent();
                this.Exit();
            }

			if(InputManager.isKeyToggled(Keys.F1))
				this.changeRez = true;
			
			if (changeRez) 
			{
				graphics.PreferredBackBufferHeight = height;
	            graphics.PreferredBackBufferWidth = width;

				graphics.ApplyChanges();

				changeRez = false;
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
        protected override void Draw (GameTime gameTime)
		{

			base.Draw (gameTime);

			if (InputManager.YesExit)
				return;


            
            //begin the sprite batch
            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone);

            //display performance
            spriteBatch.DrawString(FontManager.Fonts["General"], "ms / frame: " + avg, new Vector2(0), Color.Red);

            //end sprite batch
            spriteBatch.End();
            
        }
    }
}
