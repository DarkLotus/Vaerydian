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
        CombatExit,
        PlayerTurn,
        PlayerActing,
        EnemyTurn,
        EnemyActing,
        NPCTurn,
        NPCActing,
        Dialog
    }

    public class CombatEngine
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
        /// random number generator for classs
        /// </summary>
        private Random random = new Random();

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
        /// terrain for this combat event
        /// </summary>
        private Terrain[,] ce_Terrain = new Terrain[3, 3];
        /// <summary>
        /// terrain for this combat event
        /// </summary>
        public Terrain[,] Terrain
        {
            get { return ce_Terrain; }
            set { ce_Terrain = value; }
        }

        /// <summary>
        /// enemies in this combat event
        /// </summary>
        private EnemyCharacter[] ce_Enemies;
        /// <summary>
        /// enemies in this combat event
        /// </summary>
        public EnemyCharacter[] Enemies
        {
            get { return ce_Enemies; }
            set { ce_Enemies = value; }
        }

        /// <summary>
        /// player character reference
        /// </summary>
        private PlayerCharacter ce_Player;

        /// <summary>
        /// create a new combat event and initialize for combat to begin
        /// </summary>
        /// <param name="terrain">3x3 Terrain that combat will take place on</param>
        /// <param name="player">Current Player Character</param>
        /// <param name="enemies">Enemies player will be fighting</param>
        public void newCombatEvent(Terrain[,] terrain, PlayerCharacter player, EnemyCharacter[] enemies)
        {
            //do combat setup here
            //
            ce_Terrain = terrain;

            //set current player reference
            ce_Player = player;

            //setup enemy array
            ce_Enemies = enemies;

            //set state to ready
            ce_CombatState = CombatState.CombatReady;
        }

        /// <summary>
        /// determines the turn initiative 
        /// </summary>
        public void determineInitiative()
        {
            //creating combat list
            
        }

        /// <summary>
        /// checks the combat turn queue and sets the state appropriately
        /// </summary>
        public void updateTurnState()
        {

        }

        /// <summary>
        /// updates the turn queue for a new round
        /// </summary>
        public void updateTurnQueueForRound()
        {
        }

    }
}
