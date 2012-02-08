﻿using System;
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
using Vaerydian.Components.Debug;
using Vaerydian.Utils;
using Vaerydian.Behaviors.Actions;


namespace Vaerydian.Behaviors
{
    class SimpleFollowBehavior : CharacterBehavior
    {

        private Behavior s_Behavior;

        private ECSInstance s_EcsInstance;

        private Entity s_ThisEntity;
        private Entity s_Target;
        private Entity s_Map;
        private Entity s_Camera;
        private Entity s_MapDebug;

        private float s_FollowDistance;

        private int s_TileSize = 25;
        private int s_currentPathCell = 0;

        private bool s_BeginPathingAndMovement = false;
        private bool s_newPath = true;

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

        private ComponentMapper s_PositionMapper;
        private ComponentMapper s_VelocityMapper;
        private ComponentMapper s_HeadingMapper;
        private ComponentMapper s_GameMapMapper;
        private ComponentMapper s_ViewPortMapper;
        private ComponentMapper s_MapDebugMapper;

        private FindPathAction findPath;

        private Vector2 s_TargetPreviousPosition, s_TargetCurrentPosition;

        private Vector2 s_Offset = new Vector2(12.5f);
        private Vector2 s_Center = new Vector2(0);

        private List<Cell> s_currentPath = new List<Cell>();

        private Cell s_CurrentCell = new Cell();
        private Cell s_TargetCell = new Cell();

        private MapDebug s_Debug;
        

        public SimpleFollowBehavior(Entity entity, Entity target, float followDistance, ECSInstance ecsInstance)
        {
            //perform all needed setup
            s_ThisEntity = entity;
            s_FollowDistance = followDistance;
            s_Target = target;
            s_EcsInstance = ecsInstance;

            tooClose = new Conditional(isTooClose);
            targetMoved = new Conditional(hasTargetMoved);
            pathFound = new Conditional(hasPathBeenFound);
            pathFailed = new Conditional(hasPathFailed);
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

            ParallelSequence pSeqA = new ParallelSequence(initPathfinder, calcPath);

            ParallelSelector pSelA = new ParallelSelector(new Inverter(targetMoved), calcPath);
            ParallelSelector pSelB = new ParallelSelector(new Inverter(pathFound), getPath);
            ParallelSelector pSelC = new ParallelSelector(new Inverter(isNewPath), setPath);
            ParallelSelector pSelD = new ParallelSelector(new Inverter(reachedCell), getNextCell);
            ParallelSelector pSelE = new ParallelSelector(reachedTarget, moveToCell);

            ParallelSequence pSeqC = new ParallelSequence(new Inverter(tooClose),updatePosition, pSelA, pSelB, pSelC, pSelD, pSelE);

            //setup root node, choose initialization phase or pathing/movement phase
            root = new RootSelector(switchBehaviors, pSeqA, pSeqC);

            s_Behavior = new Behavior(root);

            s_PositionMapper = new ComponentMapper(new Position(), ecsInstance);
            s_VelocityMapper = new ComponentMapper(new Velocity(), ecsInstance);
            s_HeadingMapper = new ComponentMapper(new Heading(), ecsInstance);
            s_GameMapMapper = new ComponentMapper(new GameMap(), ecsInstance);
            s_ViewPortMapper = new ComponentMapper(new ViewPort(), ecsInstance);
            s_MapDebugMapper = new ComponentMapper(new MapDebug(), ecsInstance);
        }

        public override BehaviorReturnCode Behave()
        {
            return s_Behavior.Behave();
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

            Vector2 sVec, fVec;

            s_Center = viewport.getDimensions() / 2;

            sVec.X = (int)(start.getPosition().X + s_Center.X ) / s_TileSize;
            sVec.Y = (int)(start.getPosition().Y + s_Center.Y ) / s_TileSize;
            fVec.X = (int)(finish.getPosition().X + s_Center.X ) / s_TileSize;
            fVec.Y = (int)(finish.getPosition().Y + s_Center.Y ) / s_TileSize;

            s_TargetCell.Position = fVec;
            s_CurrentCell.Position = sVec;

            findPath = new FindPathAction(s_EcsInstance,sVec,fVec, map);

            s_TargetCurrentPosition = finish.getPosition() + s_Offset;

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
            Vector2 pos = mePos.getPosition();// +s_Offset;

            Vector2 vec = new Vector2(s_CurrentCell.Position.X * s_TileSize, s_CurrentCell.Position.Y * s_TileSize);

            //create heading from this entityt to targetNode
            vec = Vector2.Subtract(vec - s_Center, pos);
            vec.Normalize();

            //set heading
            meHeading.setHeading(Vector2.Multiply(vec, meVel.getVelocity()));

            //update position
            pos += meHeading.getHeading();
            mePos.setPosition(pos);
            
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
            float dist = Vector2.Distance(followPos.getPosition(), mePos.getPosition());
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
            s_TargetCurrentPosition = targetPos.getPosition() + s_Offset;

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
            if (s_TargetPreviousPosition != s_TargetCurrentPosition)
            {
                s_BeginPathingAndMovement = false;
                return true;
            }
            return false;
        }

        

        private bool hasNewPath()
        {
            return s_newPath;
        }

        private BehaviorReturnCode setNewPath()
        {
            s_currentPathCell = 0;
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

            GameMap map = (GameMap)s_GameMapMapper.get(s_Map);

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

            meVec = mePos.getPosition();
            celVec = s_CurrentCell.Position * s_TileSize;

            float dist = Vector2.Distance(meVec, celVec- s_Center);

            if (dist <= meVel.getVelocity())
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

            meVec = mePos.getPosition() +s_Offset;
            celVec = s_TargetCell.Position * s_TileSize + s_Offset;

            float dist = Vector2.Distance(meVec, celVec);

            if (dist <= s_Offset.Length())
                return true;
            return false;

            /*
            if (s_CurrentCell.Position == s_TargetCell.Position)
            {
                return true;
            }
            return false;
            */
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

        
    }
}
