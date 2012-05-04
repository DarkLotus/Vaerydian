using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Characters.Experience;

namespace Vaerydian.Components.Characters
{
    public class Experiences : IComponent
    {
        private static int e_TypeID;
        private int e_EntityID;

        public Experiences() { }

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

        private Dictionary<MobGroup, Experience> e_GeneralExperience = new Dictionary<MobGroup, Experience>();
        /// <summary>
        /// experience fighting a mob group type
        /// </summary>
        public Dictionary<MobGroup, Experience> GeneralExperience
        {
            get { return e_GeneralExperience; }
            set { e_GeneralExperience = value; }
        }

        private Dictionary<MobVariation, Experience> e_TypeExperience = new Dictionary<MobVariation, Experience>();

        /// <summary>
        /// experience fighting a variation of mob
        /// </summary>
        public Dictionary<MobVariation, Experience> TypeExperience
        {
            get { return e_TypeExperience; }
            set { e_TypeExperience = value; }
        }

        private Dictionary<UniqueType, Experience> e_UniqueExperience = new Dictionary<UniqueType, Experience>();

        /// <summary>
        /// experience fighting something or somewhere unique
        /// </summary>
        public Dictionary<UniqueType, Experience> UniqueExperience
        {
            get { return e_UniqueExperience; }
            set { e_UniqueExperience = value; }
        }

        private Dictionary<Entity, Experience> e_BattleExperience = new Dictionary<Entity, Experience>();

        /// <summary>
        /// experience fighting a given mob
        /// </summary>
        public Dictionary<Entity, Experience> BattleExperience
        {
            get { return e_BattleExperience; }
            set { e_BattleExperience = value; }
        }
    }
}
