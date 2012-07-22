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
