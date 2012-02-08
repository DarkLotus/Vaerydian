using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using WorldGeneration.Utils;

using Vaerydian.Utils;

namespace Vaerydian.Components
{
    public class GameMap : IComponent
    {
        private static int g_TypeID;
        private int g_EntityID;

        private Map g_Map;

        public Map Map
        {
            get { return g_Map; }
            set { g_Map = value; }
        }

        private byte[,] g_ByteMap;

        public byte[,] ByteMap
        {
            get { return g_ByteMap; }
            set { g_ByteMap = value; }
        }

        private List<Cell> g_Path = new List<Cell>();

        public List<Cell> Path
        {
            get { return g_Path; }
            set { g_Path = value; }
        }

        private List<Cell> g_Blocking = new List<Cell>();

        public List<Cell> Blocking
        {
            get { return g_Blocking; }
            set { g_Blocking = value; }
        }

        public GameMap() { }

        public GameMap(Map map)
        {
            g_Map = map;
        }

        public int getEntityId()
        {
            return g_EntityID;
        }

        public int getTypeId()
        {
            return g_TypeID;
        }

        public void setEntityId(int entityId)
        {
            g_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            g_TypeID = typeId;
        }

        public Terrain getTerrain(int x, int y)
        {

            if ((x < g_Map.XSize) && (y < g_Map.YSize) && (x >= 0) && (y >= 0))
                return g_Map.Terrain[x, y];
            else
                return null;// new CaveTerrain();
        }
    }
}
