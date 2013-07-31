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
using System.Text;

namespace Vaerydian.Utils
{
    /// <summary>
    /// class for helping with various map making tasks
    /// </summary>
    public static class MapHelper
    {
       
        /// <summary>
        /// operation that is performed on a piece of terrain
        /// </summary>
        /// <param name="terrain">terrain the operation is against</param>
        public delegate void TerrainOp( Terrain terrain);

        /// <summary>
        /// operation that is performed on a piece of terrain, may have arguments
        /// </summary>
        /// <param name="terrain">terrain operation is performed against</param>
        /// <param name="args">arguments for operation</param>
        public delegate void Operation( Terrain terrain, params Object[] args);

        public static Random Random = new Random();

        /// <summary>
        /// counds the number of neighbors of a cell of a given terrain type
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordiante</param>
        /// <param name="map">map in which cell resides</param>
        /// <param name="terrainType">terrain type to count</param>
        /// <returns>number of neighbors of the specified terrain type</returns>
        public static int countOfType(int x, int y, Map map, short terrainType)
        {
            int neighbors = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (map.Terrain[x + i, y + j].TerrainType == terrainType)
                        neighbors++;
                }
            }

            return neighbors;
        }

        public static int countOfType(int x, int y, Map map, string tileName)
        {
            int neighbors = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    List<TileDef> tiles = map.MapDef.Tiles[tileName];

                    foreach (TileDef tile in tiles)
                    {
                        if (map.Terrain[x + i, y + j].TerrainDef.Name == tile.TerrainDef.Name)
                            neighbors++;
                    }
                }
            }

            return neighbors;
        }

        public static bool isOfTileType(Map map, Terrain terrain, string tileName)
        {
            List<TileDef> tiles = map.MapDef.Tiles[tileName];

            foreach (TileDef tile in tiles)
            {
                if (terrain.TerrainDef.Name == tile.TerrainDef.Name)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// initializes a map with new terrain of the given type
        /// </summary>
        /// <param name="map">map to be initialized</param>
        /// <param name="terrainType">terrain fill type</param>
        public static void floodInitializeAll(Map map, short terrainType)
        {
            for (int i = 0; i < map.XSize; i++)
            {
                for (int j = 0; j < map.YSize; j++)
                {
                    Terrain terrain = new Terrain();
                    terrain.TerrainType = terrainType;
                    map.Terrain[i, j] = terrain;
                }
            }
        }

        public static void floodInitializeAll(Map map, string tileName)
        {
            for (int i = 0; i < map.XSize; i++)
            {
                for (int j = 0; j < map.YSize; j++)
                {
                    Terrain terrain = new Terrain();
                    terrain.TerrainDef = MapHelper.getMapTerrainDef(map.MapDef.Name, tileName);//GameConfig.TerrainDefs[terrainDefName];
                    map.Terrain[i, j] = terrain;
                }
            }
        }


        /// <summary>
        /// fills the entire map with the given terrain type
        /// </summary>
        /// <param name="map">map to be flood filled</param>
        /// <param name="terrainType">terrain type to be used</param>
        public static void floodFillAll( Map map, short terrainType)
        {
            for (int i = 0; i < map.XSize; i++)
            {
                for (int j = 0; j < map.YSize; j++)
                {
                    map.Terrain[i, j].TerrainType = terrainType;
                }
            }
        }

        /// <summary>
        /// fills the entire map with the given terrain type and operation
        /// </summary>
        /// <param name="map">map to be flood filled</param>
        /// <param name="terrainType">terrain type to be used</param>
        /// <param name="operation">opperation to be performed on each terrain</param>
        public static void floodFillAllOp( Map map, short terrainType, Operation operation)
        {
            for (int i = 0; i < map.XSize; i++)
            {
                for (int j = 0; j < map.YSize; j++)
                {
                    map.Terrain[i, j].TerrainType = terrainType;
                    operation( map.Terrain[i, j]);
                }
            }
        }

        /// <summary>
        /// fills a portion of a map with a given terrain type
        /// </summary>
        /// <param name="map">map to flood filled</param>
        /// <param name="x">starting x coordinate</param>
        /// <param name="y">starting y coordinate</param>
        /// <param name="dx">ending x coordinate</param>
        /// <param name="dy">ending y coordinate</param>
        /// <param name="terrainType">terrain type to be used</param>
        public static void floodFillSpecific( Map map, int x, int y, int dx, int dy, short terrainType)
        {
            for (int i = x; i < dx; i++)
            {
                for (int j = y; j < dy; j++)
                {
                    map.Terrain[i, j].TerrainType = terrainType;
                }
            }
        }

        public static void floodFillSpecific(Map map, int x, int y, int dx, int dy, string tileName)
        {
            for (int i = x; i < dx; i++)
            {
                for (int j = y; j < dy; j++)
                {
                    map.Terrain[i, j].TerrainDef = MapHelper.getMapTerrainDef(map.MapDef.Name, tileName);
                }
            }
        }

        /// <summary>
        /// fills a portion of a map with a given terrain type and operation
        /// </summary>
        /// <param name="map">map to flood filled</param>
        /// <param name="x">starting x coordinate</param>
        /// <param name="y">starting y coordinate</param>
        /// <param name="dx">ending x coordinate</param>
        /// <param name="dy">ending y coordinate</param>
        /// <param name="terrainType">terrain type to be used</param>
        /// <param name="operation">operation to be performed on each terrain</param>
        public static void floodFillSpecificOp( Map map, int x, int y, int dx, int dy, short terrainType, Operation operation)
        {
            for (int i = x; i < dx; i++)
            {
                for (int j = y; j < dy; j++)
                {
                    map.Terrain[i, j].TerrainType = terrainType;
                    operation( map.Terrain[i, j]);
                }
            }
        }

        public static void floodFillSpecificOp(Map map, int x, int y, int dx, int dy, string tileName, Operation operation)
        {
            for (int i = x; i < dx; i++)
            {
                for (int j = y; j < dy; j++)
                {
                    map.Terrain[i, j].TerrainDef = MapHelper.getMapTerrainDef(map.MapDef.Name, tileName);
                    operation(map.Terrain[i, j]);
                }
            }
        }


        /// <summary>
        /// performs given operation against all terrain
        /// </summary>
        /// <param name="map">map used</param>
        /// <param name="operation">operation to perform</param>
        /// <param name="args">arguments to pass to operation (if any)</param>
        public static void floodAllOp( Map map, Operation operation, params object[] args)
        {
            for (int i = 0; i < map.XSize; i++)
            {
                for (int j = 0; j < map.YSize; j++)
                {
                    operation( map.Terrain[i, j], args);
                }
            }
        }

        /// <summary>
        /// performes given operation against a specified index of terrain
        /// </summary>
        /// <param name="map">map to use</param>
        /// <param name="x">x starting coordinate</param>
        /// <param name="y">y starting coordinate</param>
        /// <param name="dx">x ending coordinate</param>
        /// <param name="dy">y ending coordinate</param>
        /// <param name="operation">operation to perform</param>
        /// <param name="args">arguments to apss to operation (if any)</param>
        public static void floodSpecificOp( Map map, int x, int y, int dx, int dy, Operation operation, params object[] args)
        {
            for (int i = x; i < dx; i++)
            {
                for (int j = y; j < dy; j++)
                {
                    operation( map.Terrain[i, j], args);
                }
            }
        }


        /// <summary>
        /// performs given operation against all terrain, but also explicitly passes operation the terrain's coordinates as 1st two arguments
        /// </summary>
        /// <param name="map">map used</param>
        /// <param name="operation">operation to perform</param>
        /// <param name="args">arguments to pass to operation (if any)</param>
        public static void floodAllOpCoords( Map map, Operation operation, params object[] args)
        {
            for (int i = 0; i < map.XSize; i++)
            {
                for (int j = 0; j < map.YSize; j++)
                {
                    object[] args2 = new object[args.Length + 2];
                    args2[0] = i;
                    args2[1] = j;
                    Array.Copy(args, 0, args2, 2, args.Length);
                    operation( map.Terrain[i, j], args2);
                }
            }
        }

        /// <summary>
        /// performes given operation against a specified index of terrain, but also explicitly passes operation the terrain's coordinates as 1st two arguments
        /// </summary>
        /// <param name="map">map to use</param>
        /// <param name="x">x starting coordinate</param>
        /// <param name="y">y starting coordinate</param>
        /// <param name="dx">x ending coordinate</param>
        /// <param name="dy">y ending coordinate</param>
        /// <param name="operation">operation to perform</param>
        /// <param name="args">arguments to apss to operation (if any)</param>
        public static void floodSpecificOpCoords( Map map, int x, int y, int dx, int dy, Operation operation, params object[] args)
        {
            for (int i = x; i < dx; i++)
            {
                for (int j = y; j < dy; j++)
                {
                    object[] args2 = new object[args.Length+2];
                    args2[0] = i;
                    args2[1] = j;
                    Array.Copy(args, 0, args2, 2, args.Length);
                    operation( map.Terrain[i, j], args2);
                }
            }
        }

        /// <summary>
        /// linearly interpolate x between values a and b
        /// </summary>
        /// <param name="a">max value</param>
        /// <param name="b">min value</param>
        /// <param name="x">value to linearly interpolate</param>
        /// <returns></returns>
        public static float lerp(float a, float b, float x)
        {
            return (a * (1f - x) + b * x);
        }

        /// <summary>
        /// sets terrain based on configuration defined map name and terrain name
        /// </summary>
        /// <param name="terrain"></param>
        /// <param name="mapName"></param>
        /// <param name="tileName"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        public static Terrain setTerrain(Terrain terrain, string mapName, string tileName)
        {
            MapDef mDef = GameConfig.MapDefs[mapName];

            List<TileDef> tiles = mDef.Tiles[tileName];

            TerrainDef tDef = tiles[Random.Next(0, tiles.Count)].TerrainDef;

            terrain.TerrainDef = tDef;
            terrain.IsBlocking = !tDef.Passible;
            terrain.TerrainType = tDef.ID;

            return terrain;
        }

        public static TerrainDef getMapTerrainDef(string mapName, string tileName)
        {
            MapDef mapDef = GameConfig.MapDefs[mapName];

            List<TileDef> tiles = mapDef.Tiles[tileName];

            return tiles[Random.Next(0, tiles.Count - 1)].TerrainDef;
        }
    }
}
