using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

using Vaerydian.Components;
using Vaerydian.Components.Debug;
using Vaerydian.Behaviors;


using WorldGeneration.Cave;
using WorldGeneration.World;
using WorldGeneration.Utils;

using DeenGames.Utils.AStarPathFinder;


namespace Vaerydian.Factories
{
    class EntityFactory
    {
        private ECSInstance e_EcsInstance;

        public EntityFactory(ECSInstance ecsInstance)
        {
            e_EcsInstance = ecsInstance;
        }

        public void createPlayer()
        {
            Entity e = e_EcsInstance.create();
            e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(576f, 360f),new Vector2(12.5f)));
            e_EcsInstance.EntityManager.addComponent(e, new Velocity(5f));
            e_EcsInstance.EntityManager.addComponent(e, new Controllable());
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("characters\\player"));
            e_EcsInstance.EntityManager.addComponent(e, new CameraFocus(150));
            e_EcsInstance.EntityManager.addComponent(e, new MapCollidable());
            e_EcsInstance.EntityManager.addComponent(e, new Heading());

            e_EcsInstance.TagManager.tagEntity("PLAYER", e);

            e_EcsInstance.refresh(e);

        }

        public void createCamera()
        {
            Entity e = e_EcsInstance.create();
            e_EcsInstance.EntityManager.addComponent(e, new Velocity(5f));
            e_EcsInstance.EntityManager.addComponent(e, new ViewPort(new Vector2(576, 360f), new Vector2(1152, 720)));

            e_EcsInstance.TagManager.tagEntity("CAMERA", e);

            e_EcsInstance.refresh(e);
           
        }

        public void createBackground()
        {
            Entity e = e_EcsInstance.create();
            e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(0,0),new Vector2(0)));
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("Title"));

            e_EcsInstance.refresh(e);

        }

        public void createMousePointer()
        {
            Entity e = e_EcsInstance.create();
            e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(0), new Vector2(24)));
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("reticle"));
            e_EcsInstance.EntityManager.addComponent(e, new MousePosition());

            e_EcsInstance.TagManager.tagEntity("MOUSE", e);

            e_EcsInstance.refresh(e);
        }

        public void createFollower(Vector2 position, Entity target, float distance)
        {
            Entity e = e_EcsInstance.create();

            e_EcsInstance.EntityManager.addComponent(e, new Position(position, new Vector2(12.5f)));
            e_EcsInstance.EntityManager.addComponent(e, new Velocity(5f));
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("characters\\enemy"));
            e_EcsInstance.EntityManager.addComponent(e, new AiBehavior(new SimpleFollowBehavior(e, target, distance, e_EcsInstance)));
            e_EcsInstance.EntityManager.addComponent(e, new MapCollidable());
            e_EcsInstance.EntityManager.addComponent(e, new Heading());

            e_EcsInstance.refresh(e);

        }

        public void createCave()
        {
            Entity e = e_EcsInstance.create();
            CaveEngine ce = new CaveEngine();

            Random rand = new Random(41);

            ce.generateCave(100, 100, rand.Next(100), 4, 0.5);
            e_EcsInstance.EntityManager.addComponent(e, new GameMap(ce.getMap()));

            e_EcsInstance.TagManager.tagEntity("MAP", e);

            e_EcsInstance.refresh(e);
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
    }
}
