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

        private Dictionary<MobGroup, Knowledge> e_GeneralKnowledge = new Dictionary<MobGroup, Knowledge>();
        /// <summary>
        /// experience fighting a mob group type
        /// </summary>
        public Dictionary<MobGroup, Knowledge> GeneralKnowledge
        {
            get { return e_GeneralKnowledge; }
            set { e_GeneralKnowledge = value; }
        }

        private Dictionary<MobVariation, Knowledge> e_TypeKnowledge = new Dictionary<MobVariation, Knowledge>();

        /// <summary>
        /// experience fighting a variation of mob
        /// </summary>
        public Dictionary<MobVariation, Knowledge> TypeKnowledge
        {
            get { return e_TypeKnowledge; }
            set { e_TypeKnowledge = value; }
        }

        private Dictionary<UniqueType, Knowledge> e_UniqueKnowledge = new Dictionary<UniqueType, Knowledge>();

        /// <summary>
        /// experience fighting something or somewhere unique
        /// </summary>
        public Dictionary<UniqueType, Knowledge> UniqueKnowledge
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
