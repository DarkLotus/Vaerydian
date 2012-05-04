using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Characters.Stats;

namespace Vaerydian.Components.Characters
{
    class Attributes : IComponent
    {
        private static int a_TypeID;
        private int a_EntityID;

        public Attributes() { }

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

        private Stat a_Muscle = new Stat();
        /// <summary>
        /// muscle of entity
        /// </summary>
        public Stat Muscle
        {
            get { return a_Muscle; }
            set { a_Muscle = value; }
        }
        private Stat a_Endurance = new Stat();
        /// <summary>
        /// endurance of entity
        /// </summary>
        public Stat Endurance
        {
            get { return a_Endurance; }
            set { a_Endurance = value; }
        }
        private Stat a_Mind = new Stat();
        /// <summary>
        /// mind of entity
        /// </summary>
        public Stat Mind
        {
            get { return a_Mind; }
            set { a_Mind = value; }
        }
        private Stat a_Personality = new Stat();
        /// <summary>
        /// personality of entity
        /// </summary>
        public Stat Personality
        {
            get { return a_Personality; }
            set { a_Personality = value; }
        }
        private Stat a_Quickness = new Stat();
        /// <summary>
        /// quickness of entity
        /// </summary>
        public Stat Quickness
        {
            get { return a_Quickness; }
            set { a_Quickness = value; }
        }

        private Stat a_Perception = new Stat();
        //perception of entity
        public Stat Perception
        {
            get { return a_Perception; }
            set { a_Perception = value; }
        }

    }
}
