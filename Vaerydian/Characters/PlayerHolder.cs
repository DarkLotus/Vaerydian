using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vaerydian.Components.Characters;
using Vaerydian.Components.Actions;
using Vaerydian.Components.Items;

namespace Vaerydian.Characters
{
    class PlayerState
    {
        private Information p_Information;

        public Information Information
        {
            get { return p_Information; }
            set { p_Information = value; }
        }

        private Life p_Life;

        public Life Life
        {
            get { return p_Life; }
            set { p_Life = value; }
        }

        private Interactable p_Interactable;

        public Interactable Interactable
        {
            get { return p_Interactable; }
            set { p_Interactable = value; }
        }

        private Knowledges p_Knowledges;

        public Knowledges Knowledges
        {
            get { return p_Knowledges; }
            set { p_Knowledges = value; }
        }

        private Statistics p_Statistics;

        public Statistics Statistics
        {
            get { return p_Statistics; }
            set { p_Statistics = value; }
        }

        private Health p_Health;

        public Health Health
        {
            get { return p_Health; }
            set { p_Health = value; }
        }

        private Skills p_Skills;

        public Skills Skills
        {
            get { return p_Skills; }
            set { p_Skills = value; }
        }

        private Factions p_Factions;

        public Factions Factions
        {
            get { return p_Factions; }
            set { p_Factions = value; }
        }

        private Equipment p_Equipment;

    }
}
