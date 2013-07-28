using System;
using System.Collections.Generic;
using System.Text;

namespace Vaerydian.Characters
{

	public enum KnowledgeType{
		General,
		Variation,
		Unique
	}

	public struct Knowledge{
		public string Name;
		public float Value;
		public KnowledgeType KnowledgeType;
	}

}
