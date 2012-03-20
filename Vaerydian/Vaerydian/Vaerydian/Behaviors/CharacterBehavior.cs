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
        
        public abstract void deathCleanup();

        protected bool c_IsClean = false;

        public bool IsClean
        {
            get { return c_IsClean; }
            set { c_IsClean = value; }
        }

    }
}
