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
		ATTRIBUTE = 2,
		SKILL = 3,
		KNOWLEDGE = 4,
		ITEM = 5,
	}
	
	public struct DamageDef{
		public string Name;
		public short ID;
		public DamageType DamageType;
		public DamageBasis DamageBasis;
	}

    public enum DamageType
    {
        NONE = 0,
		SLASHING = 1,
		CRUSHING = 2,
		PIERCING = 3,
		ICE = 4,
		FIRE = 5,
		EARTH = 6,
		WIND = 7,
		WATER = 8,
		LIGHT = 9,
		DARK = 10,
		CHAOS = 11,
		ORDER = 12,
		POISON = 13,
		DISEASE = 14,
		ARCANE  = 15,
		MENTAL = 16,
		SONIC = 17
    }

	static class DamageUtils{



		public static int calculateDamage(){
			return -1;
		}

	}
}
