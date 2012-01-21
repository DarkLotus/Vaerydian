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

        private Dictionary<String,Texture2D> s_Textures = new Dictionary<String,Texture2D>();
        private GameContainer s_Container;
        private SpriteBatch s_SpriteBatch;
        private ComponentMapper s_PositionMapper;
        private ComponentMapper s_ViewportMapper;
        private ComponentMapper s_SpriteMapper;


        public SpriteRenderSystem(GameContainer gameContainer) : base() 
        {
            this.s_Container = gameContainer;
            this.s_SpriteBatch = gameContainer.SpriteBatch;
        }

        public override void initialize()
        {
            s_Textures.Add("player", s_Container.ContentManager.Load<Texture2D>("characters\\player"));
            s_Textures.Add("Title", s_Container.ContentManager.Load<Texture2D>("Title"));

            s_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            s_ViewportMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
            s_SpriteMapper = new ComponentMapper(new Sprite(), e_ECSInstance);
        }

        protected override void process(Entity entity)
        {
            Position position = (Position) s_PositionMapper.get(entity);
            Sprite sprite = (Sprite)s_SpriteMapper.get(entity);
            ViewPort viewport = (ViewPort) s_ViewportMapper.get(e_ECSInstance.TagManager.getEntityByTag("CAMERA"));

            Vector2 pos = position.getPosition();
            Vector2 origin = viewport.getOrigin();
            Vector2 center = viewport.getDimensions() / 2;
            
            s_SpriteBatch.Draw(s_Textures[sprite.getTextureName()], pos + center, null, Color.White,0f,origin, new Vector2(1), SpriteEffects.None,0f);
        }
    }
}
