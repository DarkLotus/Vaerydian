/*
 Author:
      Thomas H. Jonell <thjonell@gmail.com>
 
 Copyright (c) 2013 Thomas H. Jonell

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Lesser General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;
using Vaerydian.Utils;
using Vaerydian.Characters;

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

        private StatType v_StatType;

        public StatType StatType
        {
            get { return v_StatType; }
            set { v_StatType = value; }
        }
    }
}
