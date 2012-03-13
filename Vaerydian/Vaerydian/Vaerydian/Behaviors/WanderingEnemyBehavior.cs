using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BehaviorLibrary;
using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Actions;
using BehaviorLibrary.Components.Composites;
using BehaviorLibrary.Components.Conditionals;
using BehaviorLibrary.Components.Decorators;

using ECSFramework;

using Vaerydian.Components;

namespace Vaerydian.Behaviors
{
    class WanderingEnemyBehavior : CharacterBehavior
    {
        private ECSInstance s_ECSInstance;

        private Behavior s_Behavior;
        private RootSelector s_Root;

        private Entity s_ThisEntity;
        private Entity s_Target;
        private Entity s_Map;
        private Entity s_Camera;
        private Entity s_Spatial;

        private ComponentMapper s_PositionMapper;
        private ComponentMapper s_VelocityMapper;
        private ComponentMapper s_HeadingMapper;
        private ComponentMapper s_GameMapMapper;
        private ComponentMapper s_ColidableMapper;
        private ComponentMapper s_ViewPortMapper;
        private ComponentMapper s_SpatialMapper;
        private ComponentMapper s_SpriteMapper;

        private const int INITIALIZE = 0;
        private const int WANDER = 1;
        private const int ATTACK = 2;
        private const int FLEE = 3;

        private int s_CurrentState = 0;

        private BehaviorAction init;

        private Conditional tooClose;
        private Conditional tooFar;
        private Conditional detectedHostile;

        public WanderingEnemyBehavior(Entity entity, ECSInstance ecsInstance)
        {
            s_ECSInstance = ecsInstance;
            s_ThisEntity = entity;

            s_CurrentState = INITIALIZE;


            //setup all conditionals
            tooClose = new Conditional(()=>true);
            tooFar = new Conditional(()=>true);
            detectedHostile = new Conditional(hasDetectedHostile);

            //setup all behavior actions
            init = new BehaviorAction(initialize);
            





            ParallelSequence wanderSeq = new ParallelSequence();
            ParallelSequence attackSeq = new ParallelSequence();
            ParallelSequence fleeSeq = new ParallelSequence();

            s_Root = new RootSelector(switchBehaviors, init, wanderSeq, attackSeq, fleeSeq);

            s_Behavior = new Behavior(s_Root);

            s_PositionMapper = new ComponentMapper(new Position(), ecsInstance);
            s_VelocityMapper = new ComponentMapper(new Velocity(), ecsInstance);
            s_HeadingMapper = new ComponentMapper(new Heading(), ecsInstance);
            s_GameMapMapper = new ComponentMapper(new GameMap(), ecsInstance);
            s_ViewPortMapper = new ComponentMapper(new ViewPort(), ecsInstance);
            s_SpatialMapper = new ComponentMapper(new SpatialPartition(), ecsInstance);
            s_SpriteMapper = new ComponentMapper(new Sprite(), ecsInstance);
        }

        public override BehaviorReturnCode Behave()
        {
            return s_Behavior.Behave();
        }

        /// <summary>
        /// returns an int of which behavior branch to use
        /// </summary>
        /// <returns>the current state</returns>
        private int switchBehaviors()
        {
            return s_CurrentState;
        }

        /// <summary>
        /// perform all needed initialization
        /// </summary>
        /// <returns>whether initialization was a success or not</returns>
        private BehaviorReturnCode initialize()
        {

            //change state
            s_CurrentState = WANDER;

            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// checks for nearby enemy factions
        /// </summary>
        /// <returns>true if hostile was detected</returns>
        private bool hasDetectedHostile()
        {
            return false;
        }
    }
}
