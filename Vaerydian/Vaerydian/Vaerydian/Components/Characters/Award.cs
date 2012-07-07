using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;
using Vaerydian.Utils;

namespace Vaerydian.Components.Characters
{

    public enum AwardType
    {
        Victory,
        SkillUp,
        Attribute,
        Health
    }


    class Award : IComponent
    {
        private static int v_TypeID;
        private int v_EntityID;

        public Award() { }

        public int getEntityId()
        {
            return v_EntityID;
        }

        public int getTypeId()
        {
            return v_TypeID;
        }

        public void setEntityId(int entityId)
        {
            v_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            v_TypeID = typeId;
        }

        private AwardType v_AwardType;
        /// <summary>
        /// type of award
        /// </summary>
        public AwardType AwardType
        {
            get { return v_AwardType; }
            set { v_AwardType = value; }
        }

        private Entity v_Owner;
        /// <summary>
        /// entity awarding victory (defeated enemy)
        /// </summary>
        public Entity Awarder
        {
            get { return v_Owner; }
            set { v_Owner = value; }
        }

        private Entity v_Receiver;
        /// <summary>
        /// entity receiving victory
        /// </summary>
        public Entity Receiver
        {
            get { return v_Receiver; }
            set { v_Receiver = value; }
        }

        private int v_MaxAwardable;
        /// <summary>
        /// maximum knowledge awardable
        /// </summary>
        public int MaxAwardable
        {
            get { return v_MaxAwardable; }
            set { v_MaxAwardable = value; }
        }

        private int v_MinAwardable = 1;
        /// <summary>
        /// minimum knowledge awardable
        /// </summary>
        public int MinAwardable
        {
            get { return v_MinAwardable; }
            set { v_MinAwardable = value; }
        }

        private SkillName v_SkillName;
        /// <summary>
        /// name of what is to be awarded
        /// </summary>
        public SkillName SkillName
        {
            get { return v_SkillName; }
            set { v_SkillName = value; }
        }

        private AttributeType v_AttributeType;

        public AttributeType AttributeType
        {
            get { return v_AttributeType; }
            set { v_AttributeType = value; }
        }
    }
}
