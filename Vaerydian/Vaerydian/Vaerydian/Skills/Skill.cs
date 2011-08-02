using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Skills
{

    public enum SkillType
    {
        Defensive,
        Offensive,
        Crafting,
        Social,
        Enviromental
    }

    public enum SkillFocus
    {
        Primary,
        Secondary,
        Notfocused
    }

    public abstract class Skill
    {
        /// <summary>
        /// name of the skill
        /// </summary>
        private String s_Name;
        /// <summary>
        /// name of the skill
        /// </summary>
        public String Name
        {
            get { return s_Name; }
            set { s_Name = value; }
        }

        /// <summary>
        /// current value of the skill
        /// </summary>
        private int s_Value;
        /// <summary>
        /// current value of the skill
        /// </summary>
        public int Value
        {
            get { return s_Value; }
            set { s_Value = value; }
        }

        /// <summary>
        /// current skill type
        /// </summary>
        private SkillType s_SkillType;
        /// <summary>
        /// current skill type
        /// </summary>
        public SkillType SkillType
        {
            get { return s_SkillType; }
            set { s_SkillType = value; }
        }

        /// <summary>
        /// the character's focus of this skill
        /// </summary>
        private SkillFocus s_SkillFous;
        /// <summary>
        /// the character's focus of this skill
        /// </summary>
        public SkillFocus SkillFous
        {
            get { return s_SkillFous; }
            set { s_SkillFous = value; }
        }
    }
}
