using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Characters;

namespace Vaerydian.Components.Characters
{
    public class Knowledges : IComponent
    {
        private static int e_TypeID;
        private int e_EntityID;

        public Knowledges() { }

        public int getEntityId()
        {
            return e_EntityID;
        }

        public int getTypeId()
        {
            return e_TypeID;
        }

        public void setEntityId(int entityId)
        {
            e_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            e_TypeID = typeId;
        }

		/// <summary>
        /// experience fighting a mob group type
        /// </summary>
		public Dictionary<string, Knowledge> GeneralKnowledge = new Dictionary<string, Knowledge>();
        /// <summary>
        /// experience fighting a variation of mob
        /// </summary>
		public Dictionary<string, Knowledge> VariationKnowledge = new Dictionary<string, Knowledge>();
        /// <summary>
        /// experience fighting something or somewhere unique
        /// </summary>
		public Dictionary<string, Knowledge> UniqueKnowledge = new Dictionary<string, Knowledge>();
        /// <summary>
        /// experience fighting a given mob
        /// </summary>
		public Dictionary<string, Knowledge> BattleKnowledge = new Dictionary<string, Knowledge>();
    }
}
