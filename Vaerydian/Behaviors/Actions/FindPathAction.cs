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
