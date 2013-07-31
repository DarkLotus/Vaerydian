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
using Vaerydian.Components.Graphical;

namespace Vaerydian.Systems.Draw
{
    class DeferredSystem : EntityProcessingSystem
    {
        private GameContainer d_Container;
        private SpriteBatch d_SpriteBatch;
        private Effect d_CombinedEffect;
        private ComponentMapper d_GeometryMapper;

        public DeferredSystem(GameContainer container) 
        {
            d_Container = container;
            d_SpriteBatch = d_Container.SpriteBatch;
        }

        public override void initialize()
        {
            d_GeometryMapper = new ComponentMapper(new GeometryMap(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            d_CombinedEffect = d_Container.ContentManager.Load<Effect>("effects\\DiferredCombine");
        }

        protected override void cleanUp(Bag<Entity> entities) { }

        protected override void process(Entity entity)
        {
            GeometryMap geometry = (GeometryMap)d_GeometryMapper.get(entity);

            //setup effect parameters and techniques
            d_CombinedEffect.CurrentTechnique = d_CombinedEffect.Techniques["Combine"];
            d_CombinedEffect.Parameters["ambient"].SetValue(1f);
            d_CombinedEffect.Parameters["lightAmbient"].SetValue(4);
            d_CombinedEffect.Parameters["ambientColor"].SetValue(geometry.AmbientColor);
            d_CombinedEffect.Parameters["ColorMap"].SetValue(geometry.ColorMap);
            d_CombinedEffect.Parameters["ShadingMap"].SetValue(geometry.ShadingMap);
            d_CombinedEffect.Parameters["NormalMap"].SetValue(geometry.NormalMap);
            d_CombinedEffect.CurrentTechnique.Passes[0].Apply();

            d_SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, d_CombinedEffect);
            d_SpriteBatch.Draw(geometry.ColorMap, Vector2.Zero, Color.White);
            d_SpriteBatch.End();
        }
    }
}
