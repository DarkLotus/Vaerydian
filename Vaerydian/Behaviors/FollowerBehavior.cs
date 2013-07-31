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
using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Actions;
using BehaviorLibrary.Components.Composites;
using BehaviorLibrary.Components.Conditionals;
using BehaviorLibrary.Components.Decorators;

using ECSFramework;

using Microsoft.Xna.Framework;

using Vaerydian.Components;
using Vaerydian.Components.Dbg;
using Vaerydian.Utils;
using Vaerydian.Behaviors.Actions;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Graphical;


namespace Vaerydian.Behaviors
{
    class FollowerBehavior : CharacterBehavior
    {

        private Behavior s_Behavior;

        private ECSInstance s_EcsInstance;

        private Entity s_ThisEntity;
        private Entity s_Target;
        private Entity s_Map;
        private Entity s_Camera;
        private Entity s_MapDebug;
        private Entity s_Spatial;

        private float s_FollowDistance;

        private int s_TileSize = 32;
        private int s_currentPathCell = 0;

        private bool s_BeginPathingAndMovement = false;
        private bool s_newPath = true;
        private bool s_moved = false;

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

        private ComponentMapper s_PositionMapper;
        private ComponentMapper s_VelocityMapper;
        private ComponentMapper s_HeadingMapper;
        private ComponentMapper s_GameMapMapper;
        private ComponentMapper s_ViewPortMapper;
        private ComponentMapper s_MapDebugMapper;
        private ComponentMapper s_SpatialMapper;
        private ComponentMapper s_SpriteMapper;

        private FindPathAction findPath;

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

        public FollowerBehavior(Entity entity, Entity target, float followDistance, ECSInstance ecsInstance)
        {
            //perform all needed setup
            s_ThisEntity = entity;
            s_FollowDistance = followDistance;
            s_Target = target;
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

            ParallelSelector pSelA = new ParallelSelector(new Inverter(targetMoved), new Inverter(reset), calcPath);
            ParallelSelector pSelB = new ParallelSelector(new Inverter(pathFound), getPath);
            ParallelSelector pSelC = new ParallelSelector(new Inverter(isNewPath), setPath);
            ParallelSelector pSelD = new ParallelSelector(new Inverter(reachedCell), getNextCell);
            ParallelSelector pSelE = new ParallelSelector(reachedTarget, moveToCell);
            

            ParallelSequence pSeqB = new ParallelSequence(new Inverter(tooClose), updatePosition, pSelA, pSelB, pSelC, pSelD, pSelE, animate);

            //setup root node, choose initialization phase or pathing/movement phase
            root = new RootSelector(switchBehaviors, pSeqA, pSeqB);

            s_Behavior = new Behavior(root);

            s_PositionMapper = new ComponentMapper(new Position(), ecsInstance);
            s_VelocityMapper = new ComponentMapper(new Velocity(), ecsInstance);
            s_HeadingMapper = new ComponentMapper(new Heading(), ecsInstance);
            s_GameMapMapper = new ComponentMapper(new GameMap(), ecsInstance);
            s_ViewPortMapper = new ComponentMapper(new ViewPort(), ecsInstance);
            s_MapDebugMapper = new ComponentMapper(new MapDebug(), ecsInstance);
            s_SpatialMapper = new ComponentMapper(new SpatialPartition(), ecsInstance);
            s_SpriteMapper = new ComponentMapper(new Sprite(), ecsInstance);
        }

        public override BehaviorReturnCode Behave()
        {
            return s_Behavior.Behave();
        }

        public override void deathCleanup()
        {
            //remove old references
            if (s_LastNode != null)
            {
                s_LastNode.Contents.Remove(s_ThisEntity);
            }

            c_IsClean = true;
        }


        private int switchBehaviors()
        {
            //is it time to start pathing?
            if (s_BeginPathingAndMovement)
                return PATHING_AND_MOVEMENT;
            return INITIALIZATION;
        }

        private BehaviorReturnCode initializePathfinder()
        {
            Position start = (Position)s_PositionMapper.get(s_ThisEntity);
            Position finish = (Position)s_PositionMapper.get(s_Target);
            
            s_Map = s_EcsInstance.TagManager.getEntityByTag("MAP");
            GameMap map = (GameMap)s_GameMapMapper.get(s_Map);

            s_Camera = s_EcsInstance.TagManager.getEntityByTag("CAMERA");
            ViewPort viewport = (ViewPort)s_ViewPortMapper.get(s_Camera);

            s_Spatial = s_EcsInstance.TagManager.getEntityByTag("SPATIAL");
            SpatialPartition spatial = (SpatialPartition)s_SpatialMapper.get(s_Spatial);

            spatial.QuadTree.setContentAtLocation(s_ThisEntity, start.Pos);
            s_LastNode = spatial.QuadTree.locateNode(start.Pos);

            Vector2 sVec, fVec;

            //s_Center = viewport.getDimensions() / 2;

            sVec = (start.Pos) / s_TileSize;
            fVec = (finish.Pos + finish.Offset) / s_TileSize;

            /*
            sVec = (start.Pos + s_Center) / s_TileSize;
            fVec = (finish.Pos + s_Center) / s_TileSize; 
            */

            s_TargetCell.Position = fVec;
            s_CurrentCell.Position = sVec;

            findPath = new FindPathAction(s_EcsInstance,sVec,fVec, map);

            s_TargetCurrentPosition = finish.Pos + s_Offset;

            //convert to map position
            s_TargetCurrentPosition = new Vector2((int) s_TargetCurrentPosition.X / s_TileSize, (int) s_TargetCurrentPosition.Y / s_TileSize);
            s_TargetPreviousPosition = s_TargetCurrentPosition;

            s_BeginPathingAndMovement = true;

            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// move toward the current cell
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode moveTowardsCell()
        {
            //get positions, velocity, and heading
            Position mePos = (Position)s_PositionMapper.get(s_ThisEntity);
            Velocity meVel = (Velocity)s_VelocityMapper.get(s_ThisEntity);
            Heading meHeading = (Heading)s_HeadingMapper.get(s_ThisEntity);

            //get current pos
            Vector2 pos = mePos.Pos;// +s_Offset;

            Vector2 vec = new Vector2(s_CurrentCell.Position.X * s_TileSize, s_CurrentCell.Position.Y * s_TileSize);

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

            s_moved = true;
            
            SpatialPartition spatial = (SpatialPartition) s_SpatialMapper.get(s_Spatial);

            //remove old reference
            if (s_LastNode != null)
                s_LastNode.Contents.Remove(s_ThisEntity);
            //set new position and get new reference
            spatial.QuadTree.setContentAtLocation(s_ThisEntity, pos);
            s_LastNode = spatial.QuadTree.locateNode(pos);


            return BehaviorReturnCode.Success;
        }

        private BehaviorReturnCode updateAnimation()
        {
            //grab components
            /*
            Sprite sprite = (Sprite)s_SpriteMapper.get(s_ThisEntity);
            Heading heading = (Heading)s_HeadingMapper.get(s_ThisEntity);

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
            if (s_moved)
                sprite.Column = s_Animation.updateFrame(s_EcsInstance.ElapsedTime);
            else
                s_Animation.reset();

            //reset movement flag
            s_moved = false;     
            */
            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// are we too close?
        /// </summary>
        /// <returns></returns>
        private bool isTooClose()
        {
            //get positions
            Position mePos = (Position)s_PositionMapper.get(s_ThisEntity);
            Position followPos = (Position)s_PositionMapper.get(s_Target);

            //check distance
            float dist = Vector2.Distance(followPos.Pos, mePos.Pos);
            return dist < s_FollowDistance;
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
            Position targetPos = (Position)s_PositionMapper.get(s_Target);
            s_TargetCurrentPosition = targetPos.Pos + s_Offset;

            //convert to map position
            s_TargetCurrentPosition = new Vector2((int)s_TargetCurrentPosition.X / s_TileSize, (int)s_TargetCurrentPosition.Y / s_TileSize);
            
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
            return s_newPath;
        }

        private BehaviorReturnCode setNewPath()
        {
            s_currentPathCell = 0;

            if (s_currentPath.Count < 1)
                return BehaviorReturnCode.Failure;

            s_CurrentCell = s_currentPath[s_currentPathCell];
            s_newPath = false;
            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// gets the next path
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode getNextPathCell()
        {
            s_currentPathCell++;
            s_CurrentCell = s_currentPath[s_currentPathCell];
            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// pathing failed
        /// </summary>
        /// <returns></returns>
        private bool hasPathFailed()
        {
            return findPath.pathFailed();
        }

        /// <summary>
        /// a path was found
        /// </summary>
        /// <returns></returns>
        private bool hasPathBeenFound()
        {
            return findPath.pathFound();
        }

        /// <summary>
        /// gets the path
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode getCurrentPath()
        {
            s_currentPath = findPath.getPath();

            //GameMap map = (GameMap)s_GameMapMapper.get(s_Map);

            s_MapDebug = s_EcsInstance.TagManager.getEntityByTag("MAP_DEBUG");
            s_Debug = (MapDebug)s_MapDebugMapper.get(s_MapDebug);

            s_Debug.ClosedSet = findPath.getClosedSet();
            s_Debug.OpenSet = findPath.getOpenSet();
            s_Debug.Blocking = findPath.getBlockingSet();
            s_Debug.Path = s_currentPath;

            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// has the target node
        /// </summary>
        /// <returns></returns>
        private bool hasReachedCell()
        {
            Position mePos = (Position) s_PositionMapper.get(s_ThisEntity);
            Velocity meVel = (Velocity) s_VelocityMapper.get(s_ThisEntity);
            
            
            Vector2 meVec, celVec;

            meVec = mePos.Pos;
            celVec = s_CurrentCell.Position * s_TileSize;

            //float dist = Vector2.Distance(meVec, celVec- s_Center);
            float dist = Vector2.Distance(meVec, celVec);

            if (dist <= meVel.Vel)
                return true;
            return false;
        }

        /// <summary>
        /// reached the final target cell
        /// </summary>
        /// <returns>true or false</returns>
        private bool hasReachedTarget()
        {
            Position mePos = (Position)s_PositionMapper.get(s_ThisEntity);
            
            Vector2 meVec, celVec;

            meVec = mePos.Pos +s_Offset;
            celVec = s_TargetCell.Position * s_TileSize + s_Offset;

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
            s_newPath = true;
            return findPath.Behave();
        }

        private BehaviorReturnCode resetPathfinder()
        {
            Position start = (Position)s_PositionMapper.get(s_ThisEntity);
            Position finish = (Position)s_PositionMapper.get(s_Target);
            GameMap map = (GameMap)s_GameMapMapper.get(s_Map);
            ViewPort viewport = (ViewPort)s_ViewPortMapper.get(s_Camera);

            Vector2 sVec, fVec;

            //s_Center = viewport.getDimensions() / 2;

            sVec = (start.Pos ) / s_TileSize;
            fVec = (finish.Pos + finish.Offset) / s_TileSize;
            /*
            sVec = (start.Pos + s_Center) / s_TileSize;
            fVec = (finish.Pos + s_Center) / s_TileSize;
            */
            s_TargetCell.Position = fVec;
            s_CurrentCell.Position = sVec;

            findPath.reset(sVec, fVec, map);

            s_currentPath.Clear();
            s_newPath = true;

            return BehaviorReturnCode.Success;
        }
    }
}
