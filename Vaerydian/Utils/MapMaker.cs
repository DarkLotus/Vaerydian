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

using Vaerydian.Generators;

namespace Vaerydian.Utils
{
    /// <summary>
    /// Class for creation of maps
    /// </summary>
    public static class MapMaker
    {
        private static object[] m_Params;
        /// <summary>
        /// passable generator parameters
        /// </summary>
        public static object[] Parameters
        {
            get { return m_Params; }
            set { m_Params = value; }
        }

		private static MapType m_CurrentMapType;

        /// <summary>
        /// creates a base map of dimesions x and y
        /// </summary>
        /// <param name="x">x dimension</param>
        /// <param name="y">y dimension</param>
        /// <returns>a blank map</returns>
        public static Map create(int x, int y)
        {
            return new Map(x,y);
        }

        /// <summary>
        /// generate the type of map
        /// </summary>
        /// <param name="map">map to be used</param>
        /// <param name="type">type of map to generate</param>
        /// <returns>true upon success, otherwise false</returns>
		public static bool generate( Map map, MapType type)
        {
            //set type
            map.MapDef.MapType = type;
            m_CurrentMapType = type;

            //call appropriate generator and generate the map
            switch (type)
            {
			case MapType.CAVE:
                return CaveGen.generate( map, m_Params);
			case MapType.CITY:
                break;
			case MapType.DUNGEON:
				return DungeonGen.generate(map,m_Params);
			case MapType.FORT:
                break;
			case MapType.OUTPOST:
                break;
			case MapType.NEXUS:
            	break;
			case MapType.TOWER:
                break;
			case MapType.TOWN:
                break;
			case MapType.WORLD:
                return WorldGen.generate( map, m_Params);
			case MapType.WILDERNESS:
				return ForestGen.generate(map, m_Params);
            default:
            	return false;//no map created
            }

            //no map was created
            return false;
        }

		private static String m_StatusMessage = "Generating Map...";

        public static String StatusMessage
        {
            get
            {
                switch (m_CurrentMapType)
                {
				case MapType.CAVE:
                        return CaveGen.StatusMessage;
				case MapType.CITY:
                        break;
				case MapType.DUNGEON:
						return DungeonGen.StatusMessage;
				case MapType.FORT:
                        break;
				case MapType.OUTPOST:
                        break;
				case MapType.NEXUS:
                        break;
				case MapType.TOWER:
                        break;
				case MapType.TOWN:
                        break;
				case MapType.WORLD:
                        return WorldGen.StatusMessage;
				case MapType.WILDERNESS:
						return ForestGen.StatusMessage;
                    default:
                        return m_StatusMessage;
                }

                return m_StatusMessage;
            }
            set { m_StatusMessage = value; }
        }

    }
}
