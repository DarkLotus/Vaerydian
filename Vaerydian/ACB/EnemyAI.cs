/*
 Author:
      Thomas H. Jonell <@Net_Gnome>
 
 Copyright (c) 2013 Thomas H. Jonell

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Lesser General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Text;

using ECSFramework;

using AgentComponentBus.Core;

using Vaerydian.Components.Characters;
using Vaerydian.Behaviors;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Spatials;
using Vaerydian.Utils;
using Microsoft.Xna.Framework;
using AgentComponentBus.Components;

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


    static class EnemyAI
    {

        public static ECSInstance ECSInstance;

		public static string COMMITTED = "AI_COMITTED";
		public static string RETRIEVED = "AI_RETRIEVED";

		public static void init(Agent agent){
			ResourcePool.on (EnemyAI.COMMITTED, agent, onCommitted);
			ResourcePool.on (EnemyAI.RETRIEVED, agent, doStateMachine);
		}

        public static TaskObject getComponents(TaskObject taskObject)
        {
            Bag<IComponent> components = new Bag<IComponent>();
            
            components.Set(AiBehavior.TypeID, new AiBehavior());
            components.Set(Aggrivation.TypeID, new Aggrivation());
            components.Set(Position.TypeID, new Position());
            components.Set(StateContainer<EnemyState, EnemyState>.TypeID, new StateContainer<EnemyState, EnemyState>());

			BusDataRetrieval bdr = new BusDataRetrieval ();
			bdr.Agent = taskObject.Agent;
			bdr.Data = components;
			bdr.EventName = EnemyAI.RETRIEVED;

			ResourcePool.issueRetrieve (bdr);

            return taskObject;
        }

		public static TaskObject doCommit(TaskObject taskObject){
			BusDataCommit bdc = new BusDataCommit ();
			bdc.Agent = taskObject.Agent;
			bdc.Data = (Bag<IComponent>)taskObject.Parameters [0];
			bdc.EventName = EnemyAI.COMMITTED;

			ResourcePool.issueCommit (bdc);

			return taskObject;
		}

		public static void onCommitted(EventObject eventObject){
			ResourcePool.issueTask (eventObject.Agent, EnemyAI.getComponents, delegate(TaskObject taskObject) {});
		}

		public static void run(Agent agent){
			ResourcePool.issueTask (agent, EnemyAI.getComponents, delegate (TaskObject TaskObject){});
		}

		public static void doStateMachine(EventObject eventObject)
        {
			Agent agent = eventObject.Agent;
			BusDataRetrieval bdr = (BusDataRetrieval) eventObject.Parameters [0];
			Bag<IComponent> components = bdr.Data;

            //retrieve state container
            StateContainer<EnemyState, EnemyState> stateContainer = (StateContainer<EnemyState, EnemyState>)components.Get(StateContainer<EnemyState, EnemyState>.TypeID);

            //evaluate state machine
            if(stateContainer != null)
                stateContainer.StateMachine.evaluate(agent, components);// aggro);

			ResourcePool.issueTask (eventObject.Agent, doCommit, delegate(TaskObject taskObject) {}, components);

			return;
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
                    
					Console.Error.WriteLine("Agent {0} switching to follow...", agent.Id);

					AiBehavior behavior = (AiBehavior)components.Get(AiBehavior.TypeID);
                    behavior.Behavior = new FollowerBehavior(agent.Entity, aggro.Target, 100, ECSInstance);

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
					Console.Error.WriteLine("Agent {0} switching to attack...", agent.Id);

                    AiBehavior behavior = (AiBehavior)components.Get(AiBehavior.TypeID);
                    behavior.Behavior = new WanderingEnemyBehavior(agent.Entity, ECSInstance);

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
