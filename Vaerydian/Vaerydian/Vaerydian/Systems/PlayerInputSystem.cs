using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ECSFramework;
using ECSFramework.Utils;

using Vaerydian.Components;

namespace Vaerydian.Systems
{
    class PlayerInputSystem : EntityProcessingSystem
    {

        private ComponentMapper p_PositionMapper;
        private ComponentMapper p_VelocityMapper;
        private ComponentMapper p_ViewPortMapper;
        private ComponentMapper p_HeadingMapper;

        private Entity p_Camera;
        private Entity p_Mouse;

        public PlayerInputSystem() : base() { }

        public override void initialize()
        {
            p_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            p_VelocityMapper = new ComponentMapper(new Velocity(), e_ECSInstance);
            p_ViewPortMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
            p_HeadingMapper = new ComponentMapper(new Heading(), e_ECSInstance);
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

            Vector2 pos = position.getPosition();

            //should we exit?
            if (InputManager.isKeyPressed(Keys.Escape))
            {
                InputManager.YesExit = true;
            }

            //move up?
            if(InputManager.isKeyPressed(Keys.Up))
            {
                pos.Y -= velocity.getVelocity();
                position.setPosition(pos);
            }
            
            //move down?
            if (InputManager.isKeyPressed(Keys.Down))
            {
                pos.Y += velocity.getVelocity();
                position.setPosition(pos);
            }
            
            //move left?
            if (InputManager.isKeyPressed(Keys.Left))
            {
                pos.X -= velocity.getVelocity();
                position.setPosition(pos);
            }
            
            //move right?
            if (InputManager.isKeyPressed(Keys.Right))
            {
                pos.X += velocity.getVelocity();
                position.setPosition(pos);
            }

            //move twoards reticle
            if (InputManager.isKeyPressed(Keys.W))
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
            if (InputManager.isKeyPressed(Keys.S))
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
            if (InputManager.isKeyPressed(Keys.A))
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
            if (InputManager.isKeyPressed(Keys.D))
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
        }


        private float getAngle(Vector2 a, Vector2 b)
        {
            float dot = Vector2.Dot(a, b);

            return (float) Math.Acos(dot / (a.Length() * b.Length()));
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
