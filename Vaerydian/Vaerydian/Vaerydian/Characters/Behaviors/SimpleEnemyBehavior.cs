using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BehaviorLibrary;
using BehaviorLibrary.Components.Composites;
using BehaviorLibrary.Components.Actions;
using BehaviorLibrary.Components.Conditionals;
using BehaviorLibrary.Components.Decorators;

using Vaerydian.Combat;
using Vaerydian.Behaviors;

using Microsoft.Xna.Framework;

namespace Vaerydian.Characters.Behaviors
{
    public enum SimpleEnemyState
    {
        Idle,
        Active,
        Combat
    }

    public enum SimpleCombatState
    {
        Thinking,
        Acted
    }

    public class SimpleEnemyBehavior : CharacterBehavior
    {
        private Behavior seb_Behavior;

        private SimpleEnemyState seb_EnemyState;

        public SimpleEnemyState EnemyState
        {
            get { return seb_EnemyState; }
            set { seb_EnemyState = value; }
        }

        private SimpleCombatState seb_CombatState;

        public SimpleCombatState CombatState
        {
            get { return seb_CombatState; }
            set { seb_CombatState = value; }
        }

        private Character seb_ThisCharacter;

        private Character seb_Target;

        private List<Character> seb_HostileList = new List<Character>();

        public List<Character> HostileList
        {
            get { return seb_HostileList; }
            set { seb_HostileList = value; }
        }

        private Vector2 seb_MoveTarget;

        public SimpleEnemyBehavior(Character character)
        {
            seb_ThisCharacter = character;

            seb_Behavior =
                new Behavior(
                    new RootSelector(
                        //determins which branch to take
                        getEnemyState,

                        //Idle State
                        new Selector(
                            new Conditional(() => false)
                            ),

                        //Active State
                        new Selector(
                            new Conditional(() => false)
                            ),                      

                        //Combat State
                        new Selector(

                            //get a target
                            new Sequence(
                                //do i have a target?
                                new Inverter(new Conditional(hasLiveTarget)),
                                //get a target
                                new BehaviorAction(findTarget)
                                ),
                            
                            new Selector(
                                //attempt to move back into range of target
                                new Sequence(
                                    //check to verify you still have a target
                                    new Conditional(hasLiveTarget),
                                    //are you too close to the target
                                    new Conditional(isTargetTooClose),
                                    //not too far
                                    new Inverter(new Conditional(isTargetTooFar)),
                                    //can you move away
                                    new Conditional(canMoveAwayFromTarget),
                                    //move
                                    new BehaviorAction(move)
                                    ),
                                
                                //attempt to move within range of target
                                new Sequence(
                                    //check to verify you still have a target
                                    new Conditional(hasLiveTarget),
                                    //are you too far from the target
                                    new Conditional(isTargetTooFar),
                                    //not too close
                                    new Inverter(new Conditional(isTargetTooClose)),
                                    //can you move towards the target
                                    new Conditional(canMoveTowardsTarget),
                                    //move
                                    new BehaviorAction(move)
                                    )
                                ),

                            //attempt to attack the target
                            new Sequence(
                                //check to verify you still have a target
                                new Conditional(hasLiveTarget),
                                //are you too close to the target
                                new Inverter(new Conditional(isTargetTooClose)),
                                //are you too far from the target
                                new Inverter(new Conditional(isTargetTooFar)),
                                //can you attack the target
                                new Conditional(isTargetAttackable),
                                //attack target
                                new BehaviorAction(attackTarget)
                                ),

                            //go idle until something happens.
                            new Sequence(
                                //assess self
                                new BehaviorAction(idle)
                                )

                            )
                        )
                    );
        }

        /// <summary>
        /// perform the behavior
        /// </summary>
        /// <returns>returns status or failure</returns>
        public override BehaviorReturnCode Behave()
        {
            try
            {
                return seb_Behavior.Behave();
            }
            catch (Exception)
            {
                return BehaviorReturnCode.Failure;
            }
        }

        /// <summary>
        /// returns current enemy state
        /// </summary>
        /// <returns></returns>
        private int getEnemyState()
        {
            return (int)seb_EnemyState;
        }

        /// <summary>
        /// picks a target based on hostility or faction
        /// </summary>
        /// <returns>returns success or failure if a target could be found</returns>
        private BehaviorReturnCode findTarget()
        {
            if (seb_HostileList.Count > 0)
            {
                seb_Target = seb_HostileList[0];
                return BehaviorReturnCode.Success;
            }
            else
            {
                seb_Target = CombatEngine.Instance.Player;

                return BehaviorReturnCode.Success;
            }
        }

        /// <summary>
        /// is character currently too far from target
        /// </summary>
        /// <returns>are farther than your max range?</returns>
        private bool isTargetTooFar()
        {
            if (Vector2.Distance(seb_ThisCharacter.BattlePosition, seb_Target.BattlePosition) > seb_ThisCharacter.Equipment.Weapon.RangeMax)
                return true;
            else
                return false;
        }

        /// <summary>
        /// is character currently too close from target
        /// </summary>
        /// <returns>are farther than your min range?</returns>
        private bool isTargetTooClose()
        {
            if (Vector2.Distance(seb_ThisCharacter.BattlePosition, seb_Target.BattlePosition) < seb_ThisCharacter.Equipment.Weapon.RangeMin)
                return true;
            else
                return false;
        }

        /// <summary>
        /// do you have a live target?
        /// </summary>
        /// <returns></returns>
        private bool hasLiveTarget()
        {
            if (seb_Target == null)
                return false;

            if (seb_Target.Health > 0)
                return true;
            else
            {
                seb_HostileList.Remove(seb_Target);
                return false;
            }
        }

        private bool isTargetAttackable()
        {
            return CombatEngine.Instance.isCellAttackable(seb_Target.BattlePosition);
        }


        private bool canMoveTowardsTarget()
        {
            //Dictionary<float, Vector2> moves = new Dictionary<float, Vector2>();
            List<float> vals = new List<float>();
            List<Vector2> moves = new List<Vector2>();

            //get the costs of the adjacent cells
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if(i == 0 && j == 0)
                        continue;

                    if(CombatEngine.Instance.isDirectionMovable(new Vector2(seb_ThisCharacter.BattlePosition.X + i, seb_ThisCharacter.BattlePosition.Y + j)))
                    {
                        //calculate the distance
                        float dist = Vector2.Distance(seb_Target.BattlePosition, new Vector2(seb_ThisCharacter.BattlePosition.X + i, seb_ThisCharacter.BattlePosition.Y + j));
                        
                        //ensure it doesnt already have the same key
                        moves.Add(new Vector2(i, j));
                        vals.Add(dist);
                        
                    }else
                        continue;
                }
            }

            if(moves.Count == 0)
                return false;

            float min = 100;

            foreach(float dist in vals)
            {
                if(dist < min)
                {
                    min = dist;
                }
            }

            //set target
            seb_MoveTarget = moves[vals.IndexOf(min)];
            return true;
        }

        private bool canMoveAwayFromTarget()
        {
            //Dictionary<float, Vector2> moves = new Dictionary<float, Vector2>();
            List<float> vals = new List<float>();
            List<Vector2> moves = new List<Vector2>();

            //get the costs of the adjacent cells
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    if (CombatEngine.Instance.isDirectionMovable(new Vector2(seb_ThisCharacter.BattlePosition.X + i, seb_ThisCharacter.BattlePosition.Y + j)))
                    {
                        //calculate the distance
                        float dist = Vector2.Distance(seb_Target.BattlePosition, new Vector2(seb_ThisCharacter.BattlePosition.X + i, seb_ThisCharacter.BattlePosition.Y + j));

                        //ensure it doesnt already have the same key
                        moves.Add(new Vector2(i, j));
                        vals.Add(dist);

                    }
                    else
                        continue;
                }
            }

            if (moves.Count == 0)
                return false;

            float max = 0;

            foreach (float dist in vals)
            {
                if (dist > max)
                {
                    max = dist;
                }
            }

            //set target
            seb_MoveTarget = moves[vals.IndexOf(max)];
            return true;
        }

        /// <summary>
        /// move
        /// </summary>
        /// <returns>success or failure</returns>
        private BehaviorReturnCode move()
        {

            try
            {
                seb_ThisCharacter.BattlePosition = new Vector2((seb_ThisCharacter.BattlePosition.X + seb_MoveTarget.X), (seb_ThisCharacter.BattlePosition.Y + seb_MoveTarget.Y));
                
                seb_CombatState = SimpleCombatState.Acted;

                return BehaviorReturnCode.Success;
            }
            catch (Exception)
            {
                seb_CombatState = SimpleCombatState.Acted;

                return BehaviorReturnCode.Success;
            }

        }

        /// <summary>
        /// attack the target
        /// </summary>
        /// <returns>success or failure</returns>
        private BehaviorReturnCode attackTarget()
        {
            try
            {
                CombatEngine.Instance.attackTarget(seb_ThisCharacter, seb_Target);
                seb_CombatState = SimpleCombatState.Acted;

                return BehaviorReturnCode.Success;
            }
            catch (Exception)
            {
                seb_CombatState = SimpleCombatState.Acted;

                return BehaviorReturnCode.Success;
            }
            
        }

        private BehaviorReturnCode idle()
        {
            if (seb_HostileList.Count > 0)
            {
                seb_CombatState = SimpleCombatState.Thinking;

                return BehaviorReturnCode.Failure;
            }
            else
            {
                seb_CombatState = SimpleCombatState.Acted;

                return BehaviorReturnCode.Success;
            }
        }
    }


    
}
