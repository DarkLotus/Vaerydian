using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Characters.Experience;

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

        private Dictionary<CreatureGeneralGroup, Knowledge> e_GeneralKnowledge = new Dictionary<CreatureGeneralGroup, Knowledge>();
        /// <summary>
        /// experience fighting a mob group type
        /// </summary>
        public Dictionary<CreatureGeneralGroup, Knowledge> GeneralKnowledge
        {
            get { return e_GeneralKnowledge; }
            set { e_GeneralKnowledge = value; }
        }

        private Dictionary<CreatureVariationGroup, Knowledge> e_VariationKnowledge = new Dictionary<CreatureVariationGroup, Knowledge>();

        /// <summary>
        /// experience fighting a variation of mob
        /// </summary>
        public Dictionary<CreatureVariationGroup, Knowledge> VariationKnowledge
        {
            get { return e_VariationKnowledge; }
            set { e_VariationKnowledge = value; }
        }

        private Dictionary<CreatureUniqueGroup, Knowledge> e_UniqueKnowledge = new Dictionary<CreatureUniqueGroup, Knowledge>();

        /// <summary>
        /// experience fighting something or somewhere unique
        /// </summary>
        public Dictionary<CreatureUniqueGroup, Knowledge> UniqueKnowledge
        {
            get { return e_UniqueKnowledge; }
            set { e_UniqueKnowledge = value; }
        }

        private Dictionary<Entity, Knowledge> e_BattleKnowledge = new Dictionary<Entity, Knowledge>();

        /// <summary>
        /// experience fighting a given mob
        /// </summary>
        public Dictionary<Entity, Knowledge> BattleKnowledge
        {
            get { return e_BattleKnowledge; }
            set { e_BattleKnowledge = value; }
        }
    }
}
