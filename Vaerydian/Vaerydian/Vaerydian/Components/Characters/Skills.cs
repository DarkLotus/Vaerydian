using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Characters.Skills;
using Vaerydian.Utils;

namespace Vaerydian.Components.Characters
{
    class Skills : IComponent
    {
        private static int s_TypeID;
        private int s_EntityID;

        public Skills() { }

        public int getEntityId()
        {
            return s_EntityID;
        }

        public int getTypeId()
        {
            return s_TypeID;
        }

        public void setEntityId(int entityId)
        {
            s_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            s_TypeID = typeId;
        }

        private Dictionary<SkillNames, Skill> s_SkillSet = new Dictionary<SkillNames, Skill>();
        /// <summary>
        /// available skills
        /// </summary>
        public Dictionary<SkillNames, Skill> SkillSet
        {
            get { return s_SkillSet; }
            set { s_SkillSet = value; }
        }
    }
}
