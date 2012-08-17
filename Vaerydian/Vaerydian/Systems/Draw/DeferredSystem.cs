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
