using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Characters
{
	public enum StatType
	{
		NONE = 0,
		MUSCLE = 1,
		ENDURANCE = 2,
		MIND = 3,
		PERSONALITY = 4,
		QUICKNESS = 5,
		PERCEPTION = 6,
		FOCUS = 7
	}

    public class Statistic
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public Statistic() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Statistic(String name, int value)
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
