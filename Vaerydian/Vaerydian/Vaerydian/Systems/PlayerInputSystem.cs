using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ECSFramework;

using Vaerydian.Components;

namespace Vaerydian.Systems
{
    class PlayerInputSystem : EntityProcessingSystem
    {

        private ComponentMapper p_PositionMapper;
        private ComponentMapper p_VelocityMapper;

        public PlayerInputSystem() : base() { }

        public override void initialize()
        {
            p_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            p_VelocityMapper = new ComponentMapper(new Velocity(), e_ECSInstance);
        }

        protected override void process(Entity entity)
        {
            Position position = (Position) p_PositionMapper.get(entity);
            Velocity velocity = (Velocity) p_VelocityMapper.get(entity);

            Vector2 pos = position.getPosition();

            if (InputManager.isKeyPressed(Keys.Escape))
            {
                InputManager.yesExit = true;
            }

            if(InputManager.isKeyPressed(Keys.Up))
            {
                pos.Y -= velocity.getVelocity();
                position.setPosition(pos);
            }
            
            if (InputManager.isKeyPressed(Keys.Down))
            {
                pos.Y += velocity.getVelocity();
                position.setPosition(pos);
            }
            
            if (InputManager.isKeyPressed(Keys.Left))
            {
                pos.X -= velocity.getVelocity();
                position.setPosition(pos);
            }
            
            if (InputManager.isKeyPressed(Keys.Right))
            {
                pos.X += velocity.getVelocity();
                position.setPosition(pos);
            }
        }
    }
}
