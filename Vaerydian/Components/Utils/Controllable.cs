using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

namespace Vaerydian.Components.Utils
{
    public class Controllable : IComponent
    {
        private static int c_TypeID;
        private int c_EntityID;

        public Controllable() { }

        public int getEntityId()
        {
            return c_EntityID;
        }

        public int getTypeId()
        {
            return c_TypeID;
        }

		public static int TypeId{ get { return c_TypeID; } set { c_TypeID = value; } }

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
