using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ECSFramework;

using Vaerydian.Characters.Experience;
using Vaerydian.Characters.Skills;
using Vaerydian.Characters.Stats;
using Vaerydian.Components;
using Vaerydian.Components.Debug;
using Vaerydian.Behaviors;
using Vaerydian.Utils;
using Vaerydian.Factories;

using WorldGeneration.Cave;
using WorldGeneration.World;
using WorldGeneration.Utils;
using Vaerydian.Characters.Factions;
using Vaerydian.Components.Characters;


namespace Vaerydian.Factories
{
    class EntityFactory
    {
        private ECSInstance e_EcsInstance;
        private static GameContainer e_Container;
        private Random rand = new Random();

        public EntityFactory(ECSInstance ecsInstance, GameContainer container)
        {
            e_EcsInstance = ecsInstance;
            e_Container = container;
        }

        public EntityFactory(ECSInstance ecsInstance) 
        {
            e_EcsInstance = ecsInstance;
        }

        public void createPlayer()
        {
            Entity e = e_EcsInstance.create();
            e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(576f, 360f),new Vector2(12.5f)));
            //e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(0, 0), new Vector2(12.5f)));
            e_EcsInstance.EntityManager.addComponent(e, new Velocity(4f));
            e_EcsInstance.EntityManager.addComponent(e, new Controllable());
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("characters\\lord_lard_sheet", "characters\\normals\\lord_lard_sheet_normals",32,32,0,0));
            e_EcsInstance.EntityManager.addComponent(e, new CameraFocus(75));
            e_EcsInstance.EntityManager.addComponent(e, new MapCollidable());
            e_EcsInstance.EntityManager.addComponent(e, new Heading());
            e_EcsInstance.EntityManager.addComponent(e, createLight(true, 100, new Vector3(new Vector2(576f, 360f), 10), 0.5f, new Vector4(1, 1, .6f, 1)));
            e_EcsInstance.EntityManager.addComponent(e, new Transform());

            //create health
            Health health = new Health(10000);
            health.RecoveryAmmount = 2000;
            health.RecoveryRate = 500;
            e_EcsInstance.EntityManager.addComponent(e, health);

            //create life
            Life life = new Life();
            life.IsAlive = true;
            life.DeathLongevity = 5000;
            e_EcsInstance.EntityManager.addComponent(e, life);

            //create interactions
            Interactable interact = new Interactable();
            interact.SupportedInteractions.PROJECTILE_COLLIDABLE = true;
            interact.SupportedInteractions.ATTACKABLE = true;
            e_EcsInstance.EntityManager.addComponent(e, interact);

            //create test equipment
            ItemFactory iFactory = new ItemFactory(e_EcsInstance);
            e_EcsInstance.EntityManager.addComponent(e, iFactory.createTestEquipment());

            //setup experiences
            Experiences experiences = new Experiences();
            Experience xp = new Experience(50);
            experiences.GeneralExperience.Add(MobGroup.Test, xp);
            e_EcsInstance.EntityManager.addComponent(e, experiences);

            //setup attributes
            Attributes attributes = new Attributes();
            attributes.Endurance.Value = 50;
            attributes.Mind.Value = 50;
            attributes.Muscle.Value = 50;
            attributes.Perception.Value = 50;
            attributes.Quickness.Value = 50;
            e_EcsInstance.EntityManager.addComponent(e, attributes);

            //setup skills
            Skills skills = new Skills();
            Skill skill = new Skill("Ranged", 50, SkillType.Offensive);
            skills.SkillSet.Add(SkillNames.Ranged, skill);
            skill = new Skill("Avoidance", 50, SkillType.Defensive);
            skills.SkillSet.Add(SkillNames.Avoidance, skill);
            skill = new Skill("Melee", 50, SkillType.Offensive);
            skills.SkillSet.Add(SkillNames.Melee, skill);
            e_EcsInstance.EntityManager.addComponent(e, skills);

            Faction faction = new Faction(100, FactionType.Player);
            Faction enemy = new Faction(-10, FactionType.TestMob);
            Faction ally = new Faction(100, FactionType.Ally);
            
            Factions factions = new Factions();
            factions.OwnerFaction = faction;
            factions.KnownFactions.Add(enemy.FactionType, enemy);
            factions.KnownFactions.Add(ally.FactionType, ally);
            e_EcsInstance.EntityManager.addComponent(e, factions);


            e_EcsInstance.TagManager.tagEntity("PLAYER", e);

            e_EcsInstance.refresh(e);

        }

        public void createCamera()
        {
            Entity e = e_EcsInstance.create();
            e_EcsInstance.EntityManager.addComponent(e, new Velocity(5f));
            //e_EcsInstance.EntityManager.addComponent(e, new ViewPort(new Vector2(576, 360f), new Vector2(1152, 720)));
            e_EcsInstance.EntityManager.addComponent(e, new ViewPort(new Vector2(0, 0), new Vector2(e_Container.GraphicsDevice.Viewport.Width, e_Container.GraphicsDevice.Viewport.Height)));

            e_EcsInstance.TagManager.tagEntity("CAMERA", e);

            e_EcsInstance.refresh(e);
           
        }

        public void createMousePointer()
        {
            Entity e = e_EcsInstance.create();
            e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(0), new Vector2(24)));
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("reticle","reticle_normal",48,48,0,0));
            e_EcsInstance.EntityManager.addComponent(e, new MousePosition());
            e_EcsInstance.EntityManager.addComponent(e, createLight(true, 75, new Vector3(576, 360, 50), 0.3f, new Vector4(1f, 1f, 0.6f, 1f)));
            e_EcsInstance.EntityManager.addComponent(e, new Transform());

            e_EcsInstance.TagManager.tagEntity("MOUSE", e);

            e_EcsInstance.refresh(e);
        }

        

        



        public void createCave()
        {
            Entity e = e_EcsInstance.create();
            CaveEngine ce = new CaveEngine();

            Random rand = new Random();

            ce.generateCave(100, 100, rand.Next(100), 100, 0.5);
            e_EcsInstance.EntityManager.addComponent(e, new GameMap(ce.getMap()));

            e_EcsInstance.TagManager.tagEntity("MAP", e);

            e_EcsInstance.refresh(e);
        }


        /// <summary>
        /// creates a random map with the following parameters
        /// </summary>
        /// <param name="x">width</param>
        /// <param name="y">height</param>
        /// <param name="prob">close cell probability (0-100)</param>
        /// <param name="h">cell operation specifier [h=true, if c>n close else open; h=false if c>n open else close]</param>
        /// <param name="counter">number of iterations</param>
        /// <param name="n">number of cells neighbors</param>
        /// <param name="c">number of cells closed neighbors</param>
        public GameMap createRandomMap(int x, int y, int prob, bool h, int counter, int n)
        {
            Map map = new Map(x, y);
            Random rand = new Random();

            //fill map
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Terrain terrain = new Terrain();
                    terrain.TerrainType = TerrainType.CAVE_WALL;
                    terrain.IsBlocking = true;
                    map.Terrain[i, j] = terrain;
                }
            }

            for (int i = 1; i < x-1; i++)
            {
                for (int j = 1; j < y-1; j++)
                {
                    Terrain terrain = new Terrain();
                    terrain.TerrainType = TerrainType.CAVE_FLOOR;
                    terrain.IsBlocking = false;
                    map.Terrain[i, j] = terrain;
                }
            }

            
            //randomize map
            for (int i = 1; i < x-1; i++)
            {
                for (int j = 1; j < y-1; j++)
                {
                    Terrain terrain = map.Terrain[i, j];

                    //randomly set it
                    if (terrain.TerrainType == TerrainType.CAVE_FLOOR &&
                        rand.Next(100) <= prob)
                    {
                        terrain.TerrainType = TerrainType.CAVE_WALL;
                        terrain.IsBlocking = true;
                        map.Terrain[i, j] = terrain;
                    }
                }
            }


            int rX, rY;

            for (int i = 0; i < counter; i++)
            {
                rX = rand.Next(1,x-1);
                rY = rand.Next(1,y-1);


                Terrain terrain = map.Terrain[rX, rY];

                /*//randomly set it
                if (terrain.TerrainType == TerrainType.CAVE_FLOOR &&
                    rand.Next(100) <= prob)
                {
                    terrain.TerrainType = TerrainType.CAVE_WALL;
                    terrain.IsBlocking = true;
                    map.Terrain[rX, rY] = terrain;
                }*/

                if (h)
                {
                    if (closedNeighbors(rX, rY, map) > n)
                    {
                        terrain.TerrainType = TerrainType.CAVE_WALL;
                        terrain.IsBlocking = true;
                        map.Terrain[rX, rY] = terrain;
                    }
                    else
                    {
                        terrain.TerrainType = TerrainType.CAVE_FLOOR;
                        terrain.IsBlocking = false;
                        map.Terrain[rX, rY] = terrain;
                    }



                }
                else
                {
                    if (closedNeighbors(rX, rY, map) > n)
                    {
                        terrain.TerrainType = TerrainType.CAVE_FLOOR;
                        terrain.IsBlocking = false;
                        map.Terrain[rX, rY] = terrain;
                    }
                    else
                    {
                        terrain.TerrainType = TerrainType.CAVE_WALL;
                        terrain.IsBlocking = true;
                        map.Terrain[rX, rY] = terrain;
                    }
                }


            }

            GameMap gameMap = new GameMap(map);

            Entity e = e_EcsInstance.create();
            e_EcsInstance.EntityManager.addComponent(e, gameMap);

            e_EcsInstance.TagManager.tagEntity("MAP", e);

            e_EcsInstance.refresh(e);


            return gameMap;
        }

        private int closedNeighbors(int x, int y, Map map)
        {
            int neighbors = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    Terrain terrain = map.Terrain[x + i, y + j];

                    if (terrain.TerrainType == TerrainType.CAVE_WALL)
                        neighbors++;
                }
            }


            return neighbors;
        }

        public void CreateTestMap()
        {
            Map map = new Map(100,100);

            Random rand = new Random();

            //byte[,] byteMap = new byte[100, 100];

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    Terrain terrain = new Terrain();
                    if (rand.Next(100) > 90)
                    {
                        terrain.TerrainType = TerrainType.CAVE_WALL;
                        terrain.IsBlocking = true;
                        //byteMap[i, j] = PathFinderHelper.BLOCKED_TILE;
                    }
                    else
                    {
                        terrain.TerrainType = TerrainType.CAVE_FLOOR;
                        //byteMap[i, j] = PathFinderHelper.EMPTY_TILE;
                    }
                    map.Terrain[i, j] = terrain;
                }
            }

            GameMap gameMap = new GameMap(map);

            //gameMap.ByteMap = byteMap;

            Entity e = e_EcsInstance.create();
            e_EcsInstance.EntityManager.addComponent(e, gameMap);

            e_EcsInstance.TagManager.tagEntity("MAP", e);

            e_EcsInstance.refresh(e);
        }

        public void createMapDebug()
        {
            Entity e = e_EcsInstance.create();

            e_EcsInstance.EntityManager.addComponent(e, new MapDebug());

            e_EcsInstance.TagManager.tagEntity("MAP_DEBUG", e);

            e_EcsInstance.refresh(e);

        }


        public Entity createGeometryMap()
        {
            Entity e = e_EcsInstance.create();

            GeometryMap geometry = new GeometryMap();

            PresentationParameters pp = e_Container.SpriteBatch.GraphicsDevice.PresentationParameters;
            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;
            SurfaceFormat format = pp.BackBufferFormat;

            geometry.ColorMap = new RenderTarget2D(e_Container.SpriteBatch.GraphicsDevice, width, height);
            geometry.NormalMap = new RenderTarget2D(e_Container.SpriteBatch.GraphicsDevice, width, height);
            geometry.DepthMap = new RenderTarget2D(e_Container.SpriteBatch.GraphicsDevice, width, height);
            geometry.ShadingMap = new RenderTarget2D(e_Container.SpriteBatch.GraphicsDevice, width, height, false, format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

            geometry.AmbientColor = new Vector4(.1f, .1f, .1f, .1f);

            e_EcsInstance.EntityManager.addComponent(e, geometry);

            e_EcsInstance.TagManager.tagEntity("GEOMETRY", e);

            e_EcsInstance.refresh(e);

            return e;
        }

        public Light createLight(bool enabled, int radius, Vector3 position, float power, Vector4 color)
        {
            Light light = new Light();

            light.IsEnabled = enabled;
            light.LightRadius = radius;
            light.Position = position;
            light.ActualPower = power;
            light.Color = color;

            return light;
        }

        public void createStandaloneLight(bool enabled, int radius, Vector3 position, float power, Vector4 color)
        {
            Entity e = e_EcsInstance.create();

            e_EcsInstance.EntityManager.addComponent(e, createLight(enabled, radius, position, power, color));
            e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(position.X, position.Y),Vector2.Zero));

            e_EcsInstance.refresh(e);
        }

        public void createRandomLight()
        {
            Entity e = e_EcsInstance.create();

            

            Vector3 pos = new Vector3(rand.Next(100*25), rand.Next(100*25), 50);

            //1152, 720
            e_EcsInstance.EntityManager.addComponent(e, createLight(true, 
                                                        rand.Next(100)+100,
                                                        pos, 
                                                        (float)rand.NextDouble()*.5f+0.5f,
                                                        new Vector4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble())));
            e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(pos.X, pos.Y), Vector2.Zero));

            e_EcsInstance.refresh(e);

        }

        public void createCollidingProjectile(Vector2 start, Vector2 heading, float velocity, int duration, Light light, Transform transform, Entity originator)
        {
            Entity e = e_EcsInstance.create();

            e_EcsInstance.EntityManager.addComponent(e, new Position(start,new Vector2(16)));
            e_EcsInstance.EntityManager.addComponent(e, new Heading(heading));
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("projectile", "projectile", 32, 32, 0, 0));
            e_EcsInstance.EntityManager.addComponent(e, new Velocity(velocity));
            
            Projectile projectile = new Projectile(duration);
            projectile.Originator = originator;
            e_EcsInstance.EntityManager.addComponent(e, projectile);
            
            e_EcsInstance.EntityManager.addComponent(e, new MapCollidable());
            e_EcsInstance.EntityManager.addComponent(e, transform);

            if (light != null)
                e_EcsInstance.EntityManager.addComponent(e, light);

            e_EcsInstance.refresh(e);

        }

        public void createProjectile(Vector2 start, Vector2 heading, float velocity, int duration, Light light, Transform transform, Entity originator)
        {
            Entity e = e_EcsInstance.create();

            e_EcsInstance.EntityManager.addComponent(e, new Position(start, new Vector2(16)));
            e_EcsInstance.EntityManager.addComponent(e, new Heading(heading));
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("projectile", "projectile", 32, 32, 0, 0));
            e_EcsInstance.EntityManager.addComponent(e, new Velocity(velocity));

            Projectile projectile = new Projectile(duration);
            projectile.Originator = originator;
            e_EcsInstance.EntityManager.addComponent(e, projectile);
            
            e_EcsInstance.EntityManager.addComponent(e, transform);
            //e_EcsInstance.EntityManager.addComponent(e, new MapCollidable());

            if (light != null)
                e_EcsInstance.EntityManager.addComponent(e, light);

            e_EcsInstance.refresh(e);

        }

        public void createSpatialPartition(Vector2 ul, Vector2 lr, int tiers)
        {
            Entity e = e_EcsInstance.create();

            SpatialPartition spatial = new SpatialPartition();

            spatial.QuadTree = new QuadTree<Entity>(ul, lr);
            spatial.QuadTree.buildQuadTree(tiers);

            e_EcsInstance.EntityManager.addComponent(e, spatial);

            e_EcsInstance.TagManager.tagEntity("SPATIAL", e);

            e_EcsInstance.refresh(e);
        }




    }
}
