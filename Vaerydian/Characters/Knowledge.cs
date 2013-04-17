using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Characters
{
    public class Knowledge
    {
        private float e_Value;
        /// <summary>
        /// experience value
        /// </summary>
        public float Value
        {
            get { return e_Value; }
            set { e_Value = value; }
        }

        public Knowledge()
        {
            e_Value = 0;
        }

        public Knowledge(float value)
        {
            e_Value = value;
        }

    }
}
