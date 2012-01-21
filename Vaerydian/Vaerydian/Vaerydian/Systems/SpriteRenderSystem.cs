using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ECSFramework;

using Vaerydian.Components;

namespace Vaerydian.Systems
{
    class SpriteRenderSystem : EntityProcessingSystem
    {

        private Texture2D s_Texture;
        private GameContainer s_Container;
        private SpriteBatch s_SpriteBatch;
        private ComponentMapper s_PositionMapper;


        public SpriteRenderSystem(GameContainer gameContainer) : base() 
        {
            this.s_Container = gameContainer;
            this.s_SpriteBatch = gameContainer.SpriteBatch;
        }

        public override void initialize()
        {
            s_Texture = s_Container.ContentManager.Load<Texture2D>("characters\\player");

            s_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
        }

        protected override void process(Entity entity)
        {
            Position position = (Position) s_PositionMapper.get(entity);
            
            s_SpriteBatch.Draw(s_Texture, position.getPosition(), Color.White);
        }
    }
}
