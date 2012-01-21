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
using Vaerydian.Factories;

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
        private FontManager fontManager = FontManager.Instance;

        private GameContainer gameContainer = new GameContainer();

        private ECSInstance ecsInstance;

        private EntitySystem playerMovementSystem;
        private EntitySystem spriteRenderSystem;
        private EntitySystem cameraFocusSystem;

        private EntityFactory entityFactory;

        private int avg, disp, elapsed;

        public VaerydianGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 675;
            graphics.PreferredBackBufferWidth = 1080;
            graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = true;

            // add a gamer-services component, which is required for the storage APIs
            //Components.Add(new GamerServicesComponent(this));

            Content.RootDirectory = "Content";

            //give the fontManager a reference to Content
            fontManager.ContentManager = this.Content;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            ecsInstance = new ECSInstance();

            gameContainer.SpriteBatch = spriteBatch;
            gameContainer.ContentManager = Content;

            //load systems
            playerMovementSystem = ecsInstance.SystemManager.setSystem(new PlayerInputSystem(), new Position(), new Velocity(), new Controllable());
            spriteRenderSystem = ecsInstance.SystemManager.setSystem(new SpriteRenderSystem(gameContainer), new Position(), new Sprite());
            cameraFocusSystem = ecsInstance.SystemManager.setSystem(new CameraFocusSystem(), new CameraFocus(), new Position());
            
            //any additional component registration
            ecsInstance.ComponentManager.registerComponentType(new ViewPort());

            ecsInstance.SystemManager.initializeSystems();

            entityFactory = new EntityFactory(ecsInstance);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            entityFactory.createBackground();
            entityFactory.createPlayer();
            entityFactory.createCamera();
            

            // TODO: use this.Content to load your game content here
            fontManager.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            fontManager.Fonts.Clear();
            GC.Collect();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (InputManager.yesExit)
                this.Exit();

            InputManager.Update();

            //calculate ms/s
            elapsed += gameTime.ElapsedGameTime.Milliseconds;

            avg = (avg + gameTime.ElapsedGameTime.Milliseconds) / 2;

            if (elapsed > 1000)
            {
                disp = avg;
                avg = 0;
                elapsed = 0;
            }

            //update entities as needed
            ecsInstance.resolveEntities();

            //run update systems
            playerMovementSystem.process();
            cameraFocusSystem.process();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            
            //run draw systems
            spriteRenderSystem.process();

            //display performance
            spriteBatch.DrawString(FontManager.Instance.Fonts["General"], "ms / frame: " + disp, new Vector2(0), Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }


        
    }
}
