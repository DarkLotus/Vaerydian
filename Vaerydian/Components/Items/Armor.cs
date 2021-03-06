﻿/*
 Author:
      Thomas H. Jonell <@Net_Gnome>
 
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

namespace Vaerydian.Components.Items
{
	public struct ArmorDef{
		public string Name;
	}

    class Armor : IComponent
    {
        private static int a_TypeID;
        private int a_EntityID;

        public Armor() { }

        public Armor(int mitigation, int mobility)
        {
            a_Mitigation = mitigation;
            a_Mobility = mobility;
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

        private int a_Mitigation;
        /// <summary>
        /// the amount of lethality the armor can mitigate
        /// </summary>
        public int Mitigation
        {
          get { return a_Mitigation; }
          set { a_Mitigation = value; }
        }

        private int a_Mobility;
        /// <summary>
        /// the mobility cost of the armor
        /// </summary>
        public int Mobility
        {
          get { return a_Mobility; }
          set { a_Mobility = value; }
        }
    }
}
