/*
 Author:
      Thomas H. Jonell <thjonell AT gmail DOT com>
 
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
using System.Text;

using ECSFramework;

using Vaerydian.Components;
using Vaerydian.Components.Utils;

using Vaerydian.Utils;
using Vaerydian.Generators;

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

		public GameMap createRandomDungeonMap (int x, int y,int features, int seed)
		{
			Map map = MapMaker.create(x,y);

			object[] parameters = new object[DungeonGen.DUNGEON_PARAMS_SIZE];

			parameters[DungeonGen.DUNGEON_PARAMS_XSIZE] = x;
			parameters[DungeonGen.DUNGEON_PARAMS_YSIZE] = y;
			parameters[DungeonGen.DUNGEON_PARAMS_SEED] = seed;
			parameters[DungeonGen.DUNGEON_PARAMS_FEATURE_COUNT] = features;

			MapMaker.Parameters = parameters;

			MapMaker.generate(map,MapType.DUNGEON);

			GameMap gameMap = new GameMap(map);

			Entity e = m_EcsInstance.create();
            m_EcsInstance.EntityManager.addComponent(e, gameMap);

            m_EcsInstance.TagManager.tagEntity("MAP", e);

            m_EcsInstance.refresh(e);

            return gameMap;
		}

		public GameMap createRandomForestMap (int x, int y, int prob, short baseTerrain, short blockingTerrain, int seed)
		{
			Map map = MapMaker.create(x,y);

			object[] parameters = new object[ForestGen.FOREST_PARAMS_SIZE];

			parameters[ForestGen.FOREST_PARAMS_X] = x;
			parameters[ForestGen.FOREST_PARAMS_Y] = y;
			parameters[ForestGen.FOREST_PARAMS_PROB] = prob;
			parameters[ForestGen.FOREST_PARAMS_BASE_TERRAIN] = baseTerrain;
			parameters[ForestGen.FOREST_PARAMS_BLOCKING_TERRAIN] = blockingTerrain;
			parameters[ForestGen.FOREST_PARAMS_SEED] = seed;

			MapMaker.Parameters = parameters;

			MapMaker.generate(map,MapType.WILDERNESS);

			GameMap gameMap = new GameMap(map);

			Entity e = m_EcsInstance.create();
            m_EcsInstance.EntityManager.addComponent(e, gameMap);

            m_EcsInstance.TagManager.tagEntity("MAP", e);

            m_EcsInstance.refresh(e);

            return gameMap;
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

        public static Vector2 findSafeLocation(GameMap map)
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
