using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Items
{
    class Item
    {
        /// <summary>
        /// Name of item
        /// </summary>
        private String i_Name;
        /// <summary>
        /// name of item
        /// </summary>
        public String Name
        {
            get { return i_Name; }
            set { i_Name = value; }
        }

        /// <summary>
        /// Components the item is crafted from
        /// </summary>
        private List<Item> i_Components = new List<Item>();

        /// <summary>
        /// Components the item is crafted from
        /// </summary>
        public List<Item> Components
        {
            get { return i_Components; }
            set { i_Components = value; }
        }

        /// <summary>
        /// Value of the item
        /// </summary>
        private int i_Value = 0;

        /// <summary>
        /// value of the item
        /// </summary>
        public int Value
        {
            get { return i_Value; }
            set { i_Value = value; }
        }

        /// <summary>
        /// the item's current durability
        /// </summary>
        private int i_Durability;

        /// <summary>
        /// the item's current durability
        /// </summary>
        public int Durability
        {
            get { return i_Durability; }
            set { i_Durability = value; }
        }

        /// <summary>
        /// the item's maximum durability
        /// </summary>
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
