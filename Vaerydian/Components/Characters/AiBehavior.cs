using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using BehaviorLibrary;

using Vaerydian.Behaviors;

namespace Vaerydian.Components.Characters
{
    class AiBehavior : IComponent
    {
        private static int a_TypeID;

        public static int TypeID
        {
            get { return AiBehavior.a_TypeID; }
            set { AiBehavior.a_TypeID = value; }
        }
        private int a_EntityID;

        private CharacterBehavior a_Behavior;

        /// <summary>
        /// current behavior of this AI
        /// </summary>
        public CharacterBehavior Behavior
        {
            get { return a_Behavior; }
            set { a_Behavior = value; }
        }

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
