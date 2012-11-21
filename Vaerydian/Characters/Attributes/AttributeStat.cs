using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Characters.Attributes
{
    public class AttributeStat
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public AttributeStat() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public AttributeStat(String name, int value)
        {
            a_Name = name;
            a_Value = value;
        }

        /// <summary>
        /// name of the skill
        /// </summary>
        private String a_Name;
        /// <summary>
        /// name of the skill
        /// </summary>
        public String Name
        {
            get { return a_Name; }
            set { a_Name = value; }
        }

        /// <summary>
        /// current value of the skill
        /// </summary>
        private int a_Value;
        /// <summary>
        /// current value of the skill
        /// </summary>
        public int Value
        {
            get { return a_Value; }
            set { a_Value = value; }
        }
    }
}
