using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECSFramework
{
    public interface IComponent
    {
        private int c_EntityId;

        public int EntityId
        {
            get { return c_EntityId; }
            set { c_EntityId = value; }
        }

        private int c_TypeId;

        public int TypeId
        {
            get { return c_TypeId; }
            set { c_TypeId = value; }
        }

        public IComponent() { }

        public IComponent(int entityId) { }

    }
}
