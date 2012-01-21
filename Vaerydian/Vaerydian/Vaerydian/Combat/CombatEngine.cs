using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WorldGeneration.World;
using Vaerydian.Characters;
using Vaerydian.Items;
using Vaerydian.Characters.Skills;
using Vaerydian.Characters.Abilities;
using BehaviorLibrary;
using Vaerydian.Characters.Behaviors;


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
        CombatLost,
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
        //private Terrain[,] ce_Terrain = new Terrain[3, 3];
        /// <summary>
        /// terrain for this combat event
        /// </summary>
        /*public Terrain[,] Terrain
        {
            get { return ce_Terrain; }
            set { ce_Terrain = value; }
        }*/

        /// <summary>
        /// enemies in this combat event
        /// </summary>
        private List<Character> ce_Combatants = new List<Character>();
        /// <summary>
        /// enemies in this combat event
        /// </summary>
        public List<Character> Combatants
        {
            get { return ce_Combatants; }
            set { ce_Combatants = value; }
        }

        private List<Character> ce_DefeatedCombatants = new List<Character>();

        /// <summary>
        /// player character reference
        /// </summary>
        private Character ce_Player;
        /// <summary>
        /// player character reference
        /// </summary>
        public Character Player
        {
            get { return ce_Player; }
            set { ce_Player = value; }
        }

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

        /// <summary>
        /// text updated from battle actions
        /// </summary>
        private StringBuilder ce_BattleText = new StringBuilder();
        /// <summary>
        /// text updated from battle actions
        /// </summary>
        public StringBuilder BattleText
        {
            get { return ce_BattleText; }
            set { ce_BattleText = value; }
        }

        /// <summary>
        /// lets the combat screen know there is assessment dialog to post
        /// </summary>
        private bool ce_AssementDialog = false;
        /// <summary>
        /// lets the combat screen know there is assessment dialog to post
        /// </summary>
        public bool AssementDialog
        {
            get { return ce_AssementDialog; }
            set { ce_AssementDialog = value; }
        }

        /// <summary>
        /// dimensions of the combat field
        /// </summary>
        private int ce_Dimensions;
        /// <summary>
        /// dimensions of the combat field
        /// </summary>
        public int Dimensions
        {
            get { return ce_Dimensions; }
            set { ce_Dimensions = value; }
        }

        #endregion

        /// <summary>
        /// create a new combat event and initialize for combat to begin
        /// </summary>
        /// <param name="terrain">3x3 Terrain that combat will take place on</param>
        /// <param name="player">Current Player Character</param>
        /// <param name="combatants">Enemies player will be fighting</param>
        public void newCombatEvent(Terrain[,] terrain, int dimensions, Character player, List<Character> combatants)
        {
            //clear old info
            resetValues();

            //do combat setup here
            //ce_Terrain = terrain;

            //combat terrain dimensions
            ce_Dimensions = dimensions;

            //set current player reference
            ce_Player = player;

            //setup enemy array
            ce_Combatants = combatants;

            //set state to ready
            ce_CurrentCombatState = CombatState.CombatReady;
        }

        /// <summary>
        /// determines the turn initiative 
        /// </summary>
        public void determineInitiative()
        {
            //creating combat initiative list
            int[] vals = new int[ce_Combatants.Count];
            int maxVal = 0;
            int index = 0;
            int count = 0;

            //Initiative is calculated by a characters Quickness, Perception, and Agility + Random number from 1-100
              
            //get character values
            for (int i = 0; i < ce_Combatants.Count; i++)
            {
                vals[i] = ce_Combatants[i].Stats["Agility"].Value / 4 + ce_Combatants[i].Stats["Quickness"].Value / 3 + ce_Combatants[i].Stats["Perception"].Value / 5 + ce_Combatants[i].Equipment.Weapon.Speed;
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
                ce_TurnList.Add(ce_Combatants[index]);

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
            if (ce_TurnList[ce_TurnIndex].CharacterType == CharacterType.NPC)
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
            if (ce_TurnList[ce_TurnIndex].CharacterType == CharacterType.NPC)
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
            //ce_TurnList[ce_TurnIndex].OldBehavior.planAction(ce_TurnList[ce_TurnIndex]);

            //set the NPC to act
            ce_CurrentCombatState = CombatState.NpcActing;
            
        }

        /// <summary>
        /// performs the NPC's chosen action
        /// </summary>
        public bool npcPerformAction()
        {
            //ce_TurnList[ce_TurnIndex].OldBehavior.performAction(ce_TurnList[ce_TurnIndex]);

            if (ce_TurnList[ce_TurnIndex].Behavior.Behave() == BehaviorReturnCode.Success &&
                ce_TurnList[ce_TurnIndex].Behavior.CombatState == SimpleCombatState.Acted)
            {
                ce_TurnList[ce_TurnIndex].Behavior.CombatState = SimpleCombatState.Thinking;

                //NPC turn is complete
                ce_CurrentCombatState = CombatState.CombatAssessTurn;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// assesses this turn to see if NPCs or the Player died or if other things occured
        /// </summary>
        public void assessCombatTurn()
        {
            //clear battle text
            ce_BattleText.Clear();

            //see if an enemy has been killed
            foreach (Character combatant in ce_Combatants)
            {
                if (combatant.Health <= 0)
                {
                    ce_TurnList.Remove(combatant);
                    ce_DefeatedCombatants.Add(combatant);

                    ce_BattleText.Append(combatant.Name + " has been killed!");

                    if (combatant.CharacterType == CharacterType.Player)
                    {
                        ce_IsPlayerDead = true;
                        ce_BattleText.Append(" You have died!");
                    }
                    
                    ce_AssementDialog = true;
                }
            }

            //remove the character from the combatants list
            foreach (Character defeated in ce_DefeatedCombatants)
            {
                if (ce_Combatants.Contains(defeated))
                    ce_Combatants.Remove(defeated);
            }
        }

        /// <summary>
        /// is it possible to move in the chosen direction
        /// </summary>
        /// <returns>true if possible, false if not</returns>
        public bool isDirectionMovable(Vector2 target)
        {
            //is the move legal
            if (target.X >= 0 &&
                target.X < ce_Dimensions &&
                target.Y >= 0 &&
                target.Y < ce_Dimensions)
            {
                //is the square occupied
                foreach (Character combatant in ce_Combatants)
                {
                    if (combatant.BattlePosition == target)
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
                target.X < ce_Dimensions &&
                target.Y >= 0 &&
                target.Y < ce_Dimensions)
            {
                //check if an enemy is there
                foreach (Character combatant in ce_Combatants)
                {   //attackable
                    if (combatant.BattlePosition == target)
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

        /// <summary>
        /// gets the character at the given location
        /// </summary>
        /// <param name="targetLocation">location to check</param>
        /// <returns>a character if found or null if not</returns>
        public Character getTarget(Vector2 targetLocation)
        {
            foreach (Character combatant in ce_Combatants)
            {
                if (combatant.BattlePosition == targetLocation)
                    return combatant;
            }
            //no character found
            return null;
        }

        /// <summary>
        /// have the attacker attack the target
        /// </summary>
        /// <param name="attacker">character performing the attack</param>
        /// <param name="target">character being attacked</param>
        public void attackTarget(Character attacker, Character target)
        {
            //clear the battle text
            ce_BattleText.Clear();
            
            //figure out attackers hit value
            double probHitAttacker = attacker.Stats["Agility"].Value / 4.0 + getWeaponSkillValue(attacker) / 2.0 + //base attack skill
                (attacker.Stats["Quickness"].Value / 5.0) * attacker.Equipment.Weapon.Speed + //equipment attack enhancements
                getAttackBonuses(attacker); //special attack bonuses

            //figure out target's avoidance
            double probDefendTarget = target.Stats["Agility"].Value / 4.0 + target.Skills["Dodge"].Value / 2.0 + //base avoidance skill
                (target.Stats["Quickness"].Value / 5.0) * target.Equipment.ArmorChest.Mobility + //equipment avoidance enhancements
                getAvoidanceBonuses(target); //special avoidance bonuses
            
            //define thresholds
            double lowPercent = 0.15;
            double highPercent = 1.75;

            //find hit probability
            double hitProbability = highPercent * (probHitAttacker / (probHitAttacker + probDefendTarget)) + //probability of attacker to hit
                                    lowPercent * (probDefendTarget / (probHitAttacker + probDefendTarget)); //probability of target to avoid
            
            //Roll hit probability
            double hitAttempt = random.NextDouble();// *hitProbability;

            if (hitAttempt <= hitProbability)
            {
                double overHit = 0.0;
                
                //check for overhit bonus
                if (hitProbability > 1.0)
                {
                    overHit = hitProbability - 1.0;
                }

                //hit determine dmg
                int damageMax = (int)((overHit + 1.0) * (((float)attacker.Stats["Strength"].Value / 4.0f) + ((float)getWeaponSkillValue(attacker) / 5.0f) +
                                              ((float)attacker.Equipment.Weapon.Lethality - (float)target.Equipment.ArmorChest.Defense)));

                int damage = random.Next(damageMax / 2, damageMax);

                //make sure you're doing 0 or greater dmg
                if (damage < 0)
                   damage = 0;

                target.Health -= damage;

                ce_BattleText.Append(attacker.Name + " hits " + target.Name + " for " + damage + " " + attacker.Equipment.Weapon.DamageType.ToString() + " damage!");

                if (target.CharacterType != CharacterType.Player)
                {
                    //target.OldBehavior.HostileList.Add(attacker);
                    target.Behavior.HostileList.Add(attacker);
                }
            }
            else
            {
                //did not hit
                ce_BattleText.Append(attacker.Name + " misses " + target.Name + " as they avoid the attack!");
            }
        }

        private int getWeaponSkillValue(Character character)
        {
            return character.Skills["WeaponSkill"].Value;
        }

        private int getAttackBonuses(Character character)
        {
            return 0;
        }

        private int getAvoidanceBonuses(Character cahracter)
        {
            return 0;
        }

    }
}
