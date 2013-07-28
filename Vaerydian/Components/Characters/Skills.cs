using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Characters;
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

        /// <summary>
        /// available skills
        /// </summary>
		public Dictionary<SkillName, Skill> SkillSet;
    }
}
