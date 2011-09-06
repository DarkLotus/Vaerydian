using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorLibrary.Components.Flowpaths
{
    class RootSelector : Selector
    {

        private BehaviorComponent[] rs_Behaviors;

        public RootSelector(params BehaviorComponent[] behaviors)
        {
            rs_Behaviors = behaviors;
        }

        public BehaviorReturnCode Behave()
        {


            return BehaviorReturnCode.Success;
        }

    }
}
