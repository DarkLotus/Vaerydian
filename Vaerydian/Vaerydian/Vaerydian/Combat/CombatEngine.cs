using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldGeneration.Terrain;
using Vaerydian.Characters;
using Vaerydian.Combat;

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
        /// each character's turn
        /// </summary>
        private List<Character> ce_TurnList = new List<Character>();

        public List<Character> TurnList
        {
            get { return ce_TurnList; }
            set { ce_TurnList = value; }
        }



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

            //make the determination
            if(ce_TurnList[0].GetType() == typeof(EnemyCharacter))
                ce_CombatState = CombatState.EnemyTurn;
            else
                ce_CombatState = CombatState.PlayerTurn;
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
