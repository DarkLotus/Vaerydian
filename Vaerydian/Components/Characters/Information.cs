using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaerydian.Characters.Experience;

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

        private CreatureGeneralGroup e_CreatureGenerapGroup;
        /// <summary>
        /// type of creature
        /// </summary>
        public CreatureGeneralGroup CreatureGeneralGroup
        {
            get { return e_CreatureGenerapGroup; }
            set { e_CreatureGenerapGroup = value; }
        }

        private CreatureVariationGroup e_CreatureVariationGroup;
        /// <summary>
        /// type of creature variation
        /// </summary>
        public CreatureVariationGroup CreatureVariationGroup
        {
            get { return e_CreatureVariationGroup; }
            set { e_CreatureVariationGroup = value; }
        }

        private CreatureUniqueGroup e_CreatureUniqueGroup;
        /// <summary>
        /// type of unique creature
        /// </summary>
        public CreatureUniqueGroup CreatureUniqueGroup
        {
            get { return e_CreatureUniqueGroup; }
            set { e_CreatureUniqueGroup = value; }
        }
    }
}
