using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Utils;


namespace Vaerydian.Components.Dbg
{
    public class MapDebug : IComponent
    {

        private static int m_TypeID;
        private int m_EntityID;

        private List<Cell> m_Path = new List<Cell>();

        public List<Cell> Path
        {
            get { return m_Path; }
            set { m_Path = value; }
        }

        private List<Cell> m_Blocking = new List<Cell>();

        public List<Cell> Blocking
        {
            get { return m_Blocking; }
            set { m_Blocking = value; }
        }

        /*private List<Cell> m_OpenSet = new List<Cell>();

        public List<Cell> OpenSet
        {
            get { return m_OpenSet; }
            set { m_OpenSet = value; }
        }*/

        private BinaryHeap<Cell> m_OpenSet = new BinaryHeap<Cell>();

        public BinaryHeap<Cell> OpenSet
        {
            get { return m_OpenSet; }
            set { m_OpenSet = value; }
        }

        private List<Cell> m_ClosedSet = new List<Cell>();

        public List<Cell> ClosedSet
        {
            get { return m_ClosedSet; }
            set { m_ClosedSet = value; }
        }

        public MapDebug() { }

        public int getEntityId()
        {
            return m_EntityID;
        }

        public int getTypeId()
        {
            return m_TypeID;
        }

        public void setEntityId(int entityId)
        {
            m_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            m_TypeID = typeId;
        }
    }
}
