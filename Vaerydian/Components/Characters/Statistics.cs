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

        /// <summary>
        /// muscle of entity
        /// </summary>
		public Statistic Muscle;
        /// <summary>
        /// endurance of entity
        /// </summary>
		public Statistic Endurance;
        /// <summary>
        /// mind of entity
        /// </summary>
		public Statistic Mind;
        /// <summary>
        /// personality of entity
        /// </summary>
		public Statistic Personality;
        /// <summary>
        /// quickness of entity
        /// </summary>
		public Statistic Quickness;
        /// <summary>
		/// perception of entity
        /// </summary>
		public Statistic Perception;
		/// <summary>
		/// focus of the entity
		/// </summary>
		public Statistic Focus;

    }
}
