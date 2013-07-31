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
using System.Linq;
using System.Text;

using BehaviorLibrary;
using ECSFramework;
using BehaviorLibrary.Components.Conditionals;
using BehaviorLibrary.Components.Composites;
using BehaviorLibrary.Components.Actions;
using Vaerydian.Behaviors.Actions;
using Microsoft.Xna.Framework;
using Vaerydian.Utils;
using Vaerydian.Components.Dbg;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Graphical;
using BehaviorLibrary.Components.Decorators;

namespace Vaerydian.Behaviors
{
    class FollowPath : CharacterBehavior
    {

        private Behavior f_Behavior;

        private ECSInstance s_EcsInstance;

        private Entity f_ThisEntity;
        private Entity f_Target;
        private Entity f_Map;
        private Entity f_Camera;
        private Entity f_MapDebug;
        private Entity f_Spatial;

        private float f_FollowDistance;

        private int f_TileSize = 32;
        private int f_currentPathCell = 0;

        private bool f_BeginPathingAndMovement = false;
        private bool f_newPath = true;
        private bool f_moved = false;

        private const int INITIALIZATION = 0;
        private const int PATHING_AND_MOVEMENT = 1;

        private Conditional tooClose;
        private Conditional targetMoved;
        private Conditional pathFound;
        private Conditional pathFailed;
        private Conditional reachedCell;
        private Conditional reachedTarget;
        private Conditional isNewPath;

        private RootSelector root;

        private BehaviorAction moveToCell;
        private BehaviorAction calcPath;
        private BehaviorAction initPathfinder;
        private BehaviorAction getNextCell;
        private BehaviorAction setPath;
        private BehaviorAction getPath;
        private BehaviorAction updatePosition;
        private BehaviorAction reset;
        private BehaviorAction animate;

        private ComponentMapper f_PositionMapper;
        private ComponentMapper f_VelocityMapper;
        private ComponentMapper f_HeadingMapper;
        private ComponentMapper f_GameMapMapper;
        private ComponentMapper f_ViewPortMapper;
        private ComponentMapper f_MapDebugMapper;
        private ComponentMapper f_SpatialMapper;
        private ComponentMapper f_SpriteMapper;
        private ComponentMapper f_PathMapper;

        //private FindPathAction findPath;

        private Vector2 s_TargetPreviousPosition, s_TargetCurrentPosition;

        private Vector2 s_Offset = new Vector2(16f);//12.5f);
        private Vector2 s_Center = new Vector2(0);

        private List<Cell> s_currentPath = new List<Cell>();

        private Cell s_CurrentCell = new Cell();
        private Cell s_TargetCell = new Cell();

        private MapDebug s_Debug;

        private QuadNode<Entity> s_LastNode;

        private const int MOVE_DOWN = 0;
        private const int MOVE_DOWNLEFT = 1;
        private const int MOVE_LEFT = 2;
        private const int MOVE_UPLEFT = 3;
        private const int MOVE_UP = 4;
        private const int MOVE_UPRIGHT = 5;
        private const int MOVE_RIGHT = 6;
        private const int MOVE_DOWNRIGHT = 7;

        private SpriteAnimation s_Animation = new SpriteAnimation(6, 42);

        public FollowPath(Entity entity, Entity target, float followDistance, ECSInstance ecsInstance)
        {
            //perform all needed setup
            f_ThisEntity = entity;
            f_FollowDistance = followDistance;
            f_Target = target;
            s_EcsInstance = ecsInstance;

            tooClose = new Conditional(isTooClose);
            targetMoved = new Conditional(hasTargetMoved);
            pathFound = new Conditional(hasPathBeenFound);
            reachedCell = new Conditional(hasReachedCell);
            reachedTarget = new Conditional(hasReachedTarget);
            isNewPath = new Conditional(hasNewPath);

            moveToCell = new BehaviorAction(moveTowardsCell);
            calcPath = new BehaviorAction(calculatePath);
            initPathfinder = new BehaviorAction(initializePathfinder);
            getNextCell = new BehaviorAction(getNextPathCell);
            setPath = new BehaviorAction(setNewPath);
            getPath = new BehaviorAction(getCurrentPath);
            updatePosition = new BehaviorAction(updateTargetPosision);
            reset = new BehaviorAction(resetPathfinder);
            animate = new BehaviorAction(updateAnimation);
            
            ParallelSequence pSeqA = new ParallelSequence(initPathfinder, calcPath);

            

            ParallelSelector pSelA = new ParallelSelector(new Inverter(targetMoved), new Inverter(reachedTarget), new Inverter(reset), calcPath);
            //ParallelSelector pSelA = new ParallelSelector(new Inverter(targetMoved));//, new Inverter(reset), calcPath);
            ParallelSelector pSelB = new ParallelSelector(new Inverter(pathFound), getPath);
            ParallelSelector pSelC = new ParallelSelector(new Inverter(isNewPath), setPath);
            ParallelSelector pSelD = new ParallelSelector(new Inverter(reachedCell), getNextCell);
            ParallelSelector pSelE = new ParallelSelector(reachedTarget, moveToCell);
            //ParallelSequence pSeqC = new ParallelSequence(pSelE, reset, calcPath);


            ParallelSequence pSeqB = new ParallelSequence(new Inverter(tooClose), updatePosition, pSelA, pSelB, pSelC, pSelD, pSelE, animate);

            //setup root node, choose initialization phase or pathing/movement phase
            root = new RootSelector(switchBehaviors, pSeqA, pSeqB);

            f_Behavior = new Behavior(root);

            f_PositionMapper = new ComponentMapper(new Position(), ecsInstance);
            f_VelocityMapper = new ComponentMapper(new Velocity(), ecsInstance);
            f_HeadingMapper = new ComponentMapper(new Heading(), ecsInstance);
            f_GameMapMapper = new ComponentMapper(new GameMap(), ecsInstance);
            f_ViewPortMapper = new ComponentMapper(new ViewPort(), ecsInstance);
            f_MapDebugMapper = new ComponentMapper(new MapDebug(), ecsInstance);
            f_SpatialMapper = new ComponentMapper(new SpatialPartition(), ecsInstance);
            f_SpriteMapper = new ComponentMapper(new Sprite(), ecsInstance);
            f_PathMapper = new ComponentMapper(new Path(), ecsInstance);
        }

        public override BehaviorReturnCode Behave()
        {
            return f_Behavior.Behave();
        }

        public override void deathCleanup()
        {
            //remove old references
            if (s_LastNode != null)
            {
                s_LastNode.Contents.Remove(f_ThisEntity);
            }

            c_IsClean = true;
        }


        private int switchBehaviors()
        {
            //is it time to start pathing?
            if (f_BeginPathingAndMovement)
                return PATHING_AND_MOVEMENT;
            return INITIALIZATION;
        }

        private BehaviorReturnCode initializePathfinder()
        {
            Position start = (Position)f_PositionMapper.get(f_ThisEntity);
            Position finish = (Position)f_PositionMapper.get(f_Target);
            
            f_Map = s_EcsInstance.TagManager.getEntityByTag("MAP");
            GameMap map = (GameMap)f_GameMapMapper.get(f_Map);

            f_Camera = s_EcsInstance.TagManager.getEntityByTag("CAMERA");
            ViewPort viewport = (ViewPort)f_ViewPortMapper.get(f_Camera);

            f_Spatial = s_EcsInstance.TagManager.getEntityByTag("SPATIAL");
            SpatialPartition spatial = (SpatialPartition)f_SpatialMapper.get(f_Spatial);

            spatial.QuadTree.setContentAtLocation(f_ThisEntity, start.Pos);
            s_LastNode = spatial.QuadTree.locateNode(start.Pos);

            Vector2 sVec, fVec;

            //s_Center = viewport.getDimensions() / 2;

            sVec = (start.Pos) / f_TileSize;
            fVec = (finish.Pos + finish.Offset) / f_TileSize;

            /*
            sVec = (start.Pos + s_Center) / s_TileSize;
            fVec = (finish.Pos + s_Center) / s_TileSize; 
            */

            s_TargetCell.Position = fVec;
            s_CurrentCell.Position = sVec;

            //findPath = new FindPathAction(s_EcsInstance,sVec,fVec, map);
            Path path = (Path)f_PathMapper.get(f_ThisEntity);
            path.Start = sVec;
            path.Finish = fVec;
            path.Map = map;
            path.PathState = PathState.Idle;

            s_TargetCurrentPosition = finish.Pos + s_Offset;

            //convert to map position
            s_TargetCurrentPosition = new Vector2((int) s_TargetCurrentPosition.X / f_TileSize, (int) s_TargetCurrentPosition.Y / f_TileSize);
            s_TargetPreviousPosition = s_TargetCurrentPosition;

            f_BeginPathingAndMovement = true;

            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// move toward the current cell
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode moveTowardsCell()
        {
            //get positions, velocity, and heading
            Position mePos = (Position)f_PositionMapper.get(f_ThisEntity);
            Velocity meVel = (Velocity)f_VelocityMapper.get(f_ThisEntity);
            Heading meHeading = (Heading)f_HeadingMapper.get(f_ThisEntity);

            //get current pos
            Vector2 pos = mePos.Pos;// +s_Offset;

            Vector2 vec = new Vector2(s_CurrentCell.Position.X * f_TileSize, s_CurrentCell.Position.Y * f_TileSize);

            //check for condition that could cause a not-a-number exception
            //if ((vec - s_Center) == pos)
            if ((vec) == pos)
                return BehaviorReturnCode.Success;

            //create heading from this entityt to targetNode
            //vec = Vector2.Subtract(vec - s_Center, pos);
            vec = Vector2.Subtract(vec, pos);
            vec.Normalize();

            //set heading
            meHeading.setHeading(Vector2.Multiply(vec, meVel.Vel));

            //update position
            pos += meHeading.getHeading();
            mePos.Pos = pos;

            f_moved = true;
            
            SpatialPartition spatial = (SpatialPartition) f_SpatialMapper.get(f_Spatial);

            //remove old reference
            if (s_LastNode != null)
                s_LastNode.Contents.Remove(f_ThisEntity);
            //set new position and get new reference
            spatial.QuadTree.setContentAtLocation(f_ThisEntity, pos);
            s_LastNode = spatial.QuadTree.locateNode(pos);


            return BehaviorReturnCode.Success;
        }

        private BehaviorReturnCode updateAnimation()
        {
            //grab components
            Sprite sprite = (Sprite)f_SpriteMapper.get(f_ThisEntity);
            Heading heading = (Heading)f_HeadingMapper.get(f_ThisEntity);

            //reset animation index
            sprite.Column = 0;

            //determine angle of heading
            float angle = VectorHelper.getAngle(new Vector2(1, 0), heading.getHeading());

            //adjust spritesheet row based on angle
            if (angle >= 0.393f && angle < 1.178f) { sprite.Row = MOVE_UPRIGHT; }
            else if (angle >= 1.178f && angle < 1.963f) { sprite.Row = MOVE_UP; }
            else if (angle >= 1.963f && angle < 2.749f) { sprite.Row = MOVE_UPLEFT; }
            else if (angle >= 2.749f && angle < 3.534f) { sprite.Row = MOVE_LEFT; }
            else if (angle >= 3.534f && angle < 4.320f) { sprite.Row = MOVE_DOWNLEFT; }
            else if (angle >= 4.320f && angle < 5.105f) { sprite.Row = MOVE_DOWN; }
            else if (angle >= 5.105f && angle < 5.890f) { sprite.Row = MOVE_DOWNRIGHT; }
            else if (angle >= 5.890f || angle < .393f) { sprite.Row = MOVE_RIGHT; }
            
            //if you moved this cycle, update your animation frame accordingly, otherwise reset
            if (f_moved)
                sprite.Column = s_Animation.updateFrame(s_EcsInstance.ElapsedTime);
            else
                s_Animation.reset();

            //reset movement flag
            f_moved = false;     

            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// are we too close?
        /// </summary>
        /// <returns></returns>
        private bool isTooClose()
        {
            //get positions
            Position mePos = (Position)f_PositionMapper.get(f_ThisEntity);
            Position followPos = (Position)f_PositionMapper.get(f_Target);

            //check distance
            float dist = Vector2.Distance(followPos.Pos, mePos.Pos);
            return dist < f_FollowDistance;
        }

        /// <summary>
        /// update targets position
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode updateTargetPosision()
        {
            //update previous
            s_TargetPreviousPosition = s_TargetCurrentPosition;

            //get latest position
            Position targetPos = (Position)f_PositionMapper.get(f_Target);
            s_TargetCurrentPosition = targetPos.Pos + s_Offset;

            //convert to map position
            s_TargetCurrentPosition = new Vector2((int)s_TargetCurrentPosition.X / f_TileSize, (int)s_TargetCurrentPosition.Y / f_TileSize);
            
            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// has the target moved?
        /// </summary>
        /// <returns></returns>
        private bool hasTargetMoved()
        {
            return s_TargetPreviousPosition != s_TargetCurrentPosition;
            /*
            if (s_TargetPreviousPosition != s_TargetCurrentPosition)
            {
                return true;
            }
            return false;*/
        }

        private bool hasNewPath()
        {
            return f_newPath;
        }

        private BehaviorReturnCode setNewPath()
        {
            f_currentPathCell = 0;
            
            if (s_currentPath.Count < 1)
                return BehaviorReturnCode.Failure;
            
            s_CurrentCell = s_currentPath[f_currentPathCell];
            f_newPath = false;
            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// gets the next path
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode getNextPathCell()
        {
            f_currentPathCell++;
            
            if (f_currentPathCell >= s_currentPath.Count)
                return BehaviorReturnCode.Failure;

            s_CurrentCell = s_currentPath[f_currentPathCell];
            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// pathing failed
        /// </summary>
        /// <returns></returns>
        private bool hasPathFailed()
        {
            Path path = (Path)f_PathMapper.get(f_ThisEntity);
            return path.PathState == PathState.PathFailed ? true : false;
        }

        /// <summary>
        /// a path was found
        /// </summary>
        /// <returns></returns>
        private bool hasPathBeenFound()
        {
            Path path = (Path)f_PathMapper.get(f_ThisEntity);
            return (path.PathState == PathState.PathFound ? true : false) && (path.FoundPath != null);
        }

        /// <summary>
        /// gets the path
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode getCurrentPath()
        {
            Path path = (Path)f_PathMapper.get(f_ThisEntity);

            
            s_currentPath = path.FoundPath;
            
            return BehaviorReturnCode.Success;


            //s_currentPath = findPath.getPath();

            //GameMap map = (GameMap)s_GameMapMapper.get(s_Map);

            /*
            f_MapDebug = s_EcsInstance.TagManager.getEntityByTag("MAP_DEBUG");
            s_Debug = (MapDebug)f_MapDebugMapper.get(f_MapDebug);

            s_Debug.ClosedSet = findPath.getClosedSet();
            s_Debug.OpenSet = findPath.getOpenSet();
            s_Debug.Blocking = findPath.getBlockingSet();
            s_Debug.Path = s_currentPath;

            return BehaviorReturnCode.Success;
            */
        }

        /// <summary>
        /// has the target node
        /// </summary>
        /// <returns></returns>
        private bool hasReachedCell()
        {
            Position mePos = (Position) f_PositionMapper.get(f_ThisEntity);
            Velocity meVel = (Velocity) f_VelocityMapper.get(f_ThisEntity);
            
            
            Vector2 meVec, celVec;

            meVec = mePos.Pos;
            celVec = s_CurrentCell.Position * f_TileSize;

            //float dist = Vector2.Distance(meVec, celVec- s_Center);
            float dist = Vector2.Distance(meVec, celVec);

            if (dist <= s_Offset.Length())//meVel.Vel)
                return true;
            return false;
        }

        /// <summary>
        /// reached the final target cell
        /// </summary>
        /// <returns>true or false</returns>
        private bool hasReachedTarget()
        {
            Position mePos = (Position)f_PositionMapper.get(f_ThisEntity);
            
            Vector2 meVec, celVec;

            meVec = mePos.Pos +s_Offset;
            celVec = s_TargetCell.Position * f_TileSize + s_Offset;

            float dist = Vector2.Distance(meVec, celVec);

            if (dist <= s_Offset.Length())
                return true;
            return false;
        }

        /// <summary>
        /// calculates a path
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode calculatePath()
        {
            f_newPath = true;
            Path path = (Path)f_PathMapper.get(f_ThisEntity);

            switch(path.PathState)
            {
                case PathState.PathFound:
                    return BehaviorReturnCode.Success;
                case PathState.PathFailed:
                    return BehaviorReturnCode.Failure;
                default:
                    path.PathState = PathState.DoPathing;
                    return BehaviorReturnCode.Running;
            }
        }

        private BehaviorReturnCode resetPathfinder()
        {
            Position start = (Position)f_PositionMapper.get(f_ThisEntity);
            Position finish = (Position)f_PositionMapper.get(f_Target);
            GameMap map = (GameMap)f_GameMapMapper.get(f_Map);
            ViewPort viewport = (ViewPort)f_ViewPortMapper.get(f_Camera);

            Vector2 sVec, fVec;

            //s_Center = viewport.getDimensions() / 2;

            sVec = (start.Pos ) / f_TileSize;
            fVec = (finish.Pos + finish.Offset) / f_TileSize;
            /*
            sVec = (start.Pos + s_Center) / s_TileSize;
            fVec = (finish.Pos + s_Center) / s_TileSize;
            */
            s_TargetCell.Position = fVec;
            s_CurrentCell.Position = sVec;

            Path path = (Path)f_PathMapper.get(f_ThisEntity);
            path.Map = map;
            path.Start = sVec;
            path.Finish = fVec;
            path.Failed = false;
            path.HasRun = false;
            path.PathState = PathState.Idle;
            //findPath.reset(sVec, fVec, map);

            s_currentPath.Clear();
            f_newPath = true;

            return BehaviorReturnCode.Success;
        }
    }
}
