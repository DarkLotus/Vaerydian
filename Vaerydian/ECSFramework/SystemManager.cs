using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework.Utils;

namespace ECSFramework
{
    public class SystemManager
    {
        private ECSInstance s_ECSManager;

        private Bag<EntitySystem> s_Systems;


        public SystemManager(ECSInstance ecsManager)
        {
            this.s_ECSManager = ecsManager;
            this.s_Systems = new Bag<EntitySystem>(8);
        }

        /// <summary>
        /// refresh the entity with all systems
        /// </summary>
        /// <param name="e">the entity to be refreshed</param>
        public void refresh(Entity e)
        {
            for (int i = 0; i < s_Systems.Size(); i++)
            {
                s_Systems.Get(i).refresh(e);
            }
        }
    }
}
