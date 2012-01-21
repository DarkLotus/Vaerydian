using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

namespace Vaerydian.Components
{
    public class Controllable : IComponent
    {
        private static int c_TypeID;
        private static int c_EntityID;

        public Controllable() { }

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
