using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Characters.Attributes;

namespace Vaerydian.Components.Characters
{
    public enum AttributeType
    {
        Muscle,
        Endurance,
        Mind,
        Personality,
        Quickness,
        Perception,
        Focus
    }

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

        private Dictionary<AttributeType, int> a_AttributeSet = new Dictionary<AttributeType, int>();

        public Dictionary<AttributeType, int> AttributeSet
        {
            get { return a_AttributeSet; }
            set { a_AttributeSet = value; }
        }


        private AttributeStat a_Muscle = new AttributeStat();
        /// <summary>
        /// muscle of entity
        /// </summary>
        public AttributeStat Muscle
        {
            get { return a_Muscle; }
            set { a_Muscle = value; }
        }
        private AttributeStat a_Endurance = new AttributeStat();
        /// <summary>
        /// endurance of entity
        /// </summary>
        public AttributeStat Endurance
        {
            get { return a_Endurance; }
            set { a_Endurance = value; }
        }
        private AttributeStat a_Mind = new AttributeStat();
        /// <summary>
        /// mind of entity
        /// </summary>
        public AttributeStat Mind
        {
            get { return a_Mind; }
            set { a_Mind = value; }
        }
        private AttributeStat a_Personality = new AttributeStat();
        /// <summary>
        /// personality of entity
        /// </summary>
        public AttributeStat Personality
        {
            get { return a_Personality; }
            set { a_Personality = value; }
        }
        private AttributeStat a_Quickness = new AttributeStat();
        /// <summary>
        /// quickness of entity
        /// </summary>
        public AttributeStat Quickness
        {
            get { return a_Quickness; }
            set { a_Quickness = value; }
        }

        private AttributeStat a_Perception = new AttributeStat();
        //perception of entity
        public AttributeStat Perception
        {
            get { return a_Perception; }
            set { a_Perception = value; }
        }

    }
}
