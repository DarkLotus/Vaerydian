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
using Microsoft.Xna.Framework.Input;

using ECSFramework;


using Vaerydian.Components;
using Vaerydian.Utils;
using Vaerydian.Factories;

using Glimpse.Input;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Graphical;
using Vaerydian.Components.Characters;
using Vaerydian.Components.Utils;

namespace Vaerydian.Systems.Update
{
    class PlayerInputSystem : EntityProcessingSystem
    {

        private ComponentMapper p_PositionMapper;
        private ComponentMapper p_VelocityMapper;
        private ComponentMapper p_HeadingMapper;
        private ComponentMapper p_LightMapper;
        private ComponentMapper p_TransformMapper;
        private ComponentMapper p_CharacterMapper;
        private ComponentMapper p_SpatialMapper;
		private ComponentMapper p_TargetMapper;
		private ComponentMapper p_SpriteMapper;

        private Entity p_Mouse;
        private Entity p_Spatial;
		private Entity p_Target;

        private const int MOVE_DOWN = 0;
        private const int MOVE_DOWNLEFT = 1;
        private const int MOVE_LEFT = 2;
        private const int MOVE_UPLEFT = 3;
        private const int MOVE_UP = 4;
        private const int MOVE_UPRIGHT = 5;
        private const int MOVE_RIGHT = 6;
        private const int MOVE_DOWNRIGHT = 7;

        private SpriteAnimation p_Movement = new SpriteAnimation(6, 42);

        private bool p_Moved = false;
        private bool p_FirstRun = true;

        private int p_FireRate = 125;
        private int p_LastFired = 0;

        //private QuadNode<Entity> p_LastULNode;
        //private QuadNode<Entity> p_LastLLNode;
        //private QuadNode<Entity> p_LastLRNode;
        //private QuadNode<Entity> p_LastURNode;
        private QuadNode<Entity> p_LastNode;

        private Random rand = new Random();

		private bool p_StatWindowOpen = false;
		private Entity p_StatWindow;

		private bool p_InvWindowOpen = false;
		private Entity p_InvWindow;

        public PlayerInputSystem() : base() { }

        public override void initialize()
        {
            p_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            p_VelocityMapper = new ComponentMapper(new Velocity(), e_ECSInstance);
            p_HeadingMapper = new ComponentMapper(new Heading(), e_ECSInstance);
            p_LightMapper = new ComponentMapper(new Light(), e_ECSInstance);
            p_TransformMapper = new ComponentMapper(new Transform(), e_ECSInstance);
            p_CharacterMapper = new ComponentMapper(new Character(), e_ECSInstance);
            p_SpatialMapper = new ComponentMapper(new SpatialPartition(), e_ECSInstance);
			p_TargetMapper = new ComponentMapper (new Target (), e_ECSInstance);
			p_SpriteMapper = new ComponentMapper (new Sprite (), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            p_Mouse = e_ECSInstance.TagManager.getEntityByTag("MOUSE");
            p_Spatial = e_ECSInstance.TagManager.getEntityByTag("SPATIAL");
			p_Target = e_ECSInstance.TagManager.getEntityByTag ("TARGET");
        }

        protected override void cleanUp(Bag<Entity> entities) { }

        protected override void process(Entity entity)
        {
            Position position = (Position) p_PositionMapper.get(entity);
            Velocity velocity = (Velocity) p_VelocityMapper.get(entity);
            Heading heading = (Heading)p_HeadingMapper.get(entity);
            Position mPosition = (Position)p_PositionMapper.get(p_Mouse);
            Transform transform = (Transform)p_TransformMapper.get(entity);
            Character character = (Character)p_CharacterMapper.get(entity);
            SpatialPartition spatial = (SpatialPartition)p_SpatialMapper.get(p_Spatial);

            if (p_FirstRun)
            {
                spatial.QuadTree.setContentAtLocation(entity, position.Pos);
                p_LastNode = spatial.QuadTree.locateNode(position.Pos + position.Offset);
                p_FirstRun = false;
            }

            Vector2 pos = position.Pos;
            float vel = velocity.Vel;
            Vector2 head = heading.getHeading();
            
            //reset direction
            int dirCount = 0;

            //reset animation
            //sprite.Column = 0;
            p_Moved = false;

            //toggle light?
            if (InputManager.isKeyToggled(Keys.L))
            {
                Light light = (Light)p_LightMapper.get(p_Mouse);
                if (light.IsEnabled)
                    light.IsEnabled = false;
                else
                    light.IsEnabled = true;
            }

            

            //move up?
            if(InputManager.isKeyPressed(Keys.W))
            {
                Vector2 dir = new Vector2(0, -1);
                
                head += dir;
                dirCount++;

                //sprite.Y = MOVE_UP;
                //sprite.X = p_Movement.updateFrame(e_ECSInstance.ElapsedTime);
                p_Moved = true;
            }
            
            //move down?
            if (InputManager.isKeyPressed(Keys.S))
            {
                Vector2 dir = new Vector2(0, 1);

                head += dir;
                dirCount++;

                //sprite.Y = MOVE_DOWN;
                //sprite.X = p_Movement.updateFrame(e_ECSInstance.ElapsedTime);
                p_Moved = true;
            }
            
            //move left?
            if (InputManager.isKeyPressed(Keys.A))
            {
                Vector2 dir = new Vector2(-1, 0);

                head += dir;
                dirCount++;

                //sprite.Y = MOVE_LEFT;
                //sprite.X = p_Movement.updateFrame(e_ECSInstance.ElapsedTime);
                p_Moved = true;
            }
            
            //move right?
            if (InputManager.isKeyPressed(Keys.D))
            {
                Vector2 dir = new Vector2(1, 0);

                head += dir;
                dirCount++;

                //sprite.Y = MOVE_RIGHT;
                //sprite.X = p_Movement.updateFrame(e_ECSInstance.ElapsedTime);
                p_Moved = true;
            }

            //move according to the correct speed
            if (dirCount > 1)
            {
                position.Pos = pos + head * vel * (float)Math.Sqrt(2) / 2;
            }
            else
            {
                position.Pos = pos + head * vel;
            }

			/*
            Vector2 test = (mPosition.Pos + mPosition.Offset) - pos;
            test.Normalize();

            float angle = VectorHelper.getAngle(new Vector2(1,0), test);
            
            if (angle >= 0.393f && angle < 1.178f) { sprite.Row = MOVE_UPRIGHT; }
            else if (angle >= 1.178f && angle < 1.963f) { sprite.Row = MOVE_UP; }
            else if (angle >= 1.963f && angle < 2.749f) { sprite.Row = MOVE_UPLEFT; }
            else if (angle >= 2.749f && angle < 3.534f) { sprite.Row = MOVE_LEFT; }
            else if (angle >= 3.534f && angle < 4.320f) { sprite.Row = MOVE_DOWNLEFT; }
            else if (angle >= 4.320f && angle < 5.105f) { sprite.Row = MOVE_DOWN; }
            else if (angle >= 5.105f && angle < 5.890f) { sprite.Row = MOVE_DOWNRIGHT; }
            else if (angle >= 5.890f || angle < .393f) { sprite.Row = MOVE_RIGHT; }

            if(p_Moved)
                sprite.Column = p_Movement.updateFrame(e_ECSInstance.ElapsedTime);
            */

            if (p_Moved)
                character.CurrentAnimtaion = "MOVING";

            //transform.Rotation = getAngle(pos, mPosition.getPosition());

           

            /*
            if (InputManager.isLeftButtonClicked())
            {
                EntityFactory ef = new EntityFactory(e_ECSInstance);

                Vector2 dir = mPosition.getPosition() + mPosition.Offset - new Vector2(16) - pos;

                dir.Normalize();

                Transform trans = new Transform();
                trans.Rotation = -VectorHelper.getAngle(new Vector2(1, 0), dir);
                trans.RotationOrigin = new Vector2(16);

                ef.createCollidingProjectile(pos + dir * 16, dir, 10f, 1000, ef.createLight(true, 35, new Vector3(pos + position.Offset, 10), 0.7f, Color.Purple.ToVector4()), trans, entity);

                UtilFactory uf = new UtilFactory(e_ECSInstance);
                uf.createSound("audio\\effects\\fire", true,0.5f);
            }*/

            p_LastFired += e_ECSInstance.ElapsedTime;

            if (InputManager.isLeftButtonDown() && (p_LastFired >= 3*p_FireRate))
            {
                Target target = (Target)p_TargetMapper.get(p_Target);
                if (target.Active)
                {

                    p_LastFired = 0;

                    Position targetPos = (Position)p_PositionMapper.get(p_Target);

                    Vector2 dir = targetPos.Pos + targetPos.Offset - new Vector2(16) - pos;
                    dir.Normalize();

                    Transform trans = new Transform();
                    trans.Rotation = -VectorHelper.getAngle(new Vector2(1, 0), dir);
                    trans.RotationOrigin = new Vector2(0, 16);

                    UtilFactory.createMeleeAction(pos + dir * 16, dir, trans, entity);
                }
            }

            if (InputManager.isRightButtonDown() && (p_LastFired >= p_FireRate))
            {
				Target target = (Target)p_TargetMapper.get(p_Target);
				if(target.Active){

					p_LastFired = 0;

					Position targetPos = (Position) p_PositionMapper.get (p_Target);

					Vector2 dir = targetPos.Pos + targetPos.Offset - new Vector2(16) - pos;
	                //Vector2 dir = mPosition.Pos + mPosition.Offset - new Vector2(16) - pos;// +new Vector2(-20 + (float)rand.NextDouble() * 40, -20 + (float)rand.NextDouble() * 40);

	                dir = VectorHelper.rotateVectorRadians(dir, -0.08726f + (float)rand.NextDouble() * 0.1745f);

	                dir.Normalize();

	                Transform trans = new Transform();
	                trans.Rotation = -VectorHelper.getAngle(new Vector2(1, 0), dir);
	                trans.RotationOrigin = new Vector2(16);

					EntityFactory.createCollidingProjectile(pos + dir * 16, dir, 10f, 1000, EntityFactory.createLight(true, 2, new Vector3(pos + position.Offset, 10), 0.7f, Color.OrangeRed.ToVector4()), trans, entity);
	                
					UtilFactory.createSound("audio\\effects\\fire", true, 0.5f);
				}
            } 

			if (InputManager.isKeyToggled(Keys.B))
			{
				if(!p_StatWindowOpen){
					p_StatWindow = UIFactory.createStatWindow(entity, new Point(100,100),new Point(300,315),20);

					p_StatWindowOpen = true;
				}else
				{
					e_ECSInstance.deleteEntity(p_StatWindow);
					p_StatWindowOpen = false;
				}
			}

			if (InputManager.isKeyToggled (Keys.I)) {
				if (!p_InvWindowOpen) {
					p_InvWindow = UIFactory.createInventoryWindow (entity, new Point (300, 100), new Point (300, 300), 20, 10, 10);
					p_InvWindowOpen = true;
				}else {
					e_ECSInstance.deleteEntity (p_InvWindow);
					p_InvWindowOpen = false;
				}
			}


			if (InputManager.isKeyToggled (Keys.R)) {
				List<Entity> locals = spatial.QuadTree.findAllWithinRange (pos, 300);
				if (locals.Count > 1) {

					Target target = (Target) p_TargetMapper.get (p_Target);
					Sprite sprite = (Sprite) p_SpriteMapper.get (p_Target);
					sprite.Visible = true;
					target.Active = true;

					Entity closest = null;
					float min = float.MaxValue;

					for(int i = 0; i < locals.Count;i++){
						if(locals[i] == entity)
							continue;

						Position p = (Position) p_PositionMapper.get(locals[i]);

						if(p == null)
							continue;

						float dist = Vector2.Distance(p.Pos,position.Pos);
						if(dist < min){
							closest = locals[i];
							min = dist;
						}
					}

					target.TargetEntity = closest;
				}
			}

			//TEST FUNCTIONS PAST HERE

            if (InputManager.isKeyPressed(Keys.Up))
            {
                transform.Rotation += 0.1f;
            }

            if (InputManager.isKeyPressed(Keys.Down))
            {
                transform.Rotation -= 0.1f;
            }

            if (InputManager.isKeyPressed(Keys.Left))
            {
                transform.RotationOrigin = new Vector2(16,32);
            }

            if (InputManager.isKeyPressed(Keys.Right))
            {
                transform.RotationOrigin = new Vector2(0);
            }

            if (!p_Moved)
            {
                p_Movement.reset();
                character.CurrentAnimtaion = "IDLE";
            }
            else
            {

                if (p_LastNode != null)
                    p_LastNode.Contents.Remove(entity);

                p_LastNode = spatial.QuadTree.setContentAtLocation(entity, pos + new Vector2(16,16));

            }

            if(InputManager.isKeyPressed(Keys.P))
            {

                NPCFactory ef = new NPCFactory(e_ECSInstance);
                ef.createFollower(mPosition.Pos + mPosition.Offset - new Vector2(16), entity, rand.Next(50,300));

            }

            if (InputManager.isKeyToggled(Keys.O))
            {

                NPCFactory ef = new NPCFactory(e_ECSInstance);
                ef.createBatEnemy(mPosition.Pos + mPosition.Offset - new Vector2(16),15);

            }

            if (InputManager.isKeyToggled(Keys.U))
            {
				ActionDef def = GameConfig.ActionDefs["TEST_DMG"];
				ActionFactory.createAction(def,entity,entity);
            }


        }

       
    }
}
