using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Components;

using WorldGeneration.Cave;
using WorldGeneration.Utils;
using WorldGeneration.World;
using WorldGeneration.Generators;
using Vaerydian.Components.Utils;
using Microsoft.Xna.Framework;


namespace Vaerydian.Factories
{
    class MapFactory
    {

        private ECSInstance m_EcsInstance;
        private static GameContainer m_Container;
        private Random rand = new Random();

        public MapFactory(ECSInstance ecsInstance, GameContainer container)
        {
            m_EcsInstance = ecsInstance;
            m_Container = container;
        }

        public MapFactory(ECSInstance ecsInstance) 
        {
            m_EcsInstance = ecsInstance;
        }

        public void createWorld()
        {
            WorldGenerator wg = new WorldGenerator();
            //wg.generateNewWorld(0, 0, 512, 512, 4, 100, rand.Next(100));

            Entity e = m_EcsInstance.create();

            m_EcsInstance.EntityManager.addComponent(e, new GameMap(wg.getMap()));

            m_EcsInstance.TagManager.tagEntity("MAP", e);

            m_EcsInstance.refresh(e);

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
        public GameMap createRandomCaveMap(int x, int y, int prob, bool h, int counter, int n, int seed)
        {
            Map map = MapMaker.create(x, y);

            object[] parameters = new object[CaveGen.CAVE_PARAMS_SIZE];

            parameters[CaveGen.CAVE_PARAMS_X] = x;
            parameters[CaveGen.CAVE_PARAMS_Y] = y;
            parameters[CaveGen.CAVE_PARAMS_PROB] = prob;
            parameters[CaveGen.CAVE_PARAMS_CELL_OP_SPEC] = h;
            parameters[CaveGen.CAVE_PARAMS_ITER] = counter;
            parameters[CaveGen.CAVE_PARAMS_NEIGHBORS] = n;
            parameters[CaveGen.CAVE_PARAMS_SEED] = seed;

            MapMaker.Parameters = parameters;
            
            MapMaker.generate(map, MapType.CAVE);

            GameMap gameMap = new GameMap(map);

            Entity e = m_EcsInstance.create();
            m_EcsInstance.EntityManager.addComponent(e, gameMap);

            m_EcsInstance.TagManager.tagEntity("MAP", e);

            m_EcsInstance.refresh(e);

            return gameMap;
        }

        public GameMap createWorldMap(int x, int y, int dx, int dy, float z, int xSize, int ySize, int seed )
        {
            Map map = MapMaker.create(xSize, ySize);

            object[] parameters = new object[WorldGen.WORLD_PARAMS_SIZE];

            parameters[WorldGen.WORLD_PARAMS_X] = x;
            parameters[WorldGen.WORLD_PARAMS_Y] = y;
            parameters[WorldGen.WORLD_PARAMS_DX] = dx;
            parameters[WorldGen.WORLD_PARAMS_DY] = dy;
            parameters[WorldGen.WORLD_PARAMS_Z] = z;
            parameters[WorldGen.WORLD_PARAMS_XSIZE] = xSize;
            parameters[WorldGen.WORLD_PARAMS_YSIZE] = ySize;
            parameters[WorldGen.WORLD_PARAMS_SEED] = seed;

            MapMaker.Parameters = parameters;

            MapMaker.generate( map, MapType.WORLD);

            GameMap gameMap = new GameMap(map);

            Entity e = m_EcsInstance.create();
            m_EcsInstance.EntityManager.addComponent(e, gameMap);

            m_EcsInstance.TagManager.tagEntity("MAP", e);

            m_EcsInstance.refresh(e);

            return gameMap;
        }

        public GameMap recreateWorldMap(GameMap map)
        {
            Entity e = m_EcsInstance.create();
            m_EcsInstance.EntityManager.addComponent(e, map);

            m_EcsInstance.TagManager.tagEntity("MAP", e);

            m_EcsInstance.refresh(e);

            return map;
        }

        public Vector2 findSafeLocation(GameMap map)
        {
            for (int i = 0; i < map.Map.XSize; i++)
            {
                for (int j = 0; j < map.Map.YSize; j++)
                {
                    if (!map.Map.Terrain[i, j].IsBlocking)
                        return new Vector2(i*32, j*32);
                }
            }

            return Vector2.Zero;
        }
    }
}
