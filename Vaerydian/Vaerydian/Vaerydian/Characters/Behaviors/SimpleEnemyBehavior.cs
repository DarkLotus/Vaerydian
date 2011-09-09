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
                            
                            new Sequence(
                                //do i have a target?
                                new Inverter(new Conditional(hasLiveTarget)),
                                //get a target
                                new BehaviorAction(findTarget)
                                ),

                            //attempt to move within range of target
                            new Sequence(
                                //check to verify you still have a target
                                new Conditional(hasLiveTarget),
                                //check to see if you're  not in range of target
                                new Inverter(new Conditional(inRangeOfTarget)),
                                //not in range, so move towards target
                                new BehaviorAction(moveTowardsTarget)
                                ), 

                            //attempt to attack the target
                            new Sequence(
                                //check to verify you still have a target
                                new Conditional(hasLiveTarget),
                                //check to see if you're in range of target
                                new Conditional(inRangeOfTarget),

                                new Conditional(isTargetAttackable),
                                //attack target
                                new BehaviorAction(attackTarget)
                                )
                            )
                        )
                    );
        }

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

        private int getEnemyState()
        {
            return (int)seb_EnemyState;
        }

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

        private bool inRangeOfTarget()
        {
            if (Vector2.Distance(seb_ThisCharacter.BattlePosition, seb_Target.BattlePosition) <= seb_ThisCharacter.Equipment.Weapon.Range)
                return true;
            else
                return false;
        }

        private bool hasLiveTarget()
        {
            if (seb_Target == null)
                return false;
            
            if (seb_Target.Health > 0)
                return true;
            else
                return false;
        }

        private bool isTargetAttackable()
        {
            return CombatEngine.Instance.isCellAttackable(seb_Target.BattlePosition);
        }

        private BehaviorReturnCode moveTowardsTarget()
        {
            seb_CombatState = SimpleCombatState.Acted;

            return BehaviorReturnCode.Success;
        }


        private BehaviorReturnCode attackTarget()
        {
            CombatEngine.Instance.attackTarget(seb_ThisCharacter, seb_Target);

            seb_CombatState = SimpleCombatState.Acted;

            return BehaviorReturnCode.Success;
        }
    }


    
}
