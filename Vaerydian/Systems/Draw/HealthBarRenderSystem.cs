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

using Vaerydian;
using Vaerydian.Components;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Characters;
using Vaerydian.Components.Graphical;


namespace Vaerydian.Systems.Draw
{
    class HealthBarRenderSystem : EntityProcessingSystem
    {
        private GameContainer h_Container;

        private SpriteBatch h_SpriteBatch;

        private ComponentMapper h_PositionMapper;
        private ComponentMapper h_ViewportMapper;
        private ComponentMapper h_HealthMapper;

        private Entity h_Camera;

        private Texture2D h_Texture;


        public HealthBarRenderSystem(GameContainer container) 
        {
            h_Container = container;
            h_SpriteBatch = container.SpriteBatch;
        }

        public override void initialize()
        {
            h_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            h_ViewportMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
            h_HealthMapper = new ComponentMapper(new Health(), e_ECSInstance);
        }

        protected override void cleanUp(Bag<Entity> entities) { }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            h_Texture = h_Container.ContentManager.Load<Texture2D>("export");

            h_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
        }

        protected override void process(Entity entity)
        {
            //grab components
            Position position = (Position)h_PositionMapper.get(entity);
            ViewPort camera = (ViewPort)h_ViewportMapper.get(h_Camera);
            Health health = (Health)h_HealthMapper.get(entity);

            //get vectors for easy working
            Vector2 pos = position.Pos;
            Vector2 origin = camera.getOrigin();

            //calculate current HP percentage
            float percentage = (float)health.CurrentHealth / (float)health.MaxHealth;

            //set the maximum x-distance that we have to draw, up to 32 pixels
            int max = (int)(32 * percentage);
            
            //construct the drawing region rectangle
            Rectangle rect = new Rectangle((int)(pos.X-origin.X),(int)(pos.Y - 10 -origin.Y),max,5);

            h_SpriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone);

            //draw the health bar
            h_SpriteBatch.Draw(h_Texture, rect, Color.Red);

            h_SpriteBatch.End();
        }


    }
}
