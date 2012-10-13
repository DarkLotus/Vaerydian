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
    class BatHSM : BusComponent
    {

        private static ECSInstance b_ECSInstance;

        private StateMachine<PathState, String> b_StateMachine;

        private const short PARAM_AGENT = 0;
        private const short PARAM_COMPONENTS = 1;
        private const short PARAM_PATH = 2;
        private const short PARAM_AGGRO = 3;

        public BatHSM(ECSInstance ecsInstance)
        {
            b_ECSInstance = ecsInstance;

            b_StateMachine = new StateMachine<PathState, String>(PathState.Idle, whenIdle, "BASE");
            b_StateMachine.addState(PathState.FollowPath, (Object[] parameters) => { });
            b_StateMachine.addState(PathState.PathFound, whenPathFound);
            b_StateMachine.addState(PathState.DoPathing, whenDoPathing);
            b_StateMachine.addState(PathState.PathFailed, (Object[] parameters) => { });
            b_StateMachine.addState(PathState.ReadyToPath, (Object[] parameters) => { });
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

            b_StateMachine.setState(path.PathState);

            b_StateMachine.evaluate(agent, components, path, aggro);

            return components;
        }

        /// <summary>
        /// called when pathstate is idle
        /// </summary>
        /// <param name="parameters"></param>
        private static void whenIdle(Object[] parameters) { }//do nothing


        /// <summary>
        /// called when path state is path found
        /// </summary>
        /// <param name="parameters"></param>
        private static void whenPathFound(Object[] parameters)
        {
            Bag<IComponent> components = (Bag<IComponent>)parameters[PARAM_COMPONENTS];
            Path path = (Path)parameters[PARAM_PATH];

            AiBehavior behavior = (AiBehavior)components.Get(AiBehavior.TypeID);
            
            //FIX: needs to use the correct following path
            behavior.Behavior = new FollowPath();

            //update pathstate
            path.PathState = PathState.FollowPath;
        }

        /// <summary>
        /// called when path state is do pathing
        /// </summary>
        /// <param name="parameters"></param>
        private static void whenDoPathing(Object[] parameters)
        {
            Aggrivation aggro = (Aggrivation)parameters[PARAM_AGGRO];

            //don't continue 
            if (aggro.HateList.Count < 1)
                return;

            //FIX: this requires adjustment when hatelists are fixed
            aggro.Target = aggro.HateList[0];

            Bag<IComponent> components = (Bag<IComponent>)parameters[PARAM_COMPONENTS];
            Path path = (Path)parameters[PARAM_PATH];

            Position position = (Position)components.Get(Position.TypeID);
            Position target = ComponentMapper.get<Position>(aggro.Target);

            path.Start = position.Pos;
            path.Finish = target.Pos;

            path.Map = ComponentMapper.get<GameMap>(b_ECSInstance.TagManager.getEntityByTag("MAP"));
            
            //update pathstate
            path.PathState = PathState.ReadyToPath;
        }

        

    }
}
