using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using AgentComponentBus.Core;

using Vaerydian.Components.Characters;
using Vaerydian.Behaviors;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Spatials;
using Vaerydian.Utils;

namespace Vaerydian.ACB
{
    enum EnemyState
    {
        Idle,
        Wandering,
        Investigating,
        Following,
        Attacking,
        Fleeing
    }


    class BatHSM : BusComponent
    {

        private static ECSInstance b_ECSInstance;

        private StateMachine<EnemyState, short> b_StateMachine;

        private const short PARAM_AGENT = 0;
        private const short PARAM_COMPONENTS = 1;
        private const short PARAM_PATH = 2;
        private const short PARAM_AGGRO = 3;

        private const short SM_TO_IDLE = 0;
        private const short SM_TO_WANDERING = 1;
        private const short SM_TO_INVESTIGATING = 2;
        private const short SM_TO_FOLLOWING = 3;
        private const short SM_TO_ATTACKING = 4;
        private const short SM_TO_FLEEING = 5;

        public BatHSM(ECSInstance ecsInstance)
        {
            b_ECSInstance = ecsInstance;

            //initialize state machine
            b_StateMachine = new StateMachine<EnemyState, short>(EnemyState.Idle, whenIdle, SM_TO_IDLE);
            
            //define states
            b_StateMachine.addState(EnemyState.Wandering, whenWandering);
            b_StateMachine.addState(EnemyState.Investigating, whenInvestigating);
            b_StateMachine.addState(EnemyState.Following, whenFollowing);
            b_StateMachine.addState(EnemyState.Attacking, whenAttacking);
            b_StateMachine.addState(EnemyState.Fleeing, whenFleeing);
        }

        protected override Bag<IComponent> requestRetrievePacakge(Agent agent)
        {
            Bag<IComponent> components = new Bag<IComponent>();
            
            components.Set(AiBehavior.TypeID, new AiBehavior());
            components.Set(Aggrivation.TypeID, new Aggrivation());
            components.Set(Path.TypeID,new Path());
            components.Set(Position.TypeID, new Position());

            return components;
        }

        protected override Bag<IComponent> runProcess(Agent agent, Bag<IComponent> components)
        {
            Path path = (Path)components.Get(Path.TypeID);
            Aggrivation aggro = (Aggrivation)components.Get(Aggrivation.TypeID);

            b_StateMachine.evaluate(agent, components, path, aggro);

            return components;
        }

        /// <summary>
        /// called when idle
        /// </summary>
        /// <param name="parameters"></param>
        private void whenIdle(Object[] parameters) { }//do nothing

        /// <summary>
        /// called when wandering
        /// </summary>
        /// <param name="parameters"></param>
        private void whenWandering(Object[] parameters) { }

        /// <summary>
        /// called when investigating
        /// </summary>
        /// <param name="parameters"></param>
        private void whenInvestigating(Object[] parameters) { }

        /// <summary>
        /// called when following
        /// </summary>
        /// <param name="parameters"></param>
        private void whenFollowing(Object[] parameters) { }

        /// <summary>
        /// called when attacking
        /// </summary>
        /// <param name="parameters"></param>
        private void whenAttacking(Object[] parameters) { }

        /// <summary>
        /// called when fleeing
        /// </summary>
        /// <param name="parameters"></param>
        private void whenFleeing(Object[] parameters) { }
        
    }



}
