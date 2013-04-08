using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ECSFramework;

using Vaerydian;
using Vaerydian.Components;
using Vaerydian.Components.Audio;
using Vaerydian.Components.Characters;
using Vaerydian.Components.Dbg;
using Vaerydian.Components.Items;
using Vaerydian.Components.Actions;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Graphical;
using Vaerydian.Characters;

using Vaerydian.Factories;
using Vaerydian.Systems;
using Vaerydian.Systems.Draw;
using Vaerydian.Systems.Update;

using Glimpse.Input;
using Glimpse.Systems;
using Glimpse.Components;
using Glimpse.Managers;

using AgentComponentBus.Core;
using AgentComponentBus.Components.ECS;
using AgentComponentBus.Systems;
using Vaerydian.Utils;
using Vaerydian.Sessions;
using Vaerydian.Maps;
using Vaerydian.ACB;


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
        private EntitySystem victorySystem;
        private EntitySystem uiUpdateSystem;
        private EntitySystem triggerSystem;
        
        //audio
        private EntitySystem audioSystem;

        //draw systems
        private EntitySystem spriteRenderSystem;
        private EntitySystem spriteNormalSystem;
        private EntitySystem animationSystem;
        //private EntitySystem spriteDepthSystem;
        private EntitySystem mapSystem;
        private EntitySystem mapNormalSystem;
        //private EntitySystem mapDepthSystem;
        private EntitySystem shadingSystem;
        private EntitySystem deferredSystem;
        private EntitySystem healthBarRenderSystem;
        private EntitySystem floatingTextDisplaySystem;
        private EntitySystem quadTreeDebugRenderSystem;
        private EntitySystem uiDrawSystem;

        //ACB systems
        private EntitySystem busCommitSystem;
        private EntitySystem busRetrieveSystem;
        private EntitySystem busRegistrationSystem;

        private Bus bus;
        private TaskWorker taskWorker;
        private Thread busThread, twThread;

        private EntityFactory entityFactory;
        private NPCFactory npcFactory;
        private UIFactory uiFactory;
        private MapFactory mapFactory;

        private int avg, disp, elapsed;

        private GeometryMap geometry;
        private ComponentMapper geometryMapper;

        private Texture2D debugTex;
        private Random rand = new Random();

        private GameMap g_Map;
        private Entity player;

        private MapState mapState;

        public const int GAMESCREEN_PARAM_SIZE = 4;
        public const int GAMESCREEN_SEED = 0;
        public const int GAMESCREEN_SKILLLEVEL = 1;
        public const int GAMESCREEN_RETURNING = 2;
        public const int GAMESCREEN_LAST_PLAYER_POSITION = 3;

        public static bool PLAYER_IS_DEAD = false;

		private JsonManager g_JsonManager = new JsonManager();

        /// <summary>
        /// current loading message
        /// </summary>
        public override string LoadingMessage
        {
            get
            {
                return MapMaker.StatusMessage;
            }
        }

        private bool g_FirstLoad = false;

        private object[] g_Parameters;

        private short g_MapType;

		private long prevCycles, currentCycles, elapsedCycles;

        public GameScreen() { }

        public GameScreen(bool firstLoad, short mapType, object[] parameters)
        {
            g_FirstLoad = firstLoad;
            g_MapType = mapType;
            g_Parameters = parameters;
        }

        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(this.ScreenManager.GraphicsDevice);

            ecsInstance = new ECSInstance();

            ComponentMapper.ECSInstance = ecsInstance;

            gameContainer = ScreenManager.GameContainer;

            //instantiate the bus
            bus = new Bus();
            bus.initialize();
            bus.UpdateIntervalTime = 32;
            bus.MaxUpdatesPerCycle = 10;
            

            taskWorker = new TaskWorker();
            taskWorker.initialize();
            taskWorker.MaxTasksPerCycle = 10;
            taskWorker.MaxCommitsPerCycle = 10;
            taskWorker.MaxRetrievesPerCycle = 10;

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
            victorySystem = ecsInstance.SystemManager.setSystem(new AwardSystem(), new Award());
            uiUpdateSystem = ecsInstance.SystemManager.setSystem(new UIUpdateSystem(), new UserInterface());
            triggerSystem = ecsInstance.SystemManager.setSystem(new TriggerSystem(), new Trigger());
            
            //audio systems
            audioSystem = ecsInstance.SystemManager.setSystem(new AudioSystem(gameContainer), new Audio());

            //register render systems
            spriteRenderSystem = ecsInstance.SystemManager.setSystem(new SpriteRenderSystem(gameContainer), new Position(), new Sprite());
            //spriteNormalSystem = ecsInstance.SystemManager.setSystem(new SpriteNormalSystem(gameContainer), new Position(), new Sprite());
            //spriteDepthSystem = ecsInstance.SystemManager.setSystem(new SpriteDepthSystem(gameContainer), new Position(), new Sprite());
            animationSystem = ecsInstance.SystemManager.setSystem(new AnimationSystem(gameContainer), new Character(), new Position());
            mapSystem = ecsInstance.SystemManager.setSystem(new MapSystem(gameContainer), new GameMap());
            //mapNormalSystem = ecsInstance.SystemManager.setSystem(new MapNormalSystem(gameContainer), new GameMap());
            //mapDepthSystem = ecsInstance.SystemManager.setSystem(new MapDepthSystem(gameContainer), new GameMap());
            //shadingSystem = ecsInstance.SystemManager.setSystem(new ShadingSystem(gameContainer), new Light());
            //deferredSystem = ecsInstance.SystemManager.setSystem(new DeferredSystem(gameContainer), new GeometryMap());
            healthBarRenderSystem = ecsInstance.SystemManager.setSystem(new HealthBarRenderSystem(gameContainer), new Health());
            floatingTextDisplaySystem = ecsInstance.SystemManager.setSystem(new FloatingTextDisplaySystem(gameContainer), new FloatingText());
            quadTreeDebugRenderSystem = ecsInstance.SystemManager.setSystem(new QuadTreeDebugRenderSystem(gameContainer), new Position(),new AiBehavior());
            uiDrawSystem = ecsInstance.SystemManager.setSystem(new UIDrawSystem(gameContainer.ContentManager, gameContainer.GraphicsDevice), new UserInterface());


            //bus setup
            busRegistrationSystem = ecsInstance.SystemManager.setSystem(new BusRegistrationSystem(bus), new BusAgent());
            busCommitSystem = ecsInstance.SystemManager.setSystem(new BusCommitSystem(taskWorker), new BusDataCommit());
            busRetrieveSystem = ecsInstance.SystemManager.setSystem(new BusRetrieveSystem(taskWorker), new BusDataRetrieval());

            //any additional component registration
            ecsInstance.ComponentManager.registerComponentType(new Vaerydian.Components.Graphical.ViewPort());
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
            ecsInstance.ComponentManager.registerComponentType(new Award());
            ecsInstance.ComponentManager.registerComponentType(new Information());
            ecsInstance.ComponentManager.registerComponentType(new Aggrivation());
			ecsInstance.ComponentManager.registerComponentType(new Audio());
            ecsInstance.ComponentManager.registerComponentType(new Vaerydian.Components.Utils.Path());
            ecsInstance.ComponentManager.registerComponentType(new StateContainer<EnemyState, EnemyState>());

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

        public override void LoadContent ()
		{
			base.LoadContent ();

			//debugTex = ScreenManager.Game.Content.Load<Texture2D>("temperature");
			//debugTex = gameContainer.ContentManager.Load<Texture2D>("temperature");

			Console.Out.WriteLine ("LOADING LEVEL...");

			string json = g_JsonManager.loadJSON("./Content/json/map_params.v");
			JsonObject jo = g_JsonManager.jsonToJsonObject (json);


			switch (g_MapType) {
			case MapType_Old.WORLD:
				if (g_FirstLoad) {
					g_Map = mapFactory.createWorldMap(jo["WORLD","x"].asInt(),
					                                  jo["WORLD","y"].asInt(),
					                                  jo["WORLD","dx"].asInt(),
					                                  jo["WORLD","dy"].asInt(),
					                                  jo["WORLD","z"].asFloat(),
					                                  jo["WORLD","xsize"].asInt(),
					                                  jo["WORLD","ysize"].asInt(),
					                                  (int) g_Parameters[GAMESCREEN_SEED]);
					GameSession.WorldMap = g_Map;
				} else {
					g_Map = mapFactory.recreateWorldMap (GameSession.WorldMap);
					GameSession.WorldMap = g_Map;
				}
				break;
			case MapType_Old.CAVE:
				g_Map = mapFactory.createRandomCaveMap(jo["CAVE","x"].asInt (),
				                                       jo["CAVE","y"].asInt (),
				                                       jo["CAVE","prob"].asInt (),
				                                       jo["CAVE","cell_op_spec"].asBool (),
				                                       jo["CAVE","iter"].asInt (),
				                                       jo["CAVE","neighbors"].asInt (),
				                                       (int)g_Parameters[GAMESCREEN_SEED]);
				break;
			case MapType_Old.WILDERNESS:
				g_Map = mapFactory.createRandomForestMap(100,
				                                         100,
				                                         75,
				                                         TerrainType_Old.FOREST_FLOOR,
				                                         TerrainType_Old.FOREST_TREE,
				                                         (int) g_Parameters[GAMESCREEN_SEED]);
				break;
			case MapType_Old.DUNGEON:
				g_Map = mapFactory.createRandomDungeonMap(100,100,200, (int) g_Parameters[GAMESCREEN_SEED]);
				break;
			default:
				g_Map = mapFactory.createWorldMap(jo["WORLD","x"].asInt(),
				                                  jo["WORLD","y"].asInt(),
				                                  jo["WORLD","dx"].asInt(),
				                                  jo["WORLD","dy"].asInt(),
				                                  jo["WORLD","z"].asFloat(),
				                                  jo["WORLD","xsize"].asInt(),
				                                  jo["WORLD","ysize"].asInt(),
				                                  (int) g_Parameters[GAMESCREEN_SEED]);
				break;
			}

			if (g_FirstLoad)
				player = entityFactory.createPlayer ((int)g_Parameters [GAMESCREEN_SKILLLEVEL]);
			else {
				if ((bool)g_Parameters [GAMESCREEN_RETURNING])
					player = entityFactory.recreatePlayer (GameSession.PlayerState, (Position)g_Parameters [GAMESCREEN_LAST_PLAYER_POSITION]);
				else {
					player = entityFactory.recreatePlayer (GameSession.PlayerState, new Position (mapFactory.findSafeLocation (g_Map), new Vector2 (16, 16)));
				}
			}


            mapState = new MapState();
            mapState.MapType = g_Map.Map.MapType;
            mapState.Seed = g_Map.Map.Seed;
            mapState.SkillLevel = (int)g_Parameters[GAMESCREEN_SKILLLEVEL];
            

            entityFactory.createCamera();
            entityFactory.createMousePointer();

            uiFactory.createHitPointLabel(player, 100, 50, new Point((this.ScreenManager.GraphicsDevice.Viewport.Width - 100) / 2, 0));

            if(!g_FirstLoad && mapState.MapType != MapType_Old.WORLD)
                npcFactory.createWandererTrigger(20, g_Map,(int)g_Parameters[GAMESCREEN_SKILLLEVEL]);

            //create map debug
            entityFactory.createMapDebug();
            
            //create lights
            /*
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    //entityFactory.createRandomLight(;)
                    entityFactory.createStandaloneLight(true, 640, new Vector3(i * 640, j * 640, 100), .1f,
                        new Vector4(.5f, .5f, .7f, (float)rand.NextDouble()));
                }
            }*/

            //create GeometryMap
            //entityFactory.createGeometryMap();

            //create spatialpartition
            entityFactory.createSpatialPartition(new Vector2(0, 0), new Vector2(3200, 3200), 4);


            //early entity reslove
            ecsInstance.resolveEntities();

            //load system content
            ecsInstance.SystemManager.systemsLoadContent();

            //get geometry map
            //geometry = (GeometryMap)geometryMapper.get(ecsInstance.TagManager.getEntityByTag("GEOMETRY"));

            //setup bus components
            bus.addComponent(new ComponentRegisterArgs(new PathFinder(), "PATH_FINDER"));
            bus.addComponent(new ComponentRegisterArgs(new BatHSM(ecsInstance), "BAT_HSBM"));

            //launch bus
            busThread = new Thread(bus.run);
            busThread.Start();

            twThread = new Thread(taskWorker.run);
            twThread.Start();
                
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            Console.Out.WriteLine("LEAVING LEVEL...");

            ecsInstance.cleanUp();

            
            bus.shutdown();
            taskWorker.shutdown();

            busThread.Join();
            twThread.Join();

            ResourcePool.cleanup();

            GC.Collect();
        }

        public override void hasFocusUpdate (GameTime gameTime)
		{
			base.hasFocusUpdate (gameTime);

			//check to see if escape was recently pressed
			if (InputManager.isKeyToggled (Keys.Escape)) {
				this.ScreenManager.removeScreen (this);
				NewLoadingScreen.Load (this.ScreenManager, false, new StartScreen ());
			}

			if (InputManager.isKeyToggled (Keys.Enter)) {
				Console.Out.WriteLine ("CHANGING LEVEL...");

				//set skill level
				Skills skills = new Skills ();
				int skilllevel = ((Skills)ecsInstance.ComponentManager.getComponent (player, skills.getTypeId ())).SkillSet [Utils.SkillName.Ranged].Value;
                
				//set seed
				Position pos = new Position ();
				pos = (Position)ecsInstance.ComponentManager.getComponent (player, pos.getTypeId ());
				int x = (int)(pos.Pos.X + pos.Offset.X) / 32;
				int y = (int)(pos.Pos.Y + pos.Offset.Y) / 32;

				/* WILL HELP DETECT ENTRANCES LATER
                if (map.Map.Terrain[x,y].TerrainType != TerrainType.CAVE_ENTRANCE)
                    return;
                 */

				mapState.LastPlayerPosition = pos;

				GameSession.MapStack.Push (mapState);

				//setup the parameters for the new zone
				object[] parameters = new object[GameScreen.GAMESCREEN_PARAM_SIZE];
				parameters [GameScreen.GAMESCREEN_SEED] = x * y + x + y + (int)g_Parameters [GAMESCREEN_SEED];
				parameters [GameScreen.GAMESCREEN_SKILLLEVEL] = mapState.SkillLevel + 5;//skilllevel + 5;
				parameters [GameScreen.GAMESCREEN_RETURNING] = false;
				parameters [GameScreen.GAMESCREEN_LAST_PLAYER_POSITION] = null;

				this.ScreenManager.removeScreen (this);
				NewLoadingScreen.Load (this.ScreenManager, false, new GameScreen (false, MapType_Old.CAVE, parameters));

			}

			//return to previous map
			if (InputManager.isKeyToggled (Keys.F12)) {
				if (GameSession.MapStack.Count == 0)
					return;

				MapState state = GameSession.MapStack.Pop ();

				//setup the parameters for the previous zone
				object[] parameters = new object[GameScreen.GAMESCREEN_PARAM_SIZE];
				parameters [GameScreen.GAMESCREEN_SEED] = state.Seed;
				parameters [GameScreen.GAMESCREEN_SKILLLEVEL] = state.SkillLevel;
				parameters [GameScreen.GAMESCREEN_RETURNING] = true;
				parameters [GameScreen.GAMESCREEN_LAST_PLAYER_POSITION] = state.LastPlayerPosition;

				this.ScreenManager.removeScreen (this);
				NewLoadingScreen.Load (this.ScreenManager, true, new GameScreen (false, state.MapType, parameters));
			}

			if (InputManager.isKeyToggled (Keys.F6)) {
				string json = g_JsonManager.objToJsonString(g_Map);
				g_JsonManager.saveJSON("./map_"+g_Parameters[GAMESCREEN_SEED] +".v",json);

				json = g_JsonManager.objToJsonString(GameSession.PlayerState);
				g_JsonManager.saveJSON("./player.v",json);
			}

            if (PLAYER_IS_DEAD)
            {
                GameScreen.PLAYER_IS_DEAD = false;
                this.ScreenManager.removeScreen(this);
                NewLoadingScreen.Load(this.ScreenManager, false, new StartScreen());
            }

        }



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


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
            victorySystem.process();
            triggerSystem.process();

            mapCollisionSystem.process();

            //process audio
            audioSystem.process();

            //process user interfaces;
            uiUpdateSystem.process();

            //run bus systems
            busRegistrationSystem.process();
            busCommitSystem.process();
            busRetrieveSystem.process();

			prevCycles = currentCycles;
			currentCycles = taskWorker.Cycles;
			elapsedCycles = currentCycles - prevCycles;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            gameContainer.GraphicsDevice.Clear(Color.Black);

            //gameContainer.GraphicsDevice.SetRenderTarget(null);
            //gameContainer.GraphicsDevice.SetRenderTarget(geometry.ColorMap);
            //gameContainer.GraphicsDevice.Clear(Color.Transparent);

            //run color draw systems
            mapSystem.process();
            spriteRenderSystem.process();
            animationSystem.process();

			/* 
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
			*/

            //run UI systems
            floatingTextDisplaySystem.process();
            healthBarRenderSystem.process();
            uiDrawSystem.process();

            //run debug systems
            //quadTreeDebugRenderSystem.process();

            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone);

            spriteBatch.DrawString(FontManager.Fonts["General"], "Entities: " + ecsInstance.EntityManager.getEntityCount(), new Vector2(0, 14), Color.Red);
			spriteBatch.DrawString(FontManager.Fonts["General"], "TCycles/Frame: " + elapsedCycles, new Vector2(0, 28), Color.Red);

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
            /*			
            //setup a color buffer to get the back Buffer's data
            Color[] colors = new Color[graphicsDevice.PresentationParameters.BackBufferHeight * graphicsDevice.PresentationParameters.BackBufferWidth];

            //place the back bugger data into the color buffer
            graphicsDevice.GetBackBufferData<Color>(colors);

            string timestamp = "" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;

            //setup the filestream for the screenshot
            FileStream fs = new FileStream("screenshot_" + timestamp + ".png", FileMode.Create);

            //setup the texture that will be saved
            Texture2D picTex = new Texture2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            //set the texture's color data to that of the color buffer
            picTex.SetData<Color>(colors);

            //save the texture to a png image file
            picTex.SaveAsPng(fs, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            //close the file stream
            fs.Close();

            GC.Collect();
             * */
		}

        

    }
}
