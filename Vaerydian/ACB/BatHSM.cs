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
using Microsoft.Xna.Framework;

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

        //private StateMachine<EnemyState, short> b_StateMachine;

        private const short PARAM_AGENT = 0;
        private const short PARAM_COMPONENTS = 1;
        private const short PARAM_PATH = 2;
        private const short PARAM_AGGRO = 3;

        public BatHSM(ECSInstance ecsInstance)
        {
            b_ECSInstance = ecsInstance;

            /*
            //initialize state machine
            b_StateMachine = new StateMachine<EnemyState, short>(EnemyState.Idle, whenIdle, SM_TO_IDLE);
            
            //define states
            b_StateMachine.addState(EnemyState.Wandering, whenWandering);
            b_StateMachine.addState(EnemyState.Investigating, whenInvestigating);
            b_StateMachine.addState(EnemyState.Following, whenFollowing);
            b_StateMachine.addState(EnemyState.Attacking, whenAttacking);
            b_StateMachine.addState(EnemyState.Fleeing, whenFleeing);
            */
        }

        protected override Bag<IComponent> requestRetrievePacakge(Agent agent)
        {
            Bag<IComponent> components = new Bag<IComponent>();
            
            components.Set(AiBehavior.TypeID, new AiBehavior());
            components.Set(Aggrivation.TypeID, new Aggrivation());
            components.Set(Position.TypeID, new Position());
            components.Set(StateContainer<EnemyState, EnemyState>.TypeID, new StateContainer<EnemyState, EnemyState>());

            return components;
        }

        protected override Bag<IComponent> runProcess(Agent agent, Bag<IComponent> components)
        {
            //retrieve state container
            StateContainer<EnemyState, EnemyState> stateContainer = (StateContainer<EnemyState, EnemyState>)components.Get(StateContainer<EnemyState, EnemyState>.TypeID);

            //evaluate state machine
            if(stateContainer != null)
                stateContainer.StateMachine.evaluate(agent, components);// aggro);

            return components;
        }

        /// <summary>
        /// called when idle
        /// </summary>
        /// <param name="parameters"></param>
        public static void whenIdle(Object[] parameters) 
        {
            Bag<IComponent> components = (Bag<IComponent>)parameters[1];
            StateContainer<EnemyState, EnemyState> stateContainer = (StateContainer<EnemyState, EnemyState>)components.Get(StateContainer<EnemyState, EnemyState>.TypeID);

            //change to wandering state
            stateContainer.StateMachine.changeState(EnemyState.Wandering);
        }

        /// <summary>
        /// called when wandering
        /// </summary>
        /// <param name="parameters"></param>
        public static void whenWandering(Object[] parameters) 
        {
            Agent agent = (Agent)parameters[0];
            Bag<IComponent> components = (Bag<IComponent>) parameters[1];

            
            Aggrivation aggro = (Aggrivation)components.Get(Aggrivation.TypeID);
            Position ePos = (Position)components.Get(Position.TypeID);

            if (aggro.Target != null)
            {
                Position tPos = ComponentMapper.get<Position>(aggro.Target);
                float dist = Vector2.Distance(ePos.Pos, tPos.Pos);
                if ( dist >= 200f)
                {
                    AiBehavior behavior = (AiBehavior)components.Get(AiBehavior.TypeID);
                    behavior.Behavior = new FollowerBehavior(agent.Entity, aggro.Target, 100, b_ECSInstance);

                    StateContainer<EnemyState, EnemyState> stateContainer = (StateContainer<EnemyState, EnemyState>)components.Get(StateContainer<EnemyState, EnemyState>.TypeID);
                    stateContainer.StateMachine.changeState(EnemyState.Following);
                }
            }
        }

        /// <summary>
        /// called when investigating
        /// </summary>
        /// <param name="parameters"></param>
        public static void whenInvestigating(Object[] parameters) { }

        /// <summary>
        /// called when following
        /// </summary>
        /// <param name="parameters"></param>
        public static void whenFollowing(Object[] parameters) 
        {
            Agent agent = (Agent)parameters[0];
            Bag<IComponent> components = (Bag<IComponent>)parameters[1];


            Aggrivation aggro = (Aggrivation)components.Get(Aggrivation.TypeID);
            Position ePos = (Position)components.Get(Position.TypeID);

            if (aggro.Target != null)
            {
                Position tPos = ComponentMapper.get<Position>(aggro.Target);
                float dist = Vector2.Distance(ePos.Pos, tPos.Pos);
                if (dist < 100f)
                {
                    AiBehavior behavior = (AiBehavior)components.Get(AiBehavior.TypeID);
                    behavior.Behavior = new WanderingEnemyBehavior(agent.Entity, b_ECSInstance);

                    StateContainer<EnemyState, EnemyState> stateContainer = (StateContainer<EnemyState, EnemyState>)components.Get(StateContainer<EnemyState, EnemyState>.TypeID);
                    stateContainer.StateMachine.changeState(EnemyState.Wandering);
                }
            }
        }

        /// <summary>
        /// called when attacking
        /// </summary>
        /// <param name="parameters"></param>
        public static void whenAttacking(Object[] parameters) { }

        /// <summary>
        /// called when fleeing
        /// </summary>
        /// <param name="parameters"></param>
        public static void whenFleeing(Object[] parameters) { }
        
    }



}
