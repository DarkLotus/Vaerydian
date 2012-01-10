using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECSFramework
{
    public class TagManager
    {
        private ECSInstance s_ECSManager;


        public TagManager(ECSInstance ecsManager)
        {
            this.s_ECSManager = ecsManager;
        }

        public void delete(Entity e)
        {

        }
    }
}
