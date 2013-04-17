using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Characters;

namespace Vaerydian.Components.Characters
{


    class Statistics : IComponent
    {
        private static int a_TypeID;
        private int a_EntityID;

        public Statistics() { }

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

        private Dictionary<StatType, int> a_AttributeSet = new Dictionary<StatType, int>();

        public Dictionary<StatType, int> StatisticSet
        {
            get { return a_AttributeSet; }
            set { a_AttributeSet = value; }
        }


        private Statistic a_Muscle = new Statistic();
        /// <summary>
        /// muscle of entity
        /// </summary>
        public Statistic Muscle
        {
            get { return a_Muscle; }
            set { a_Muscle = value; }
        }
        private Statistic a_Endurance = new Statistic();
        /// <summary>
        /// endurance of entity
        /// </summary>
        public Statistic Endurance
        {
            get { return a_Endurance; }
            set { a_Endurance = value; }
        }
        private Statistic a_Mind = new Statistic();
        /// <summary>
        /// mind of entity
        /// </summary>
        public Statistic Mind
        {
            get { return a_Mind; }
            set { a_Mind = value; }
        }
        private Statistic a_Personality = new Statistic();
        /// <summary>
        /// personality of entity
        /// </summary>
        public Statistic Personality
        {
            get { return a_Personality; }
            set { a_Personality = value; }
        }
        private Statistic a_Quickness = new Statistic();
        /// <summary>
        /// quickness of entity
        /// </summary>
        public Statistic Quickness
        {
            get { return a_Quickness; }
            set { a_Quickness = value; }
        }

        private Statistic a_Perception = new Statistic();
        //perception of entity
        public Statistic Perception
        {
            get { return a_Perception; }
            set { a_Perception = value; }
        }

    }
}
