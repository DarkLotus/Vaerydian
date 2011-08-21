using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaerydian.Characters;

namespace Vaerydian.Combat.CombatIntelligence
{
    public abstract class CombatAI
    {
        /// <summary>
        /// list of all hostile characters against this character
        /// </summary>
        protected List<Character> cai_HostileList = new List<Character>();
        /// <summary>
        /// list of all hostile characters against this character
        /// </summary>
        public List<Character> HostileList
        {
            get { return cai_HostileList; }
            set { cai_HostileList = value; }
        }
        


        public virtual void planAction(Character character) { }

        public virtual void performAction(Character character) { }

    }
}
