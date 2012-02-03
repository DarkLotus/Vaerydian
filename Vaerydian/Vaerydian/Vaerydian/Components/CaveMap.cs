using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using WorldGeneration.Cave;

namespace Vaerydian.Components
{
    class CaveMap : IComponent
    {
        private static int c_TypeID;
        private int c_EntityID;

        private CaveEngine c_Cave;

        public CaveEngine Cave
        {
            get { return c_Cave; }
            set { c_Cave = value; }
        }

        public CaveMap() { }

        public CaveMap(CaveEngine cave)
        {
            c_Cave = cave;
        }

        public int getEntityId()
        {
            return c_EntityID;
        }

        public int getTypeId()
        {
            return c_TypeID;
        }

        public void setEntityId(int entityId)
        {
            c_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            c_TypeID = typeId;
        }
    }
}
