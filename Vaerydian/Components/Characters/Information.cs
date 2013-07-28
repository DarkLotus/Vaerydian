using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaerydian.Characters;

using ECSFramework;

namespace Vaerydian.Components.Characters
{
    class Information : IComponent
    {
        private static int i_TypeID;
        private int i_EntityID;

        public Information() { }

        public int getEntityId()
        {
            return i_EntityID;
        }

        public int getTypeId()
        {
            return i_TypeID;
        }

        public void setEntityId(int entityId)
        {
            i_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            i_TypeID = typeId;
        }

        private String i_Name;
        /// <summary>
        /// name of creature
        /// </summary>
        public String Name
        {
            get { return i_Name; }
            set { i_Name = value; }
        }

        /// <summary>
        /// type of creature
        /// </summary>
		public string CreatureGeneralGroup;
        /// <summary>
        /// type of creature variation
        /// </summary>
		public string CreatureVariationGroup;
        /// <summary>
        /// type of unique creature
        /// </summary>
		public string CreatureUniqueGroup;
    }
}
