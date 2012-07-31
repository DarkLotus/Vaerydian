using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Utils;

namespace Vaerydian.Components.Utils
{
    class BoundingPolygon : IComponent
    {

        private static int b_TypeID;
        private int b_EntityID;

        public BoundingPolygon() { }

        public int getEntityId()
        {
            return b_EntityID;
        }

        public int getTypeId()
        {
            return b_TypeID;
        }

        public void setEntityId(int entityId)
        {
            b_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            b_TypeID = typeId;
        }

        private Polygon b_Polygon;

        internal Polygon Polygon
        {
            get { return b_Polygon; }
            set { b_Polygon = value; }
        }
    }
}
