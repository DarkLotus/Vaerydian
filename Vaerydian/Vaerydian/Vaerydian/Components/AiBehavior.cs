using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using BehaviorLibrary;

using Vaerydian.Behaviors;

namespace Vaerydian.Components
{
    class AiBehavior : IComponent
    {
        private static int a_TypeID;
        private int a_EntityID;

        private CharacterBehavior a_Behavior;

        public AiBehavior() { }

        public AiBehavior(CharacterBehavior behavior) 
        {
            a_Behavior = behavior;
        }

        public int getEntityId()
        {
            return a_EntityID;
        }

        public int getTypeId()
        {
            return a_TypeID;
        }

        public CharacterBehavior getBehavior()
        {
            return a_Behavior;
        }

        public void setEntityId(int entityId)
        {
            a_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            a_TypeID = typeId;
        }
    }
}
