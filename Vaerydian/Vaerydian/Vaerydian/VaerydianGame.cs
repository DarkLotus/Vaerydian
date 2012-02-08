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

        //update systems
        private EntitySystem playerMovementSystem;
        private EntitySystem cameraFocusSystem;
        private EntitySystem mousePointerSystem;
        private EntitySystem behaviorSystem;
        private EntitySystem mapCollisionSystem;

        //draw systems
        private EntitySystem spriteRenderSystem;
        private EntitySystem caveMapSystem;

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

            //create & register systems
            //register update systems
            playerMovementSystem = ecsInstance.SystemManager.setSystem(new PlayerInputSystem(), new Position(), new Velocity(), new Controllable());
            cameraFocusSystem = ecsInstance.SystemManager.setSystem(new CameraFocusSystem(), new CameraFocus(), new Position());
            mousePointerSystem = ecsInstance.SystemManager.setSystem(new MousePointerSystem(), new Position(), new MousePosition());
            behaviorSystem = ecsInstance.SystemManager.setSystem(new BehaviorSystem(), new AiBehavior());
            mapCollisionSystem = ecsInstance.SystemManager.setSystem(new MapCollisionSystem(), new MapCollidable());
            //register render systems
            spriteRenderSystem = ecsInstance.SystemManager.setSystem(new SpriteRenderSystem(gameContainer), new Position(), new Sprite());
            caveMapSystem = ecsInstance.SystemManager.setSystem(new MapSystem(gameContainer), new GameMap());
            

            //any additional component registration
            ecsInstance.ComponentManager.registerComponentType(new ViewPort());
            ecsInstance.ComponentManager.registerComponentType(new MousePosition());
            ecsInstance.ComponentManager.registerComponentType(new Heading());
            ecsInstance.ComponentManager.registerComponentType(new MapDebug());

            //initialize all systems
            ecsInstance.SystemManager.initializeSystems();

            //create the entity factory
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

            //load early entities
            //entityFactory.createBackground();
            entityFactory.createPlayer();
            entityFactory.createCamera();
            entityFactory.createMousePointer();

            entityFactory.createFollower(new Vector2(500, 370), ecsInstance.TagManager.getEntityByTag("PLAYER"), 50);
            //entityFactory.createFollower(new Vector2(150, 250), ecsInstance.TagManager.getEntityByTag("PLAYER"), 100);
            //entityFactory.createFollower(new Vector2(250, 350), ecsInstance.TagManager.getEntityByTag("PLAYER"), 150);
            //entityFactory.createFollower(new Vector2(350, 450), ecsInstance.TagManager.getEntityByTag("PLAYER"), 200);

            //create cave
            //entityFactory.createCave();
            entityFactory.CreateTestMap();

            //create map debug
            entityFactory.createMapDebug();

            //load fonts
            fontManager.LoadContent();
            
            //early entity reslove
            ecsInstance.resolveEntities();

            //load system content
            ecsInstance.SystemManager.systemsLoadContent();
            
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
            //update all input
            InputManager.Update();

            if (InputManager.YesExit)
                this.Exit();

            if (InputManager.isKeyToggled(Keys.PrintScreen))
                InputManager.YesScreenshot = true;

            //calculate ms/s
            elapsed += gameTime.ElapsedGameTime.Milliseconds;

            avg = (avg + gameTime.ElapsedGameTime.Milliseconds) / 2;

            if (elapsed > 1000)
            {
                disp = avg;
                avg = 0;
                elapsed = 0;
            }

            //update time
            ecsInstance.TotalTime = gameTime.TotalGameTime.Milliseconds;
            ecsInstance.ElapsedTime = gameTime.ElapsedGameTime.Milliseconds;

            //resolve any entity updates as needed
            ecsInstance.resolveEntities();

            //process systems
            playerMovementSystem.process();
            cameraFocusSystem.process();
            mousePointerSystem.process();
            behaviorSystem.process();

            mapCollisionSystem.process();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            //begin the sprite batch
            spriteBatch.Begin();
            
            //run draw systems
            caveMapSystem.process();
            spriteRenderSystem.process();

            //display performance
            spriteBatch.DrawString(FontManager.Instance.Fonts["General"], "ms / frame: " + disp, new Vector2(0), Color.Red);

            //end sprite batch
            spriteBatch.End();

            if (InputManager.YesScreenshot)
            {
                saveScreenShot(GraphicsDevice);
                InputManager.YesScreenshot = false;
            }

            base.Draw(gameTime);
        }


        /// <summary>
        /// captures and saves the screen of the current graphics device
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public void saveScreenShot(GraphicsDevice graphicsDevice)
        {
            //setup a color buffer to get the back Buffer's data
            Color[] colors = new Color[graphicsDevice.PresentationParameters.BackBufferHeight * graphicsDevice.PresentationParameters.BackBufferWidth];

            //place the back bugger data into the color buffer
            graphicsDevice.GetBackBufferData<Color>(colors);

            //setup the filestream for the screenshot
            FileStream fs = new FileStream("screenshot.png", FileMode.Create);

            //setup the texture that will be saved
            Texture2D picTex = new Texture2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            //set the texture's color data to that of the color buffer
            picTex.SetData<Color>(colors);

            //save the texture to a png image file
            picTex.SaveAsPng(fs, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            //close the file stream
            fs.Close();

            GC.Collect();
        }
    }
}
