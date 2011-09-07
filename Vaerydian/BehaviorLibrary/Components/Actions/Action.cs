using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorLibrary.Components.Actions
{
    public abstract class Action
    {

        public Action() { }

        public abstract BehaviorReturnCode Behave();

    }
}
