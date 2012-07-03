using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ECSFramework;

using Vaerydian;
using Vaerydian.Components;
using Vaerydian.Components.Audio;
using Vaerydian.Components.Characters;
using Vaerydian.Components.Debug;
using Vaerydian.Components.Items;
using Vaerydian.Factories;
using Vaerydian.Systems;
using Vaerydian.Systems.Draw;
using Vaerydian.Systems.Update;

using Glimpse.Input;
using Glimpse.Systems;
using Glimpse.Components;
using Glimpse.Managers;
using Vaerydian.Components.Actions;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Graphical;
using Vaerydian.Components.Action;

namespace Vaerydian.Screens
{
    class GameScreen : Screen
    {
        private ECSInstance ecsInstance;
        
        private SpriteBatch spriteBatch;
        

        private GameContainer gameContainer = new GameContainer();

        //update systems
        private EntitySystem playerMovementSystem;
        private EntitySystem cameraFocusSystem;
        private EntitySystem mousePointerSystem;
        private EntitySystem behaviorSystem;
        private EntitySystem mapCollisionSystem;
        private EntitySystem projectileSystem;
        private EntitySystem meleeSystem;
        private EntitySystem attackSystem;
        private EntitySystem damageSystem;
        private EntitySystem healthSystem;
        private EntitySystem lifeSystem;
        private EntitySystem uiUpdateSystem;
        
        //audio
        private EntitySystem audioSystem;

        //draw systems
        private EntitySystem spriteRenderSystem;
        private EntitySystem spriteNormalSystem;
        //private EntitySystem spriteDepthSystem;
        private EntitySystem mapSystem;
        private EntitySystem mapNormalSystem;
        //private EntitySystem mapDepthSystem;
        private EntitySystem shadingSystem;
        private EntitySystem deferredSystem;
        private EntitySystem healthBarRenderSystem;
        private EntitySystem damageDisplaySystem;
        private EntitySystem quadTreeDebugRenderSystem;
        private EntitySystem uiDrawSystem;

        private EntityFactory entityFactory;
        private NPCFactory npcFactory;
        private UIFactory uiFactory;
        private MapFactory mapFactory;

        private int avg, disp, elapsed;

        private GeometryMap geometry;
        private ComponentMapper geometryMapper;

        private Texture2D debugTex;
        private Random rand = new Random();

        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(this.ScreenManager.GraphicsDevice);

            ecsInstance = new ECSInstance();

            gameContainer = ScreenManager.GameContainer;

            //gameContainer.SpriteBatch = spriteBatch;
            //gameContainer.ContentManager = this.ScreenManager.Game.Content;
            //gameContainer.GraphicsDevice = this.ScreenManager.GraphicsDevice;

            //create & register systems
            //register update systems
            playerMovementSystem = ecsInstance.SystemManager.setSystem(new PlayerInputSystem(), new Position(), new Velocity(), new Controllable());
            cameraFocusSystem = ecsInstance.SystemManager.setSystem(new CameraFocusSystem(), new CameraFocus(), new Position());
            mousePointerSystem = ecsInstance.SystemManager.setSystem(new MousePointerSystem(), new Position(), new MousePosition());
            behaviorSystem = ecsInstance.SystemManager.setSystem(new BehaviorSystem(), new AiBehavior());
            mapCollisionSystem = ecsInstance.SystemManager.setSystem(new MapCollisionSystem(), new MapCollidable());
            projectileSystem = ecsInstance.SystemManager.setSystem(new ProjectileSystem(), new Projectile());
            meleeSystem = ecsInstance.SystemManager.setSystem(new MeleeSystem(), new MeleeAction());
            attackSystem = ecsInstance.SystemManager.setSystem(new AttackSystem(), new Attack());
            damageSystem = ecsInstance.SystemManager.setSystem(new DamageSystem(), new Damage());
            healthSystem = ecsInstance.SystemManager.setSystem(new HealthSystem(), new Health());
            lifeSystem = ecsInstance.SystemManager.setSystem(new LifeSystem(), new Life());
            uiUpdateSystem = ecsInstance.SystemManager.setSystem(new UIUpdateSystem(), new UserInterface());
            
            //audio systems
            audioSystem = ecsInstance.SystemManager.setSystem(new AudioSystem(gameContainer), new Audio());

            //register render systems
            spriteRenderSystem = ecsInstance.SystemManager.setSystem(new SpriteRenderSystem(gameContainer), new Position(), new Sprite());
            spriteNormalSystem = ecsInstance.SystemManager.setSystem(new SpriteNormalSystem(gameContainer), new Position(), new Sprite());
            //spriteDepthSystem = ecsInstance.SystemManager.setSystem(new SpriteDepthSystem(gameContainer), new Position(), new Sprite());
            mapSystem = ecsInstance.SystemManager.setSystem(new MapSystem(gameContainer), new GameMap());
            mapNormalSystem = ecsInstance.SystemManager.setSystem(new MapNormalSystem(gameContainer), new GameMap());
            //mapDepthSystem = ecsInstance.SystemManager.setSystem(new MapDepthSystem(gameContainer), new GameMap());
            shadingSystem = ecsInstance.SystemManager.setSystem(new ShadingSystem(gameContainer), new Light());
            deferredSystem = ecsInstance.SystemManager.setSystem(new DeferredSystem(gameContainer), new GeometryMap());
            healthBarRenderSystem = ecsInstance.SystemManager.setSystem(new HealthBarRenderSystem(gameContainer), new Health());
            damageDisplaySystem = ecsInstance.SystemManager.setSystem(new DamageDisplaySystem(gameContainer), new Damage());
            quadTreeDebugRenderSystem = ecsInstance.SystemManager.setSystem(new QuadTreeDebugRenderSystem(gameContainer), new Position(),new AiBehavior());
            uiDrawSystem = ecsInstance.SystemManager.setSystem(new UIDrawSystem(gameContainer.ContentManager, gameContainer.GraphicsDevice), new UserInterface());

            //any additional component registration
            ecsInstance.ComponentManager.registerComponentType(new ViewPort());
            ecsInstance.ComponentManager.registerComponentType(new MousePosition());
            ecsInstance.ComponentManager.registerComponentType(new Heading());
            ecsInstance.ComponentManager.registerComponentType(new MapDebug());
            ecsInstance.ComponentManager.registerComponentType(new Transform());
            ecsInstance.ComponentManager.registerComponentType(new SpatialPartition());
            ecsInstance.ComponentManager.registerComponentType(new Interactable());
            ecsInstance.ComponentManager.registerComponentType(new BoundingPolygon());
            ecsInstance.ComponentManager.registerComponentType(new Item());
            ecsInstance.ComponentManager.registerComponentType(new Equipment());
            ecsInstance.ComponentManager.registerComponentType(new Armor());
            ecsInstance.ComponentManager.registerComponentType(new Weapon());
            ecsInstance.ComponentManager.registerComponentType(new Attributes());
            ecsInstance.ComponentManager.registerComponentType(new Skills());
            ecsInstance.ComponentManager.registerComponentType(new Knowledges());
            ecsInstance.ComponentManager.registerComponentType(new Factions());
            ecsInstance.ComponentManager.registerComponentType(new Victory());

            //initialize all systems
            ecsInstance.SystemManager.initializeSystems();

            //create the entity factory
            entityFactory = new EntityFactory(ecsInstance, gameContainer);
            npcFactory = new NPCFactory(ecsInstance);
            uiFactory = new UIFactory(ecsInstance,gameContainer);
            mapFactory = new MapFactory(ecsInstance, gameContainer);

            //setup local geometrymapper
            geometryMapper = new ComponentMapper(new GeometryMap(), ecsInstance);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            //debugTex = ScreenManager.Game.Content.Load<Texture2D>("temperature");
            debugTex = gameContainer.ContentManager.Load<Texture2D>("temperature");

            //load early entities
            //entityFactory.createBackground();
            entityFactory.createPlayer();
            entityFactory.createCamera();
            entityFactory.createMousePointer();

            //npcFactory.createFollower(new Vector2(500, 350), ecsInstance.TagManager.getEntityByTag("PLAYER"), 50);
            //entityFactory.createFollower(new Vector2(150, 250), ecsInstance.TagManager.getEntityByTag("PLAYER"), 100);
            //entityFactory.createFollower(new Vector2(250, 350), ecsInstance.TagManager.getEntityByTag("PLAYER"), 150);
            //entityFactory.createFollower(new Vector2(350, 450), ecsInstance.TagManager.getEntityByTag("PLAYER"), 200);
            //npcFactory.createWanderingEnemy(new Vector2(100, 100));
            

            //create cave
            //entityFactory.createCave();
            entityFactory.CreateTestMap();
            //GameMap map = entityFactory.createRandomMap(100, 100, 75, true, 50000, 5);
            //GameMap map = mapFactory.createRandomCaveMap(100, 100, 45, true, 50000, 4);
            //GameMap map = mapFactory.createWorldMap(0, 0, (int)(480 * 1.6), 480, 5f, (int)(480 * 1.6), 480, 42);


            //npcFactory.createWanders(100, map);

            //uiFactory.createUITests();
            

            //create map debug
            entityFactory.createMapDebug();

            //create lights

            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    //entityFactory.createRandomLight(;)
                    entityFactory.createStandaloneLight(true, 640, new Vector3(i * 640, j * 640, 100), .1f,
                        new Vector4(.5f, .5f, .7f, (float)rand.NextDouble()));
                }
            }

            //create GeometryMap
            entityFactory.createGeometryMap();

            //create spatialpartition
            entityFactory.createSpatialPartition(new Vector2(0, 0), new Vector2(3200, 3200), 3);

            //load fonts
            //fontManager.LoadContent();

            //early entity reslove
            ecsInstance.resolveEntities();

            //load system content
            ecsInstance.SystemManager.systemsLoadContent();

            //get geometry map
            geometry = (GeometryMap)geometryMapper.get(ecsInstance.TagManager.getEntityByTag("GEOMETRY"));
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            ecsInstance.cleanUp();

            GC.Collect();
        }

        public override void hasFocusUpdate(GameTime gameTime)
        {
            base.hasFocusUpdate(gameTime);

            //check to see if escape was recently pressed
            if (InputManager.isKeyToggled(Keys.Escape))
            {
                this.ScreenManager.removeScreen(this);
                LoadingScreen.Load(this.ScreenManager, false, new StartScreen());
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            /*
            //update all input
            InputManager.Update();

            if (InputManager.YesExit)
                this.Exit();
            */
            if (InputManager.isKeyToggled(Keys.PrintScreen))
                InputManager.YesScreenshot = true;


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
            projectileSystem.process();
            meleeSystem.process();
            lifeSystem.process();
            healthSystem.process();
            damageSystem.process();
            attackSystem.process();

            mapCollisionSystem.process();

            //process audio
            audioSystem.process();

            //process user interfaces;
            uiUpdateSystem.process();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            gameContainer.GraphicsDevice.Clear(Color.Black);

            gameContainer.GraphicsDevice.SetRenderTarget(null);
            gameContainer.GraphicsDevice.SetRenderTarget(geometry.ColorMap);
            gameContainer.GraphicsDevice.Clear(Color.Transparent);

            //run color draw systems
            mapSystem.process();
            spriteRenderSystem.process();

            gameContainer.GraphicsDevice.SetRenderTarget(null);
            gameContainer.GraphicsDevice.SetRenderTarget(geometry.NormalMap);
            gameContainer.GraphicsDevice.Clear(Color.Transparent);

            //run normal systems
            mapNormalSystem.process();
            spriteNormalSystem.process();

            //GraphicsDevice.SetRenderTarget(null);
            //GraphicsDevice.SetRenderTarget(geometry.DepthMap);
            //GraphicsDevice.Clear(Color.Transparent);

            //mapDepthSystem.process();
            //spriteDepthSystem.process();

            gameContainer.GraphicsDevice.SetRenderTarget(null);
            gameContainer.GraphicsDevice.SetRenderTarget(geometry.ShadingMap);
            gameContainer.GraphicsDevice.Clear(Color.Transparent);

            //run shading system
            shadingSystem.process();

            gameContainer.GraphicsDevice.SetRenderTarget(null);
            gameContainer.GraphicsDevice.Clear(Color.Black);

            //run differed system
            deferredSystem.process();


            //run UI systems
            damageDisplaySystem.process();
            healthBarRenderSystem.process();
            uiDrawSystem.process();

            //run debug systems
            //quadTreeDebugRenderSystem.process();

            spriteBatch.Begin();

            spriteBatch.DrawString(FontManager.Fonts["General"], "Entities: " + ecsInstance.EntityManager.getEntityCount(), new Vector2(0, 14), Color.Red);

            spriteBatch.End();


            if (InputManager.YesScreenshot)
            {
                saveScreenShot(gameContainer.GraphicsDevice);
                InputManager.YesScreenshot = false;
            }

            //DrawDebugRenderTargets(spriteBatch);
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


        /// <summary>
        /// [DEBUG] Draws the debug render targets onto the bottom of the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        public void DrawDebugRenderTargets(SpriteBatch spriteBatch)
        {
            // Draw some debug textures
            spriteBatch.Begin();

            Rectangle size = new Rectangle(0, 0, geometry.ColorMap.Width / 3, geometry.ColorMap.Height / 3);
            var position = new Vector2(0, gameContainer.GraphicsDevice.Viewport.Height - size.Height);
            spriteBatch.Draw(
                geometry.ColorMap,
                new Rectangle(
                    (int)position.X, (int)position.Y,
                    size.Width,
                    size.Height),
                Color.White);

            spriteBatch.Draw(
                geometry.NormalMap,
                new Rectangle(
                    (int)position.X + size.Width, (int)position.Y,
                    size.Width,
                    size.Height),
                Color.White);

            spriteBatch.Draw(
                debugTex,
                new Rectangle(
                    (int)position.X + size.Width * 2, (int)position.Y,
                    size.Width,
                    size.Height), Color.Black);

            spriteBatch.Draw(
                geometry.ShadingMap,
                new Rectangle(
                    (int)position.X + size.Width * 2, (int)position.Y,
                    size.Width,
                    size.Height),
                Color.White);

            spriteBatch.End();
        }

    }
}
