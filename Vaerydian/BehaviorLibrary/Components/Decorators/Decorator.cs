using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorLibrary.Components.Decorators
{
    public abstract class Decorator : BehaviorComponent
    {

        protected BehaviorComponent d_Behavior;

        public Decorator(BehaviorComponent behavior) 
        {
            d_Behavior = behavior;
        }

        public abstract BehaviorReturnCode Behave();

    }
}
