using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldGeneration.Terrain;
using Vaerydian.Characters;

namespace Vaerydian.Combat
{

    public enum CombatState
    {
        CombatInitializing,
        CombatReady,
        CombatFinished,
        PlayerTurn,
        PlayerActing,
        EnemyTurn,
        EnemyActing,
        NPCTurn,
        NPCActing,
        Dialog
    }

    class CombatEngine
    {

        /// <summary>
        /// singleton private class
        /// </summary>
        private CombatEngine() { }

        /// <summary>
        /// isntance variable
        /// </summary>
        private static readonly CombatEngine me_Instance = new CombatEngine();
        /// <summary>
        /// singleton instance access
        /// </summary>
        public static CombatEngine Instance { get { return me_Instance; } }

        /// <summary>
        /// current state of the combat engine
        /// </summary>
        private CombatState ce_CombatState = CombatState.CombatInitializing;

        /// <summary>
        /// current state of the combat engine
        /// </summary>
        public CombatState CombatState
        {
            get { return ce_CombatState; }
            set { ce_CombatState = value; }
        }

        /// <summary>
        /// create a new combat event and initialize for combat to begin
        /// </summary>
        /// <param name="terrain">3x3 Terrain that combat will take place on</param>
        /// <param name="player">Current Player Character</param>
        /// <param name="enemies">Enemies player will be fighting</param>
        public void newCombatEvent(Terrain[][] terrain, PlayerCharacter player, EnemyCharacter[] enemies)
        {
        }


    }
}
