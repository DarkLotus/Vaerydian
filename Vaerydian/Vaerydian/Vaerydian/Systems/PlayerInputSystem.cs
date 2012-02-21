using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ECSFramework;
using ECSFramework.Utils;

using Vaerydian.Components;
using Vaerydian.Utils;
using Vaerydian.Factories;

namespace Vaerydian.Systems
{
    class PlayerInputSystem : EntityProcessingSystem
    {

        private ComponentMapper p_PositionMapper;
        private ComponentMapper p_VelocityMapper;
        private ComponentMapper p_ViewPortMapper;
        private ComponentMapper p_HeadingMapper;
        private ComponentMapper p_LightMapper;
        private ComponentMapper p_TransformMapper;
        private ComponentMapper p_SpriteMapper;

        private Entity p_Camera;
        private Entity p_Mouse;

        private const int MOVE_DOWN = 0;
        private const int MOVE_DOWNLEFT = 1;
        private const int MOVE_LEFT = 2;
        private const int MOVE_UPLEFT = 3;
        private const int MOVE_UP = 4;
        private const int MOVE_UPRIGHT = 5;
        private const int MOVE_RIGHT = 6;
        private const int MOVE_DOWNRIGHT = 7;

        private Animation p_Movement = new Animation(6, 42);

        private bool p_Moved = false;

        public PlayerInputSystem() : base() { }

        public override void initialize()
        {
            p_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            p_VelocityMapper = new ComponentMapper(new Velocity(), e_ECSInstance);
            p_ViewPortMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
            p_HeadingMapper = new ComponentMapper(new Heading(), e_ECSInstance);
            p_LightMapper = new ComponentMapper(new Light(), e_ECSInstance);
            p_TransformMapper = new ComponentMapper(new Transform(), e_ECSInstance);
            p_SpriteMapper = new ComponentMapper(new Sprite(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            p_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
            p_Mouse = e_ECSInstance.TagManager.getEntityByTag("MOUSE");
        }

        protected override void process(Entity entity)
        {
            Position position = (Position) p_PositionMapper.get(entity);
            Velocity velocity = (Velocity) p_VelocityMapper.get(entity);
            Heading heading = (Heading)p_HeadingMapper.get(entity);
            Position mPosition = (Position)p_PositionMapper.get(p_Mouse);
            Transform transform = (Transform)p_TransformMapper.get(entity);
            Sprite sprite = (Sprite)p_SpriteMapper.get(entity);

            Vector2 pos = position.getPosition();
            float vel = velocity.getVelocity();
            Vector2 head = heading.getHeading();
            
            //reset direction
            int dirCount = 0;

            //reset animation
            sprite.X = 0;
            p_Moved = false;

            //should we exit?
            if (InputManager.isKeyPressed(Keys.Escape))
            {
                InputManager.YesExit = true;
            }

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
                position.setPosition(pos + head * vel * (float)Math.Sqrt(2) / 2);
            }
            else
            {
                position.setPosition(pos + head * vel);
            }
            /*
            if (head == new Vector2(0, 1)) { sprite.Y = MOVE_DOWN; }
            else if (head == new Vector2(0, -1)) { sprite.Y = MOVE_UP; }
            else if (head == new Vector2(1, 0)) { sprite.Y = MOVE_RIGHT; }
            else if (head == new Vector2(-1, 0)) { sprite.Y = MOVE_LEFT; }
            else if (head == new Vector2(1, 1)) { sprite.Y = MOVE_DOWNRIGHT; }
            else if (head == new Vector2(1, -1)) { sprite.Y = MOVE_UPRIGHT; }
            else if (head == new Vector2(-1, 1)) { sprite.Y = MOVE_DOWNLEFT; }
            else if (head == new Vector2(-1, -1)) { sprite.Y = MOVE_UPLEFT; }
            */
            Vector2 test = (mPosition.getPosition() + mPosition.getOffset()) - pos;
            test.Normalize();

            float angle = getAngle(new Vector2(1,0), test);

            if (angle >= 0.393f && angle < 1.178f) { sprite.Y = MOVE_UPRIGHT; }
            else if (angle >= 1.178f && angle < 1.963f) { sprite.Y = MOVE_UP; }
            else if (angle >= 1.963f && angle < 2.749f) { sprite.Y = MOVE_UPLEFT; }
            else if (angle >= 2.749f && angle < 3.534f) { sprite.Y = MOVE_LEFT; }
            else if (angle >= 3.534f && angle < 4.320f) { sprite.Y = MOVE_DOWNLEFT; }
            else if (angle >= 4.320f && angle < 5.105f) { sprite.Y = MOVE_DOWN; }
            else if (angle >= 5.105f && angle < 5.890f) { sprite.Y = MOVE_DOWNRIGHT; }
            else if (angle >= 5.890f || angle < .393f) { sprite.Y = MOVE_RIGHT; }


            if(p_Moved)
                sprite.X = p_Movement.updateFrame(e_ECSInstance.ElapsedTime);


            //transform.Rotation = getAngle(pos, mPosition.getPosition());

            //move twoards reticle
            if (InputManager.isKeyPressed(Keys.T))
            {
                //get mouse location
                Vector2 mPos = mPosition.getPosition();

                //find vector pointing from entity towards reticle
                Vector2 vec = Vector2.Subtract(mPos, pos);
                vec.Normalize();
                
                //issue new heading
                heading.setHeading(Vector2.Multiply(vec,velocity.getVelocity()));
                
                //set new position
                pos += heading.getHeading();
                position.setPosition(pos);
            }

            //move away from reticle
            if (InputManager.isKeyPressed(Keys.G))
            {
                //get mouse location
                Vector2 mPos = mPosition.getPosition();

                //find vector pointing from entity away from reticle
                Vector2 vec = Vector2.Subtract(mPos, pos);
                vec.Normalize();
                vec = Vector2.Negate(vec);

                //issue new heading
                heading.setHeading(Vector2.Multiply(vec, velocity.getVelocity()));

                //set new position
                pos += heading.getHeading();
                position.setPosition(pos);
            }

            //move perp left
            if (InputManager.isKeyPressed(Keys.Q))
            {
                //get mouse location
                Vector2 mPos = mPosition.getPosition();

                //find vector pointing from entity towards reticle
                Vector2 vec = Vector2.Subtract(mPos, pos);
                vec = getLeftNormal(vec);
                vec.Normalize();

                //issue new heading
                heading.setHeading(Vector2.Multiply(vec, velocity.getVelocity()/2));

                //set new position
                pos += heading.getHeading();
                position.setPosition(pos);
            }

            //move perp right
            if (InputManager.isKeyPressed(Keys.E))
            {
                //get mouse location
                Vector2 mPos = mPosition.getPosition();

                //find vector pointing from entity towards reticle
                Vector2 vec = Vector2.Subtract(mPos, pos);
                vec = getRightNormal(vec);
                vec.Normalize();

                //issue new heading
                heading.setHeading(Vector2.Multiply(vec, velocity.getVelocity()/2));

                //set new position
                pos += heading.getHeading();
                position.setPosition(pos);
            }

            /*
            if (InputManager.isRightButtonClicked())
            {
                EntityFactory ef = new EntityFactory(e_ECSInstance);
                ef.createStandaloneLight(true, 50, new Vector3(pos+position.getOffset(),10), 0.5f, Color.Yellow.ToVector4());
            }*/

            if (InputManager.isLeftButtonClicked())
            {
                EntityFactory ef = new EntityFactory(e_ECSInstance);

                Vector2 dir = mPosition.getPosition() - pos;

                dir.Normalize();

                ef.createProjectile(pos, dir, 10f, 1000, ef.createLight(true, 25, new Vector3(pos + position.getOffset(), 10), 0.7f, Color.Purple.ToVector4()));
            }

            if (InputManager.isRightButtonDown())
            {
                EntityFactory ef = new EntityFactory(e_ECSInstance);

                Vector2 dir = mPosition.getPosition() - pos;

                dir.Normalize();

                ef.createProjectile(pos, dir, 10f, 1000, ef.createLight(true, 25, new Vector3(pos + position.getOffset(), 10), 0.7f, Color.OrangeRed.ToVector4()));
            }


            if (!p_Moved)
                p_Movement.reset();

        }

        private void adjustRotation(Vector2 me, Vector2 other)
        {
            
        }


        private float getAngle(Vector2 a, Vector2 b)
        {
            Vector2 ta = a;
            Vector2 tb = b;
            ta.Normalize();
            tb.Normalize();
            
            float dot = Vector2.Dot(ta, tb);
            
            if(ta.Y > tb.Y )
                return (float)Math.Acos(dot);
            else
                return 6.283f - (float)Math.Acos(dot);
        }

        private Vector2 rotateVector(Vector2 vector, float angle)
        {
            float x = vector.X * (float)Math.Cos(angle) - vector.Y * (float)Math.Sin(angle);
            float y = vector.X * (float)Math.Sin(angle) + vector.Y * (float)Math.Cos(angle);
            return new Vector2(x, y);
        }

        private Vector2 getRightNormal(Vector2 vector)
        {
            Vector2 returnVec;
            returnVec.X = -vector.Y;
            returnVec.Y = vector.X;
            return returnVec;
        }

        private Vector2 getLeftNormal(Vector2 vector)
        {
            Vector2 returnVec;
            returnVec.X = vector.Y;
            returnVec.Y = -vector.X;
            return returnVec;
        }
    }
}
