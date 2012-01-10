using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework.Utils;

namespace ECSFramework
{
    public class ComponentManager
    {

        private ECSInstance c_ECSInstance;

        private Bag<Bag<IComponent>> c_ComponentsByType;

        private Dictionary<String, int> c_ComponentTypes;


        public ComponentManager(ECSInstance ecsInstance)
        {
            this.c_ECSInstance = ecsInstance;
        }


        public void removeComponents(Entity e)
        {

        }
    }
}
