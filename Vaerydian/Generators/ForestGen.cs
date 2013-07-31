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

using Vaerydian.Utils;

namespace Vaerydian.Generators
{
    public static class ForestGen
    {

		public const short FOREST_PARAMS_SIZE = 6;

		public const short FOREST_PARAMS_BASE_TERRAIN = 0;

		public const short FOREST_PARAMS_BLOCKING_TERRAIN = 1;

		public const short FOREST_PARAMS_X = 2;

		public const short FOREST_PARAMS_Y = 3;

		public const short FOREST_PARAMS_PROB = 4;

		public const short FOREST_PARAMS_SEED = 5;

        private static Random f_Rand;

		private static String f_StatusMessage = "Generating Forest...";

        public static String StatusMessage
        {
            get { return f_StatusMessage; }
            set { f_StatusMessage = value; }
        }

        public static bool generate( Map map, object[] parameters)
        {
            try
            {
                generateMap( map,
                            (short)parameters[FOREST_PARAMS_BASE_TERRAIN],
				            (short)parameters[FOREST_PARAMS_BLOCKING_TERRAIN],
				            (int) parameters[FOREST_PARAMS_X],
				            (int) parameters[FOREST_PARAMS_Y],
				            (int) parameters[FOREST_PARAMS_PROB],
                            (int)parameters[FOREST_PARAMS_SEED]
                            );
            }
            catch (Exception e)
            {
				Console.Error.WriteLine(e.ToString());
                return false;
            }

            return true;
        }


        private static void generateMap( Map map, short terrainBaseType,short terrainBlockType, int x, int y, int prob, int seed)
        {
            f_Rand = new Random(seed);

           // MapHelper.floodInitializeAll( map, terrainBlockType);

			MapHelper.floodInitializeAll (map, "BLOCK");

			MapHelper.floodAllOp(map,setBlocking);

			//MapHelper.floodFillSpecificOp(map,1,1,x-1,y-1,terrainBlockType,setNotBlocking);
			MapHelper.floodFillSpecificOp(map,1,1,x-1,y-1,"BLOCK",setNotBlocking);

			MapHelper.floodSpecificOp(map,1,1,x-1,y-1,foo, prob, terrainBaseType,terrainBlockType);

			MapHelper.floodAllOp(map, addVariation);


			//TODO: need to finish map generation


			//generate forest wall
			//MapHelper.floodSpecificOp(map, 1,1,

        }

		private static void addVariation(Terrain terrain, params object[] args)
        {
            terrain.Variation += (float)(0.125 - (f_Rand.NextDouble() * 0.25));
        }

        /// <summary>
        /// set terrain to blocking
        /// </summary>
        /// <param name="terrain">terrain to be set</param>
        /// <param name="args"></param>
        private static void setBlocking(Terrain terrain, params object[] args) { terrain.IsBlocking = true; }

        /// <summary>
        /// set terrain to not blocking
        /// </summary>
        /// <param name="terrain">terrain to be set</param>
        /// <param name="args"></param>
        private static void setNotBlocking(Terrain terrain, params object[] args) { terrain.IsBlocking = false; }

        private static void foo(Terrain terrain, params object[] args)
        {
            int prob = (int)args[0];
			short baseTerrain = (short)args[1];
			short blockerrain = (short)args[2];

            if (f_Rand.Next(100) >= prob)
            {
                //terrain.TerrainType = blockerrain;
				terrain.TerrainType = blockerrain;
				terrain.ObjectType = ObjectType.TREE;
                terrain.IsBlocking = true;
            }
            else
            {
                terrain.TerrainType = baseTerrain;
            }
        }

    }
}
