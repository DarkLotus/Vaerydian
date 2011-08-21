using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Characters.Skills
{

    public enum SkillType
    {
        Defensive,
        Offensive,
        Crafting,
        Social,
        Enviromental
    }

    public class Skill
    {
        public Skill() { }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">name fo the skill</param>
        /// <param name="value">value of the skill</param>
        /// <param name="skillType">type of skill</param>
        public Skill(String name, int value, SkillType skillType)
        {
            s_Name = name;
            s_Value = value;
            s_SkillType = skillType;
        }

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

    }
}
