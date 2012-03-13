using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Characters.Factions
{
    public enum FactionType
    {
        Player,
        TestMob,
        Ally
    }


    public class Faction
    {

        public Faction(int value)
        {
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
