using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Vaerydian.Characters.Skills;
using Vaerydian.Characters.Abilities;
using Vaerydian.Characters.Stats;
using Vaerydian.Characters.BehaviorIntelligence;
using Vaerydian.Combat.CombatIntelligence;
using Vaerydian.Items;


namespace Vaerydian.Characters
{
    public enum Alignment
    {
        Good,
        Neutral,
        Evil
    }

    public enum Personality
    {
        Extrovert,
        Introvert
    }

    public enum CharacterType
    {
        Player,
        NPC
    }


    public class Character
    {
        /// <summary>
        /// type of character
        /// </summary>
        private CharacterType c_CharacterType;

        /// <summary>
        /// type of character
        /// </summary>
        public CharacterType CharacterType
        {
            get { return c_CharacterType; }
            set { c_CharacterType = value; }
        }

        /// <summary>
        /// the artificial intelligence behavior for the character
        /// </summary>
        private BehaviorAI c_BehaviorAI;
        /// <summary>
        /// the artificial intelligence behavior for the character
        /// </summary>
        public BehaviorAI BehaviorAI
        {
            get { return c_BehaviorAI; }
            set { c_BehaviorAI = value; }
        }

        /// <summary>
        /// the artificial intelligence for combat for the character
        /// </summary>
        private CombatAI c_CombatAI;

        /// <summary>
        /// the artificial intelligence for combat for the character
        /// </summary>
        public CombatAI CombatAI
        {
            get { return c_CombatAI; }
            set { c_CombatAI = value; }
        }
        
        /// <summary>
        /// name of character
        /// </summary>
        private String c_Name;
        /// <summary>
        /// name of character
        /// </summary>
        public String Name
        {
            get { return c_Name; }
            set { c_Name = value; }
        }

        /// <summary>
        /// health of character
        /// </summary>
        private int c_Health;
        /// <summary>
        /// health of character
        /// </summary>
        public int Health
        {
            get { return c_Health; }
            set { c_Health = value; }
        }

        /// <summary>
        /// characters current stats
        /// </summary>
        private Dictionary<String, Stat> c_Stats = new Dictionary<String, Stat>();

        /// <summary>
        /// characters current stats
        /// </summary>
        public Dictionary<String, Stat> Stats
        {
            get { return c_Stats; }
            set { c_Stats = value; }
        }

        /// <summary>
        /// Alighnment of character
        /// </summary>
        private Alignment c_Alignment;
        /// <summary>
        /// Alighnment of character
        /// </summary>
        public Alignment Alignment
        {
            get { return c_Alignment; }
            set { c_Alignment = value; }
        }

        /// <summary>
        /// Personality of characer
        /// </summary>
        private Personality c_Personality;
        /// <summary>
        /// Personality of characer
        /// </summary>
        public Personality Personality
        {
            get { return c_Personality; }
            set { c_Personality = value; }
        }

        /// <summary>
        /// Character's available skills
        /// </summary>
        private Dictionary<String, Skill> c_Skills = new Dictionary<String, Skill>();
        /// <summary>
        /// Character's available skills
        /// </summary>
        public Dictionary<String, Skill> Skills
        {
            get { return c_Skills; }
            set { c_Skills = value; }
        }

        /// <summary>
        /// Characer's available Abilities
        /// </summary>
        private Dictionary<String, Ability> c_Abilities = new Dictionary<String, Ability>();

        internal Dictionary<String, Ability> Abilities
        {
            get { return c_Abilities; }
            set { c_Abilities = value; }
        }
        
        /// <summary>
        /// Character's current battle position
        /// </summary>
        private Vector2 c_BattlePosition;

        /// <summary>
        /// Character's current battle position
        /// </summary>
        public Vector2 BattlePosition
        {
            get { return c_BattlePosition; }
            set { c_BattlePosition = value; }
        }

        /// <summary>
        /// Character's current world position
        /// </summary>
        private Vector2 c_WorldPosition;

        /// <summary>
        /// Character's current world position
        /// </summary>
        public Vector2 WorldPosition
        {
            get { return c_WorldPosition; }
            set { c_WorldPosition = value; }
        }

        /// <summary>
        /// characer's Inventory
        /// </summary>
        private Dictionary<String, Item> c_Inventory = new Dictionary<string, Item>();

        /// <summary>
        /// characer's Inventory
        /// </summary>
        public Dictionary<String, Item> Inventory
        {
            get { return c_Inventory; }
            set { c_Inventory = value; }
        }

        /// <summary>
        /// characer's equipment
        /// </summary>
        private Equipment c_Equipment;

        public Equipment Equipment
        {
            get { return c_Equipment; }
            set { c_Equipment = value; }
        }




    }
}
