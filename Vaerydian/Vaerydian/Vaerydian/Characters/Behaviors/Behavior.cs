using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Vaerydian.Combat;

namespace Vaerydian.Characters.Behaviors
{
    public enum BasicCombatPlan
    {
        Attack,
        Move
    }

    public class Behavior
    {

        public Behavior(Character character) 
        {
            b_ThisCharacter = character;
        }

        #region variables / parameters

        /// <summary>
        /// list of all hostile characters against this character
        /// </summary>
        protected List<Character> b_HostileList = new List<Character>();
        /// <summary>
        /// list of all hostile characters against this character
        /// </summary>
        public List<Character> HostileList
        {
            get { return b_HostileList; }
            set { b_HostileList = value; }
        }

        /// <summary>
        /// the owner of this AI
        /// </summary>
        private Character b_ThisCharacter;

        /// <summary>
        /// current chosen hostile target
        /// </summary>
        private Character b_HostleTarget;

        /// <summary>
        /// enumerator holding current combat state
        /// </summary>
        private BasicCombatPlan b_BasicCombatPlan;

        /// <summary>
        /// target of AI
        /// </summary>
        private Vector2 targetCell = Vector2.Zero;

        #endregion

        #region Combat Planning

        public void planAction(Character character)
        {

            //determine most hostile target
            if (isHostileFound())
            {

                //figure out if character is close enough to attack
                if (isHostileAttackable(b_HostleTarget))
                {
                    b_BasicCombatPlan = BasicCombatPlan.Attack;
                }
                else
                {
                    b_BasicCombatPlan = BasicCombatPlan.Move;
                }
            }
        }

        /// <summary>
        /// find a hostile target
        /// </summary>
        /// <returns></returns>
        private bool isHostileFound()
        {
            //are there any current active hostile targets
            if (b_HostileList.Count > 0)
            {
                b_HostleTarget = getMostHostile(b_HostileList);
            }
            else
            {
                foreach (Character character in CombatEngine.Instance.TurnList)
                {
                    //don't add yourself
                    if (character == b_ThisCharacter)
                        continue;

                    //find weakest attackable character

                }
            }


            return true;
        }

        private Character getMostHostile(List<Character> characterList)
        {



            return characterList[0];
        }

        /// <summary>
        /// figure out if the player is within range to be attacked
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        private bool isHostileAttackable(Character character)
        {
            targetCell = Vector2.Zero;

            if (character == null)
                return false;

            // distance should be less than or equal to the weapon range
            if (Vector2.Distance(b_ThisCharacter.BattlePosition, character.BattlePosition) <= b_ThisCharacter.Equipment.Weapon.Range)
            {
                targetCell = character.BattlePosition;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Combat Performing

        /// <summary>
        /// performs the chosen action for the AI
        /// </summary>
        /// <param name="character"></param>
        public void performAction(Character character)
        {

            if (b_BasicCombatPlan == BasicCombatPlan.Attack)
            {
                //attack your target
                CombatEngine.Instance.attackTarget(character, CombatEngine.Instance.getTarget(targetCell));
            }
            else if (b_BasicCombatPlan == BasicCombatPlan.Move)
            {

            }
        }

        #endregion







    }
}
