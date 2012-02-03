using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using BehaviorLibrary;

namespace Vaerydian.Behaviors
{
    public abstract class CharacterBehavior
    {
        public CharacterBehavior() { }
        public abstract BehaviorReturnCode Behave();
    }
}
