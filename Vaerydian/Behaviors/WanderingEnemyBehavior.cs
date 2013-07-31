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

using Microsoft.Xna.Framework;

using BehaviorLibrary;
using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Actions;
using BehaviorLibrary.Components.Composites;
using BehaviorLibrary.Components.Conditionals;
using BehaviorLibrary.Components.Decorators;

using ECSFramework;

using Vaerydian.Components;
using Vaerydian.Components.Characters;
using Vaerydian.Characters;
using Vaerydian.Utils;
using Vaerydian.Components.Items;
using Vaerydian.Factories;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Graphical;


namespace Vaerydian.Behaviors
{
    class WanderingEnemyBehavior : CharacterBehavior
    {
        private ECSInstance w_ECSInstance;

        private Behavior w_Behavior;
        private RootSelector w_Root;

        private Entity w_ThisEntity;
        private Entity w_Target;
        private Entity w_Map;
        private Entity w_Camera;
        private Entity w_Spatial;

        private ComponentMapper w_PositionMapper;
        private ComponentMapper w_VelocityMapper;
        private ComponentMapper w_HeadingMapper;
        //private ComponentMapper s_GameMapMapper;
        private ComponentMapper w_ColidableMapper;
        private ComponentMapper w_ViewPortMapper;
        private ComponentMapper w_SpatialMapper;
        //private ComponentMapper w_SpriteMapper;
        private ComponentMapper w_FactionMapper;
        private ComponentMapper w_HealthMapper;
        private ComponentMapper w_EquipmentMapper;
        private ComponentMapper w_ItemMapper;
        private ComponentMapper w_WeaponMapper;
        private ComponentMapper w_AggroMapper;

        private const int STATE_INITIALIZE = 0;
        private const int STATE_WANDER = 1;
        private const int STATE_PURSUE = 2;
        private const int STATE_FLEE = 3;

        private int w_CurrentState = 0;

        private Factions w_EntityFaction;

        //private QuadNode<Entity> w_LastULNode;
        //private QuadNode<Entity> w_LastLLNode;
        //private QuadNode<Entity> w_LastLRNode;
        //private QuadNode<Entity> w_LastURNode;
        private QuadNode<Entity> w_LastNode;

        private Random w_Random = new Random((int)DateTime.Now.Ticks);

        private bool w_Moved = false;

        private const int MOVE_DOWN = 0;
        private const int MOVE_DOWNLEFT = 1;
        private const int MOVE_LEFT = 2;
        private const int MOVE_UPLEFT = 3;
        private const int MOVE_UP = 4;
        private const int MOVE_UPRIGHT = 5;
        private const int MOVE_RIGHT = 6;
        private const int MOVE_DOWNRIGHT = 7;

        private SpriteAnimation w_Animation = new SpriteAnimation(6, 42);

        private BehaviorAction init;
        private BehaviorAction setStateWander;
        private BehaviorAction setStatePursue;
        private BehaviorAction setStateFlee;
        private BehaviorAction choseDirection;
        private BehaviorAction collideCorrection;
        private BehaviorAction move;
        private BehaviorAction towardsHeading;
        private BehaviorAction awayHeading;
        private BehaviorAction fireShot;
        private BehaviorAction animate;
        private BehaviorAction playDetected;
        private BehaviorAction playFlee;

        private Conditional tooClose;
        private Conditional tooFar;
        private Conditional detectedHostile;
        private Conditional collided;
        private Conditional healthy;
        private Conditional targetDead;

        public WanderingEnemyBehavior(Entity entity, ECSInstance ecsInstance)
        {
            w_ECSInstance = ecsInstance;
            w_ThisEntity = entity;

            w_CurrentState = STATE_INITIALIZE;


            //setup all conditionals
            tooClose = new Conditional(tooCloseToTarget);
            tooFar = new Conditional(tooFarFromTarget);
            detectedHostile = new Conditional(hasDetectedHostile);
            collided = new Conditional(hasCollided);
            healthy = new Conditional(isHealthy);
            targetDead = new Conditional(isTargetDead);

            //setup all behavior actions
            init = new BehaviorAction(initialize);
            setStateWander = new BehaviorAction(stateChangeWander);
            setStatePursue = new BehaviorAction(stateChangePursue);
            setStateFlee = new BehaviorAction(stateChangeFlee);
            
            choseDirection = new BehaviorAction(chooseRandomDirection);
            collideCorrection = new BehaviorAction(correctHeadingForCollision);
            move = new BehaviorAction(moveViaHeading);
            towardsHeading = new BehaviorAction(headingTowardsTarget);
            awayHeading = new BehaviorAction(headingAwayTarget);
            fireShot = new BehaviorAction(fireAtTarget);
            animate = new BehaviorAction(updateAnimation);
            playDetected = new BehaviorAction(playDetectedSound);
            playFlee = new BehaviorAction(playFleeSound);


            ParallelSequence setPursue = new ParallelSequence(playDetected, setStatePursue);
            ParallelSequence setFlee = new ParallelSequence(playFlee, setStateFlee);

            //initialize sequence
            ParallelSequence initSeq = new ParallelSequence(init, setStateWander);

            //if not healthy, flee
            ParallelSelector healthSel = new ParallelSelector(healthy, new Inverter(setFlee));

            //if target is dead, wander
            ParallelSelector deadTargetSel = new ParallelSelector(new Inverter(targetDead), setStateWander);

            //if not collided, then chose a new direction every second
            ParallelSequence randWalk = new ParallelSequence(new Inverter(collided), new Timer(elapsedTime, 500, choseDirection));
            
            //if not randomly walking, correct for a collision
            ParallelSelector walkOrCorrect = new ParallelSelector(randWalk, collideCorrection);
            
            //wander sequence, while no hostiles detected, walk around randomly
            ParallelSequence wanderSeq = new ParallelSequence(new Inverter(new Timer(elapsedTime, 250, detectedHostile)), walkOrCorrect, move, animate);
            //ParallelSequence wanderSeq = new ParallelSequence(new Inverter(detectedHostile), walkOrCorrect, move, animate);
            
            //wander or change to pursue state88
            ParallelSelector wanderSel2 = new ParallelSelector(wanderSeq, new Inverter(setPursue));

            //move towards your target
            ParallelSequence moveTowards = new ParallelSequence(towardsHeading, move, animate);
            
            //move away from your target
            ParallelSequence moveAway = new ParallelSequence(new Timer(elapsedTime,250,awayHeading), move, animate);
            
            //if too far from your target, move towards it
            ParallelSelector moveTooFar = new ParallelSelector(new Inverter(tooFar), moveTowards);
            
            //if too close to your target, move away from it
            ParallelSelector moveTooClose = new ParallelSelector(new Inverter(tooClose), moveAway);
            
            //if target isnt dead and you're not too far and not too close, shoot at it
            ParallelSequence attackSeq = new ParallelSequence(deadTargetSel, new Inverter(tooFar), new Inverter(tooClose), new Timer(elapsedTime, 500, fireShot), animate);
            
            //move towards or away from your target, then attemp to attack it
            ParallelSequence pursAttackSeq1 = new ParallelSequence(moveTooFar, moveTooClose, attackSeq);
            
            //as long as your healthy, pursue and attack your target
            ParallelSequence pursAttackSeq2 = new ParallelSequence(healthSel, pursAttackSeq1);

            //flee sequence, while unhealthy, flee
            ParallelSequence fleeSeq = new ParallelSequence(new Inverter(healthy), deadTargetSel, new Timer(elapsedTime,250,awayHeading), move, animate);
            ParallelSelector fleeSel = new ParallelSelector(fleeSeq, setStateWander);

            //setup root selector
            w_Root = new RootSelector(switchBehaviors, initSeq, wanderSel2, pursAttackSeq2, fleeSel); 
            
            //set tree reference
            w_Behavior = new Behavior(w_Root);

            //initialize mappers
            w_PositionMapper = new ComponentMapper(new Position(), ecsInstance);
            w_VelocityMapper = new ComponentMapper(new Velocity(), ecsInstance);
            w_HeadingMapper = new ComponentMapper(new Heading(), ecsInstance);
            w_ColidableMapper = new ComponentMapper(new MapCollidable(), ecsInstance);
            w_ViewPortMapper = new ComponentMapper(new ViewPort(), ecsInstance);
            w_SpatialMapper = new ComponentMapper(new SpatialPartition(), ecsInstance);
            //w_SpriteMapper = new ComponentMapper(new Sprite(), ecsInstance);
            w_FactionMapper = new ComponentMapper(new Factions(), ecsInstance);
            w_HealthMapper = new ComponentMapper(new Health(), ecsInstance);
            w_EquipmentMapper = new ComponentMapper(new Equipment(), ecsInstance);
            w_ItemMapper = new ComponentMapper(new Item(), ecsInstance);
            w_WeaponMapper = new ComponentMapper(new Weapon(), ecsInstance);
            w_AggroMapper = new ComponentMapper(new Aggrivation(), ecsInstance);
        }

        public override BehaviorReturnCode Behave()
        {
            return w_Behavior.Behave();
        }

        public override void deathCleanup()
        {
            //remove old references
            if (w_LastNode != null)
                w_LastNode.Contents.Remove(w_ThisEntity);

            c_IsClean = true;
        }


        /// <summary>
        /// returns an int of which behavior branch to use
        /// </summary>
        /// <returns>the current state</returns>
        private int switchBehaviors()
        {
            return w_CurrentState;
        }

        /// <summary>
        /// perform all needed initialization
        /// </summary>
        /// <returns>whether initialization was a success or not</returns>
        private BehaviorReturnCode initialize()
        {
            Position position = (Position)w_PositionMapper.get(w_ThisEntity);

            w_Spatial =  w_ECSInstance.TagManager.getEntityByTag("SPATIAL");
            SpatialPartition spatial = (SpatialPartition)w_SpatialMapper.get(w_Spatial);

            Vector2 pos = position.Pos;

            Vector2 dir = new Vector2((float)w_Random.NextDouble() * 2 - 1, (float)w_Random.NextDouble() * 2 - 1);
            dir.Normalize();

            Heading heading = (Heading)w_HeadingMapper.get(w_ThisEntity);
            heading.setHeading(dir);
                        
            w_LastNode = spatial.QuadTree.setContentAtLocation(w_ThisEntity, pos + new Vector2(16,16));

            w_Camera = w_ECSInstance.TagManager.getEntityByTag("CAMERA");

            w_EntityFaction = (Factions)w_FactionMapper.get(w_ThisEntity);

            return BehaviorReturnCode.Success;
        }

        private BehaviorReturnCode stateChangeWander()
        {
            //change state
            w_CurrentState = STATE_WANDER;
            return BehaviorReturnCode.Success;
        }

        private BehaviorReturnCode stateChangePursue()
        {
            //change state
            w_CurrentState = STATE_PURSUE;
            return BehaviorReturnCode.Success;
        }

        private BehaviorReturnCode stateChangeFlee()
        {
            //change state
            w_CurrentState = STATE_FLEE;
            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// checks for nearby enemy factions
        /// </summary>
        /// <returns>true if hostile was detected</returns>
        private bool hasDetectedHostile()
        {

            if (w_LastNode == null)
                return false;
            
            SpatialPartition spatial = (SpatialPartition) w_SpatialMapper.get(w_Spatial);
            Position position = (Position) w_PositionMapper.get(w_ThisEntity);
            List<Entity> locals = spatial.QuadTree.findAllWithinRange(position.Pos, 100f);
            
             
            //nothing to detect
            if (locals.Count == 0)
                return false;
             
            for (int i = 0; i < locals.Count; i++)
            {
                //dont look at yourself
                if (locals[i] == w_ThisEntity)
                    continue;

                Factions factions = (Factions)w_FactionMapper.get(locals[i]);

                if (factions == null)
                    continue;

                //is this local known to this entity
                if (factions.KnownFactions.ContainsKey(w_EntityFaction.OwnerFaction.Name))
                {
                    //should this entity be hostile towards this local?
                    if (factions.KnownFactions[w_EntityFaction.OwnerFaction.Name].Value < 0)
                    {
                        Position pos = (Position)w_PositionMapper.get(w_ThisEntity);
                        Position tPos = (Position)w_PositionMapper.get(locals[i]);

                        if (Vector2.Distance(pos.Pos+pos.Offset,tPos.Pos +tPos.Offset) <= 200f)
                        {
                            //go hostile against it
                            w_Target = locals[i];

                            Aggrivation aggro = (Aggrivation)w_AggroMapper.get(w_ThisEntity);
                            aggro.Target = w_Target;

                            return true;
                        }
                        else
                            continue;
                    }
                }
                
            }
            
            return false;
        }

        /// <summary>
        /// chose a random direction to move in
        /// </summary>
        /// <returns>success if direction was chosen</returns>
        private BehaviorReturnCode chooseRandomDirection()
        {
            Heading heading = (Heading)w_HeadingMapper.get(w_ThisEntity);

            int neg = w_Random.NextDouble() > 0.5 ? 1:-1;

            heading.setHeading(VectorHelper.normalize(VectorHelper.rotateVectorDegrees(heading.getHeading(), (float) w_Random.NextDouble() * 45 * neg)));

            //heading.setHeading(dir);

            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// has the entity collided
        /// </summary>
        /// <returns>returns true if it did</returns>
        private bool hasCollided()
        {
            MapCollidable collidable = (MapCollidable)w_ColidableMapper.get(w_ThisEntity);

            return collidable.Collided;
        }

        /// <summary>
        /// returns the current elapsed time since last update
        /// </summary>
        /// <returns>elapsed time</returns>
        private int elapsedTime()
        {
            return w_ECSInstance.ElapsedTime;// -w_Random.Next(200);
        }

        /// <summary>
        /// corrects the heading for a collision
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode correctHeadingForCollision()
        {
            MapCollidable collidable = (MapCollidable)w_ColidableMapper.get(w_ThisEntity);
            Heading heading = (Heading)w_HeadingMapper.get(w_ThisEntity);

            Vector2 dir = collidable.ResponseVector;
            dir.Normalize();

            heading.setHeading(dir);

            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// are you currently too far from target
        /// </summary>
        /// <returns></returns>
        private bool tooFarFromTarget()
        {
            Equipment equip = (Equipment)w_EquipmentMapper.get(w_ThisEntity);

            Weapon weapon = (Weapon)w_WeaponMapper.get(equip.RangedWeapon);

            Position pos = (Position)w_PositionMapper.get(w_ThisEntity);
            Position tPos = (Position)w_PositionMapper.get(w_Target);

            float dist = Vector2.Distance(pos.Pos, tPos.Pos);

            if (dist > weapon.MaxRange)
                return true;

            return false;
        }

        /// <summary>
        /// are you currently too close to target
        /// </summary>
        /// <returns></returns>
        private bool tooCloseToTarget()
        {
            Equipment equip = (Equipment)w_EquipmentMapper.get(w_ThisEntity);

            Weapon weapon = (Weapon)w_WeaponMapper.get(equip.RangedWeapon);

            Position pos = (Position)w_PositionMapper.get(w_ThisEntity);
            Position tPos = (Position)w_PositionMapper.get(w_Target);

            float dist = Vector2.Distance(pos.Pos, tPos.Pos);

            if (dist < weapon.MinRange)
                return true;

            return false;
        }

        /// <summary>
        /// are you healthy
        /// </summary>
        /// <returns></returns>
        private bool isHealthy()
        {
            Health health = (Health)w_HealthMapper.get(w_ThisEntity);

            if (((float)health.CurrentHealth / (float)health.MaxHealth) > 0.25f)
                return true;
            else
                return false;
        }

        /// <summary>
        /// set heading towards target
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode headingTowardsTarget()
        {
            Heading heading = (Heading)w_HeadingMapper.get(w_ThisEntity);
            Position position = (Position)w_PositionMapper.get(w_ThisEntity);
            Position tPosition = (Position)w_PositionMapper.get(w_Target);

            Vector2 dir = (tPosition.Pos) - (position.Pos);

            dir.Normalize();

            heading.setHeading(dir);

            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// set heading away from target
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode headingAwayTarget()
        {
            Heading heading = (Heading)w_HeadingMapper.get(w_ThisEntity);
            Position position = (Position)w_PositionMapper.get(w_ThisEntity);
            Position tPosition = (Position)w_PositionMapper.get(w_Target);
			MapCollidable collide = (MapCollidable) w_ColidableMapper.get (w_ThisEntity);
			
			
            Vector2 dir = (tPosition.Pos) - (position.Pos);
			
			if(collide.Collided)
            	dir = Vector2.Negate(collide.ResponseVector);//VectorHelper.rotateVector(, (float)w_Random.NextDouble());

            dir.Normalize();

            heading.setHeading(Vector2.Negate(dir));

            return BehaviorReturnCode.Success;
        }
		
		private BehaviorReturnCode fleeFromTarget()
		{
			Heading heading = (Heading)w_HeadingMapper.get(w_ThisEntity);
            Position position = (Position)w_PositionMapper.get(w_ThisEntity);
            Position tPosition = (Position)w_PositionMapper.get(w_Target);
			MapCollidable collide = (MapCollidable) w_ColidableMapper.get (w_ThisEntity);
			
            Vector2 dir = (tPosition.Pos) - (position.Pos);
			
			if(collide.Collided)
				dir = Vector2.Negate(collide.ResponseVector);
			
            dir = VectorHelper.rotateVectorRadians(dir, (float)(w_Random.NextDouble()*(Math.PI/2f)));

            dir.Normalize();

            heading.setHeading(Vector2.Negate(dir));

            return BehaviorReturnCode.Success;
			
		}

        /// <summary>
        /// move using the entities heading
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode moveViaHeading()
        {
            Heading heading = (Heading)w_HeadingMapper.get(w_ThisEntity);
            Position position = (Position)w_PositionMapper.get(w_ThisEntity);
            Velocity velocity = (Velocity)w_VelocityMapper.get(w_ThisEntity);


            Vector2 pos = position.Pos;// +position.Offset;

            pos += heading.getHeading() * velocity.Vel;

            position.Pos = pos;

            w_Moved = true;

            SpatialPartition spatial = (SpatialPartition)w_SpatialMapper.get(w_Spatial);

            if (w_LastNode != null)
                w_LastNode.Contents.Remove(w_ThisEntity);

            w_LastNode = spatial.QuadTree.setContentAtLocation(w_ThisEntity, pos + new Vector2(16,16));
            

            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// fire a projectile at the target
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode fireAtTarget()
        {
          
            Position position = (Position) w_PositionMapper.get(w_ThisEntity);
            Position enemy = (Position) w_PositionMapper.get(w_Target);
            Heading heading = (Heading)w_HeadingMapper.get(w_ThisEntity);

            Vector2 dir = (enemy.Pos+enemy.Offset) - (position.Pos+position.Offset);

            dir = VectorHelper.rotateVectorRadians(dir, -0.08726f + (float)w_Random.NextDouble() * 0.1745f);
            
            dir.Normalize();

            heading.setHeading(dir);

            Vector2 pos = position.Pos;

            Transform trans = new Transform();
            trans.Rotation = -VectorHelper.getAngle(new Vector2(1, 0), dir);
            trans.RotationOrigin = new Vector2(16);

			EntityFactory.createSonicProjectile(pos + dir * 16, dir, 10, 1000, EntityFactory.createLight(true, 2, new Vector3(pos + position.Offset, 10), 0.7f, Color.Blue.ToVector4()), trans, w_ThisEntity);

            UtilFactory.createSound("audio\\effects\\fire", true, 0.5f);

            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// is the target currently dead?
        /// </summary>
        /// <returns></returns>
        private bool isTargetDead()
        {
            Health health = (Health)w_HealthMapper.get(w_Target);

            if (health == null)
                return true;

            if (health.CurrentHealth <= 0)
                return true;

            return false;
        }


        /// <summary>
        /// update the current animation frame
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode updateAnimation()
        {
            //grab components
            /*
            Sprite sprite = (Sprite)w_SpriteMapper.get(w_ThisEntity);
            Heading heading = (Heading)w_HeadingMapper.get(w_ThisEntity);

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
            if (w_Moved)
                sprite.Column = w_Animation.updateFrame(w_ECSInstance.ElapsedTime);
            else
                w_Animation.reset();

            //reset movement flag
            w_Moved = false;*/

            return BehaviorReturnCode.Success;
        }

        /// <summary>
        /// returns a random float
        /// </summary>
        /// <returns></returns>
        private float getRandom()
        {
            return (float)w_Random.NextDouble();
        }

        /// <summary>
        /// plays the flee sound sound
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode playFleeSound()
        {
            Position pos = (Position)w_PositionMapper.get(w_ThisEntity);
            ViewPort camera = (ViewPort)w_ViewPortMapper.get(w_Camera);

			UIFactory.createTimedDialogWindow(w_ThisEntity, "(wimper)", pos.Pos - camera.getOrigin(), "NPC-" + w_ThisEntity.Id, 1000);

            return BehaviorReturnCode.Success;
        }

        

        /// <summary>
        /// plays the detected sound
        /// </summary>
        /// <returns></returns>
        private BehaviorReturnCode playDetectedSound()
        {
            
            Position pos = (Position)w_PositionMapper.get(w_ThisEntity);
            ViewPort camera = (ViewPort)w_ViewPortMapper.get(w_Camera);

			UIFactory.createTimedDialogWindow(w_ThisEntity, "SCREEE!", pos.Pos - camera.getOrigin(), "NPC-" + w_ThisEntity.Id, 1000);

            return BehaviorReturnCode.Success;
        }

    }
}
