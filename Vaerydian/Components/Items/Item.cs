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
    class Item : IComponent
    {
        private static int i_TypeID;
        private int i_EntityID;

        public Item() { }

        public Item(String name, int Value, int durability)
        {
            i_Name = name;
            i_Value = Value;
            i_CurrentDurability = durability;
            i_MaxDurability = durability;
        }

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
        /// name of item
        /// </summary>
        public String Name
        {
            get { return i_Name; }
            set { i_Name = value; }
        }

        private int i_Value;
        /// <summary>
        /// Value of the item
        /// </summary>
        public int Value
        {
            get { return i_Value; }
            set { i_Value = value; }
        }

        private int i_CurrentDurability;
        /// <summary>
        /// the item's current durability
        /// </summary>
        public int CurrentDurability
        {
            get { return i_CurrentDurability; }
            set { i_CurrentDurability = value; }
        }

        private int i_MaxDurability;
        /// <summary>
        /// the item's maximum durability
        /// </summary>
        public int MaxDurability
        {
            get { return i_MaxDurability; }
            set { i_MaxDurability = value; }
        }
    }
}
