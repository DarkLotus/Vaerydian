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
	public enum MapType
	{
		NONE = 0,
		CAVE = 1,
		DUNGEON = 2,
		TOWN = 3,
		CITY = 4,
		TOWER = 5,
		OUTPOST = 6,
		FORT = 7,
		NEXUS = 8,
		WORLD = 9,
		WILDERNESS = 10
	}

	public struct TileDef{
		public TerrainDef TerrainDef;
		public int Probability;
	}

	public struct MapDef{
		public string Name;
		public MapType MapType;
		public Dictionary<string,List<TileDef>> Tiles;
	}

    public class Map
    {
                
        public Map(int xSize, int ySize)
        {
            m_xSize = xSize;
            m_ySize = ySize;
            m_Terrain = new Terrain[m_xSize, m_ySize];
        }

        private int m_xSize = 100;
        /// <summary>
        /// size of map in x-dimension
        /// </summary>
        public int XSize
        {
            get { return m_xSize; }
            set { m_xSize = value; }
        }

        private int m_ySize = 100;
        /// <summary>
        /// size of map in y-dimension
        /// </summary>
        public int YSize
        {
            get { return m_ySize; }
            set { m_ySize = value; }
        }

		public MapDef MapDef;

        private Terrain[,] m_Terrain;
        /// <summary>
        /// map's terrain
        /// </summary>
        public Terrain[,] Terrain
        {
            get { return m_Terrain; }
            set { m_Terrain = value; }
        }

        private int m_Seed;
        /// <summary>
        /// map's random seed
        /// </summary>
        public int Seed
        {
            get { return m_Seed; }
            set { m_Seed = value; }
        }

    }
}
