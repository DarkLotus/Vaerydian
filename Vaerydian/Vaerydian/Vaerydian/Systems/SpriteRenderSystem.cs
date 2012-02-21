﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ECSFramework;
using ECSFramework.Utils;

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
        private ComponentMapper s_GeometryMapper;
        private ComponentMapper s_TranformMapper;
        private Entity s_Geometry;

        private Entity s_Camera;

        public SpriteRenderSystem(GameContainer gameContainer) : base() 
        {
            this.s_Container = gameContainer;
            this.s_SpriteBatch = gameContainer.SpriteBatch;
        }

        public override void initialize()
        {
            s_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            s_ViewportMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
            s_SpriteMapper = new ComponentMapper(new Sprite(), e_ECSInstance);
            s_GeometryMapper = new ComponentMapper(new GeometryMap(), e_ECSInstance);
            s_TranformMapper = new ComponentMapper(new Transform(), e_ECSInstance); 
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            Sprite sprite;
            String texName;
            
            //pre-load all known textures
            for (int i = 0; i < entities.Size(); i++)
            {
                sprite = (Sprite) s_SpriteMapper.get(entities.Get(i));
                texName = sprite.TextureName;
                if(!s_Textures.ContainsKey(texName))
                    s_Textures.Add(texName, s_Container.ContentManager.Load<Texture2D>(texName));

            }

            s_Textures.Add("projectile", s_Container.ContentManager.Load<Texture2D>("projectile"));

            //pre-load camera entity reference
            s_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
            s_Geometry = e_ECSInstance.TagManager.getEntityByTag("GEOMETRY");
        }

        protected override void process(Entity entity)
        {
            Position position = (Position) s_PositionMapper.get(entity);
            Sprite sprite = (Sprite) s_SpriteMapper.get(entity);
            ViewPort viewport = (ViewPort) s_ViewportMapper.get(s_Camera);
            GeometryMap geometry = (GeometryMap)s_GeometryMapper.get(s_Geometry);
            Transform transform = (Transform)s_TranformMapper.get(entity);


            Vector2 pos = position.getPosition();
            Vector2 offset = position.getOffset();
            Vector2 origin = viewport.getOrigin();
            Vector2 center = viewport.getDimensions() / 2;

            s_SpriteBatch.Begin();

            //s_SpriteBatch.Draw(s_Textures[sprite.getTextureName()], pos+center , null, Color.White, 0f, origin, new Vector2(1), SpriteEffects.None,0f);
            s_SpriteBatch.Draw(s_Textures[sprite.TextureName], pos + center, new Rectangle(sprite.X*sprite.Width, sprite.Y*sprite.Height, sprite.Width, sprite.Height), Color.White, 0f, origin, new Vector2(1), SpriteEffects.None, 0f);

            s_SpriteBatch.End();
        }
    }
}
