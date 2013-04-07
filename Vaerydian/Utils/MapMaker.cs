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

        private static short m_CurrentMapType;

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
        public static bool generate( Map map, short type)
        {
            //set type
            map.MapType = type;
            m_CurrentMapType = type;

            //call appropriate generator and generate the map
            switch (type)
            {
                case MapType_Old.CAVE:
                    return CaveGen.generate( map, m_Params);
                case MapType_Old.CITY:
                    break;
                case MapType_Old.DUNGEON:
					return DungeonGen.generate(map,m_Params);
                case MapType_Old.FORT:
                    break;
                case MapType_Old.OUTPOST:
                    break;
                case MapType_Old.NEXUS:
                    break;
                case MapType_Old.TOWER:
                    break;
                case MapType_Old.TOWN:
                    break;
                case MapType_Old.WORLD:
                    return WorldGen.generate( map, m_Params);
                case MapType_Old.WILDERNESS:
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
                    case MapType_Old.CAVE:
                        return CaveGen.StatusMessage;
                    case MapType_Old.CITY:
                        break;
                    case MapType_Old.DUNGEON:
						return DungeonGen.StatusMessage;
                    case MapType_Old.FORT:
                        break;
                    case MapType_Old.OUTPOST:
                        break;
                    case MapType_Old.NEXUS:
                        break;
                    case MapType_Old.TOWER:
                        break;
                    case MapType_Old.TOWN:
                        break;
                    case MapType_Old.WORLD:
                        return WorldGen.StatusMessage;
					case MapType_Old.WILDERNESS:
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
