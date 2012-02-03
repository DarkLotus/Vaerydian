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

using Microsoft.Xna.Framework;

using Vaerydian.Components;

namespace Vaerydian.Behaviors
{
    class SimpleFollowBehavior : CharacterBehavior
    {

        private Behavior s_Behavior;

        private Entity s_ThisEntity;
        private Entity s_Target;

        private float s_FollowDistance;

        private Conditional tooClose;
        private Inverter notClose;
        private BehaviorAction move;
        private RootSelector root;

        private ComponentMapper s_PositionMapper;
        private ComponentMapper s_VelocityMapper;
        private ComponentMapper s_HeadingMapper;

        public SimpleFollowBehavior(Entity entity, Entity target, float followDistance, ECSInstance ecsInstance)
        {
            //perform all needed setup
            s_ThisEntity = entity;
            s_FollowDistance = followDistance;
            s_Target = target;

            tooClose = new Conditional(isTooClose);
            notClose = new Inverter(tooClose);
            move = new BehaviorAction(moveToTarget);

            root = new RootSelector(() => 0, new ParallelSequence(notClose, move));

            s_Behavior = new Behavior(root);

            s_PositionMapper = new ComponentMapper(new Position(), ecsInstance);
            s_VelocityMapper = new ComponentMapper(new Velocity(), ecsInstance);
            s_HeadingMapper = new ComponentMapper(new Heading(), ecsInstance);
        }

        public override BehaviorReturnCode Behave()
        {
            return s_Behavior.Behave();
        }

        private BehaviorReturnCode moveToTarget()
        {
            //get positions, velocity, and heading
            Position mePos = (Position)s_PositionMapper.get(s_ThisEntity);
            Position targetPos = (Position)s_PositionMapper.get(s_Target);
            Velocity meVel = (Velocity)s_VelocityMapper.get(s_ThisEntity);
            Heading meHeading = (Heading)s_HeadingMapper.get(s_ThisEntity);

            //get current pos
            Vector2 pos = mePos.getPosition();

            //create heading from this entityt to target
            Vector2 vec = Vector2.Subtract(targetPos.getPosition(), pos);
            vec.Normalize();
            
            //set heading
            meHeading.setHeading(Vector2.Multiply(vec, meVel.getVelocity()));

            //update position
            pos += meHeading.getHeading();
            mePos.setPosition(pos);
            
            return BehaviorReturnCode.Success;
        }

        private bool isTooClose()
        {
            //get positions
            Position mePos = (Position)s_PositionMapper.get(s_ThisEntity);
            Position followPos = (Position)s_PositionMapper.get(s_Target);

            //check distance
            float dist = Vector2.Distance(followPos.getPosition(), mePos.getPosition());
            return dist < s_FollowDistance;
        }
    }
}
