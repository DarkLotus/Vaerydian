using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Utils;

namespace Vaerydian.Components.Spatials
{
    class SpatialPartition : IComponent
    {
        private static int s_TypeID;
        private int s_EntityID;

        public SpatialPartition() { }

        public int getEntityId()
        {
            return s_EntityID;
        }

        public int getTypeId()
        {
            return s_TypeID;
        }

        public void setEntityId(int entityId)
        {
            s_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            s_TypeID = typeId;
        }

        private QuadTree<Entity> s_QuadTree;

        public QuadTree<Entity> QuadTree
        {
            get { return s_QuadTree; }
            set { s_QuadTree = value; }
        }

    }
}
