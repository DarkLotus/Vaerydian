﻿/*
 Author:
      Thomas H. Jonell <thjonell AT gmail DOT com>
 
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
