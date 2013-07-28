using System;
using System.Collections.Generic;
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

	public struct Statistic{
		public string Name;
		public int Value;
		public StatType StatType;
	}
}
