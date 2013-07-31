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
using Microsoft.Xna.Framework.Graphics;

using ECSFramework;


using Vaerydian.Components;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Graphical;

namespace Vaerydian.Systems.Draw
{
    class SpriteNormalSystem : EntityProcessingSystem
    {
        
        private Dictionary<String, Texture2D> s_Normals = new Dictionary<string, Texture2D>();
        private GameContainer s_Container;
        private SpriteBatch s_SpriteBatch;
        private ComponentMapper s_PositionMapper;
        private ComponentMapper s_ViewportMapper;
        private ComponentMapper s_SpriteMapper;
        private ComponentMapper s_GeometryMapper;
        private ComponentMapper s_TranformMapper;
        private Entity s_Geometry;

        private Entity s_Camera;

        public SpriteNormalSystem(GameContainer gameContainer) : base() 
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
                texName = sprite.NormalName;
                if(!s_Normals.ContainsKey(texName))
                    s_Normals.Add(texName, s_Container.ContentManager.Load<Texture2D>(texName));
            }

            s_Normals.Add("projectile", s_Container.ContentManager.Load<Texture2D>("projectile2"));

            //pre-load camera entity reference
            s_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
            s_Geometry = e_ECSInstance.TagManager.getEntityByTag("GEOMETRY");
        }

        protected override void cleanUp(Bag<Entity> entities) { }

        protected override void added(Entity entity)
        {
            base.added(entity);
            
            Sprite sprite = (Sprite)s_SpriteMapper.get(entity);
            if (!s_Normals.ContainsKey(sprite.NormalName))
                s_Normals.Add(sprite.NormalName, s_Container.ContentManager.Load<Texture2D>(sprite.NormalName));
        }

        protected override void process(Entity entity)
        {
            Position position = (Position) s_PositionMapper.get(entity);
            Sprite sprite = (Sprite) s_SpriteMapper.get(entity);
            ViewPort viewport = (ViewPort) s_ViewportMapper.get(s_Camera);
            GeometryMap geometry = (GeometryMap)s_GeometryMapper.get(s_Geometry);
            Transform transform = (Transform)s_TranformMapper.get(entity);

            Vector2 pos = position.Pos;
            Vector2 origin = viewport.getOrigin();
            Vector2 center = viewport.getDimensions() / 2;

            s_SpriteBatch.Begin();

            //s_SpriteBatch.Draw(s_Normals[sprite.NormalName], pos + center, null, Color.White, 0f, origin, new Vector2(1), SpriteEffects.None, 0f);
            if (transform != null)
            {
                s_SpriteBatch.Draw(s_Normals[sprite.NormalName], pos - origin + transform.RotationOrigin, new Rectangle(sprite.Column * sprite.Width, sprite.Row * sprite.Height, sprite.Width, sprite.Height), Color.White, transform.Rotation, transform.RotationOrigin, new Vector2(1), SpriteEffects.None, 0f);
            }
            else
            {
                s_SpriteBatch.Draw(s_Normals[sprite.NormalName], pos -origin, new Rectangle(sprite.Column * sprite.Width, sprite.Row * sprite.Height, sprite.Width, sprite.Height), Color.White, 0f, new Vector2(0), new Vector2(1), SpriteEffects.None, 0f);
            }
            s_SpriteBatch.End();
        }
    }
}
