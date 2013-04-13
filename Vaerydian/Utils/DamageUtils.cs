using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Utils
{
    public enum DamageClass
    {
        DIRECT,
        OVERTIME,
        AREA
    }

	public enum DamageBasis{
		NONE = 0,
		STATIC = 1,
		VARIABLE = 2,
		ATTRIBUTE = 3,
		SKILL = 4,
		KNOWLEDGE = 5,
		ITEM = 6,
	}
	
	public struct DamageDef{
		public string Name;
		public short ID;
		public DamageType DamageType;
		public DamageBasis DamageBasis;
	}

    public enum DamageType
    {
        NONE,
		SLASHING,
		CRUSHING,
		PIERCING,
		ICE,
		FIRE,
		EARTH,
		WIND,
		WATER,
		LIGHT,
		DARK,
		CHAOS,
		ORDER,
		POISON,
		DISEASE,
		ARCANE,
		MENTAL,
		SONIC
    }

	static class DamageUtils{

		public static int calcumateDamage(){
			return -1;
		}

	}
}
