using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Characters.Experience
{
    public class Experience
    {
        private int e_Value;
        /// <summary>
        /// experience value
        /// </summary>
        public int Value
        {
            get { return e_Value; }
            set { e_Value = value; }
        }

        public Experience()
        {
            e_Value = 0;
        }

        public Experience(int value)
        {
            e_Value = value;
        }

    }
}
