using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Characters
{

    public enum SkillType
    {
        Defensive,
        Offensive,
        Crafting,
        Social,
        Enviromental
    }

	public enum SkillName
	{
		NONE = 0,
		RANGED = 1,
		MELEE = 2,
		AVOIDANCE = 3
	}

	public struct Skill{
		public string Name;
		public int Value;
		public SkillType SkillType;
	}
}
