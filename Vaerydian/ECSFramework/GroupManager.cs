using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECSFramework
{
    public class GroupManager
    {
        private ECSInstance s_ECSManager;


        public GroupManager(ECSInstance ecsManager)
        {
            this.s_ECSManager = ecsManager;
        }

        public void delete(Entity e)
        {

        }
    }
}
