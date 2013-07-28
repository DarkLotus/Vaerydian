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

	public struct Faction{
		public string Name;
		public int Value;
		public FactionType FactionType;
	}

}
