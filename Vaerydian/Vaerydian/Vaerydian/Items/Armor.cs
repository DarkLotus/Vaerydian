using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Items
{
    public class Armor : Item
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public Armor() { }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="defense"></param>
        /// <param name="mobility"></param>
        public Armor(int defense, int mobility)
        {
            a_Defense = defense;
            a_Mobility = mobility;
        }

        /// <summary>
        /// how much mobility the armor allows
        /// </summary>
        private int a_Mobility;

        /// <summary>
        /// how much mobility the armor allows
        /// </summary>
        public int Mobility
        {
            get { return a_Mobility; }
            set { a_Mobility = value; }
        }

        /// <summary>
        /// defensive value of the armor
        /// </summary>
        private int a_Defense;

        /// <summary>
        /// defensive value of the armor
        /// </summary>
        public int Defense
        {
            get { return a_Defense; }
            set { a_Defense = value; }
        }


    }
}
