using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Characters
{
    public enum FactionType
    {
        Player,
        Wilderness,
        Ally
    }


    public class Faction
    {

        public Faction(int value, FactionType type)
        {
            f_Value = value;
            f_FactionType = type;
        }

        private int f_Value;

        public int Value
        {
            get { return f_Value; }
            set { f_Value = value; }
        }

        private FactionType f_FactionType;

        public FactionType FactionType
        {
            get { return f_FactionType; }
            set { f_FactionType = value; }
        }


    }
}
