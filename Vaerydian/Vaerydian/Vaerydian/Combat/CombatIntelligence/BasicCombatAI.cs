using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaerydian.Characters;
using Microsoft.Xna.Framework;
using System.Threading;

namespace Vaerydian.Combat.CombatIntelligence
{
    public enum BasicCombatPlan
    {
        Attack,
        Move
    }
    
    public class BasicCombatAI : CombatAI
    {
        public BasicCombatAI(Character thisCharacter)
        {
            bcai_ThisCharacter = thisCharacter;
        }

        #region variables

        /// <summary>
        /// the owner of this AI
        /// </summary>
        private Character bcai_ThisCharacter;

        /// <summary>
        /// current chosen hostile target
        /// </summary>
        private Character bcai_HostleTarget;

        /// <summary>
        /// enumerator holding current combat state
        /// </summary>
        private BasicCombatPlan bcai_BasicCombatPlan;

        /// <summary>
        /// target of AI
        /// </summary>
        private Vector2 targetCell = Vector2.Zero;

        #endregion


        #region planning

        public override void planAction(Character character)
        {
            base.planAction(character);

            //determine most hostile target
            if (isHostileFound())
            {

                //figure out if character is close enough to attack
                if (isHostileAttackable(bcai_HostleTarget))
                {
                    bcai_BasicCombatPlan = BasicCombatPlan.Attack;
                }
                else
                {
                    bcai_BasicCombatPlan = BasicCombatPlan.Move;
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
            if (cai_HostileList.Count > 0)
            {
                bcai_HostleTarget = getMostHostile(cai_HostileList);
            }
            else
            {
                foreach (Character character in CombatEngine.Instance.TurnList)
                {
                    //don't add yourself
                    if (character == bcai_ThisCharacter)
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
            if (Vector2.Distance(bcai_ThisCharacter.BattlePosition, character.BattlePosition) <= bcai_ThisCharacter.Equipment.Weapon.RangeMin)
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

        #region performing

        /// <summary>
        /// performs the chosen action for the AI
        /// </summary>
        /// <param name="character"></param>
        public override void performAction(Character character)
        {
            base.performAction(character);

            if (bcai_BasicCombatPlan == BasicCombatPlan.Attack)
            {
                //attack your target
                CombatEngine.Instance.attackTarget(character, CombatEngine.Instance.getTarget(targetCell));
            }
            else if (bcai_BasicCombatPlan == BasicCombatPlan.Move)
            {
                
            }
        }

        #endregion
    }
}
