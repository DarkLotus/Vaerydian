using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldGeneration.Terrain;
using Vaerydian.Characters;
using Microsoft.Xna.Framework;
//using Vaerydian.Combat;

namespace Vaerydian.Combat
{

    #region enums
    
    /// <summary>
    /// represents the current state of the Combat Engine
    /// </summary>
    public enum CombatState
    {
        CombatInitializing,
        CombatReady,
        CombatFinished,
        CombatExit,
        CombatAssessTurn,
        PlayerChooseAction,
        PlayerActing,
        NpcChooseAction,
        NpcActing,
        Dialog
    }
    
    #endregion

    /// <summary>
    /// engine that controls all combat behavior
    /// </summary>
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

        #region Variables

        /// <summary>
        /// random number generator for classs
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// current state of the combat engine
        /// </summary>
        private CombatState ce_CurrentCombatState = CombatState.CombatInitializing;

        /// <summary>
        /// current state of the combat engine
        /// </summary>
        public CombatState CurrentCombatState
        {
            get { return ce_CurrentCombatState; }
            set { ce_CurrentCombatState = value; }
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
        /// each character's turn
        /// </summary>
        private List<Character> ce_TurnList = new List<Character>();

        public List<Character> TurnList
        {
            get { return ce_TurnList; }
            set { ce_TurnList = value; }
        }

        /// <summary>
        /// index of current TurnList position
        /// </summary>
        private int ce_TurnIndex = 0;
        /// <summary>
        /// index of current TurnList position
        /// </summary>
        public int TurnIndex
        {
            get { return ce_TurnIndex; }
            set { ce_TurnIndex = value; }
        }

        /// <summary>
        /// current character
        /// </summary>
        private Character ce_CurrentCharacter;

        /// <summary>
        /// is the player now considered dead
        /// </summary>
        private bool ce_IsPlayerDead = false;

        /// <summary>
        /// is the player now considered dead
        /// </summary>
        public bool IsPlayerDead
        {
            get { return ce_IsPlayerDead; }
            set { ce_IsPlayerDead = value; }
        }

        /// <summary>
        /// current round
        /// </summary>
        private int ce_RoundCounter = 1;

        /// <summary>
        /// current round
        /// </summary>
        public int RoundCounter
        {
            get { return ce_RoundCounter; }
            set { ce_RoundCounter = value; }
        }




        #endregion

        /// <summary>
        /// create a new combat event and initialize for combat to begin
        /// </summary>
        /// <param name="terrain">3x3 Terrain that combat will take place on</param>
        /// <param name="player">Current Player Character</param>
        /// <param name="enemies">Enemies player will be fighting</param>
        public void newCombatEvent(Terrain[,] terrain, PlayerCharacter player, EnemyCharacter[] enemies)
        {
            //clear old info
            resetValues();

            //do combat setup here
            //
            ce_Terrain = terrain;

            //set current player reference
            ce_Player = player;

            //setup enemy array
            ce_Enemies = enemies;

            //set state to ready
            ce_CurrentCombatState = CombatState.CombatReady;
        }

        /// <summary>
        /// determines the turn initiative 
        /// </summary>
        public void determineInitiative()
        {
            //creating combat initiative list
            int[] vals = new int[1 + ce_Enemies.Length];
            int maxVal = 0;
            int index = 0;
            int count = 0;

            //Initiative is calculated by a players Quickness, Perception, and Agility + Random number from 1-100
            //first val is ALWAYS the player's initiative
            vals[0] = ce_Player.Agility + ce_Player.Quickness + ce_Player.Perception + random.Next(1, 100);
            
            //get enemies values
            for (int i = 0; i < ce_Enemies.Length; i++)
            {
                vals[i + 1] = ce_Enemies[i].Agility + ce_Enemies[i].Quickness + ce_Enemies[i].Perception + random.Next(1, 100);
            }

            //Next, figure out which which should go next and place them in the Character Turn List
            while (count < vals.Length)
            {
                //loop through all the values
                for (int i = 0; i < vals.Length; i++)
                {
                    //check to see if this is the largest so far
                    if (vals[i] > maxVal)
                    {
                        //its the largest so far, so capture it and its index
                        maxVal = vals[i];
                        index = i;
                    }
                }

                //based on the index of the largest, add it to the turn list
                if (index != 0)//enemies are added to the array, so subtract one from their index
                    ce_TurnList.Add(ce_Enemies[index-1]);
                else
                    ce_TurnList.Add(ce_Player);

                //set this val to 0 so its not counted again
                vals[index] = 0;
                //reset maxVal for next round
                maxVal = 0;
                //update the counter
                count++;
            }

            //ensure it is set to 0
            ce_TurnIndex = 0;

            //make the determination
            if (ce_TurnList[ce_TurnIndex].GetType() == typeof(EnemyCharacter))
                ce_CurrentCombatState = CombatState.NpcChooseAction;
            else
                ce_CurrentCombatState = CombatState.PlayerChooseAction;
        }

        /// <summary>
        /// resets values that should not be carried over between combat events
        /// </summary>
        public void resetValues()
        {
            ce_TurnList.Clear();
            ce_TurnIndex = 0;
            ce_IsPlayerDead = false;
            ce_RoundCounter = 1;
        }

        /// <summary>
        /// checks the combat turn queue and sets the state appropriately
        /// </summary>
        public void updateTurnState()
        {
            ce_TurnIndex++;

            //make the determination
            if (ce_TurnList[ce_TurnIndex].GetType() == typeof(EnemyCharacter))
                ce_CurrentCombatState = CombatState.NpcChooseAction;
            else
                ce_CurrentCombatState = CombatState.PlayerChooseAction;
        }

        /// <summary>
        /// updates the turn queue for a new round
        /// </summary>
        public void newRound()
        {
            //clear the turn list and reset the index
            ce_TurnList.Clear();
            ce_TurnIndex = 0;
            ce_RoundCounter++;
            ce_CurrentCombatState = CombatState.CombatReady;
        }

        /// <summary>
        /// access the NPC's Combat AI routines to plan their combat action
        /// </summary>
        public void npcPlanAction()
        {


            //set the NPC to act
            ce_CurrentCombatState = CombatState.NpcActing;
        }

        /// <summary>
        /// performs the NPC's chosen action
        /// </summary>
        public void npcPerformAction()
        {


            //NPC turn is complete
            ce_CurrentCombatState = CombatState.CombatAssessTurn;
        }

        /// <summary>
        /// assesses this turn to see if NPCs or the Player died or if other things occured
        /// </summary>
        public void assessCombatTurn()
        {
            
        }

        /// <summary>
        /// is it possible to move in the chosen direction
        /// </summary>
        /// <returns>true if possible, false if not</returns>
        public bool isDirectionMovable(Vector2 target)
        {
            //is the move legal
            if (target.X >= 0 &&
                target.X < 3 &&
                target.Y >= 0 &&
                target.Y < 3)
            {
                //is the square occupied
                foreach (EnemyCharacter enemy in ce_Enemies)
                {
                    if (enemy.BattlePosition == target)
                        return false;
                }

                //its a good move
                return true;
            }
            else
            {
                //not a good move
                return false;
            }
        }

        /// <summary>
        /// does the cell have a valid target
        /// </summary>
        /// <returns>true if yes, otherwise no</returns>
        public bool isCellAttackable(Vector2 target)
        {
            //does the cell have a valid target
            if (target.X >= 0 &&
                target.X < 3 &&
                target.Y >= 0 &&
                target.Y < 3)
            {
                //check if an enemy is there
                foreach (EnemyCharacter enemy in ce_Enemies)
                {   //attackable
                    if (enemy.BattlePosition == target)
                        return true;
                }

                //nothing to attack
                return false;
            }
            else
            {
                //nothing to attack
                return false;
            }
        }
    }
}
