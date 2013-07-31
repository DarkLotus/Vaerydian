/*
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

namespace Vaerydian.Components.Items
{
    class Equipment : IComponent
    {
        private static int e_TypeID;
        private int e_EntityID;

        public Equipment() { }

        public int getEntityId()
        {
            return e_EntityID;
        }

        public int getTypeId()
        {
            return e_TypeID;
        }

        public void setEntityId(int entityId)
        {
            e_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            e_TypeID = typeId;
        }

        private Entity a_RangedWeapon;
        /// <summary>
        /// the given entity's ranged weapon entity reference
        /// </summary>
        public Entity RangedWeapon
        {
            get { return a_RangedWeapon; }
            set { a_RangedWeapon = value; }
        }

        private Entity a_MeleeWeapon;

        /// <summary>
        /// the given entity's melee weapon entity reference
        /// </summary>
        public Entity MeleeWeapon
        {
            get { return a_MeleeWeapon; }
            set { a_MeleeWeapon = value; }
        }

        private Entity a_Armor;
        /// <summary>
        /// the given entities armor entity reference
        /// </summary>
        public Entity Armor
        {
            get { return a_Armor; }
            set { a_Armor = value; }
        }
    }
}
