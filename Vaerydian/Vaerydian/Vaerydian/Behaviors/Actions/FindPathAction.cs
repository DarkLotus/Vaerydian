using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BehaviorLibrary;
using BehaviorLibrary.Components;
//using BehaviorLibrary.Components.Actions;

using Microsoft.Xna.Framework;

using Vaerydian.Utils;
using Vaerydian.Components;

using ECSFramework;
using Vaerydian.Components.Utils;

namespace Vaerydian.Behaviors.Actions
{
    class FindPathAction : BehaviorComponent
    {
        private AStarPathing f_Pathing;
        private ECSInstance f_EcsInstance;

        public FindPathAction(ECSInstance ecsInstance, Vector2 start, Vector2 finish, GameMap map) 
        {
            f_EcsInstance = ecsInstance;
            f_Pathing = new AStarPathing(start, finish, map);
        }

        public override BehaviorReturnCode Behave()
        {
            if (pathFound())
                return BehaviorReturnCode.Success;

            if (pathFailed())
                return BehaviorReturnCode.Failure;

            f_Pathing.findPath();
            
            return BehaviorReturnCode.Running;
        }

        public void initialize()
        {

        }

        public List<Cell> getPath()
        {
            return f_Pathing.getPath();
        }

        public List<Cell> getClosedSet()
        {
            return f_Pathing.ClosedSet;
        }

        /*public List<Cell> getOpenSet()
        {
            return f_Pathing.OpenSet;
        }*/

        public BinaryHeap<Cell> getOpenSet()
        {
            return f_Pathing.OpenSet;
        }

        public List<Cell> getBlockingSet()
        {
            return f_Pathing.BlockingSet;
        }

        /// <summary>
        /// has the path been found?
        /// </summary>
        /// <returns></returns>
        public bool pathFound()
        {
            return f_Pathing.IsFound;
        }

        public bool pathFailed()
        {
            return f_Pathing.Failed;
        }

        public void reset(Vector2 start, Vector2 finish, GameMap map)
        {
            f_Pathing.reset(start, finish, map);
        }

    }
}
