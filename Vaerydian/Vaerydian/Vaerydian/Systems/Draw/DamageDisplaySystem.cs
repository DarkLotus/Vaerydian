using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ECSFramework;

using Vaerydian;
using Vaerydian.Components;

namespace Vaerydian.Systems.Draw
{
    public class DamageDisplaySystem : EntityProcessingSystem
    {
        private GameContainer d_Container;
        private SpriteBatch d_SpriteBatch;

        private ComponentMapper d_DamageMapper;
        private ComponentMapper d_PositionMapper;
        private ComponentMapper d_ViewPortMapper;

        private SpriteFont d_Font;

        private Entity d_Camera;

        public DamageDisplaySystem(GameContainer container)
        {
            d_Container = container;
            d_SpriteBatch = container.SpriteBatch;
        }


        public override void initialize()
        {
            d_DamageMapper = new ComponentMapper(new Damage(), e_ECSInstance);
            d_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            d_ViewPortMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
        }
        
        protected override void preLoadContent(ECSFramework.Utils.Bag<Entity> entities)
        {
            d_Font = FontManager.Instance.Fonts["Damage"];
            d_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
        }

        protected override void process(Entity entity)
        {
            Damage damage = (Damage)d_DamageMapper.get(entity);
            Position position = (Position)d_PositionMapper.get(entity);//damage.Target);

            if (position == null || damage == null)
                return;

            ViewPort camera = (ViewPort)d_ViewPortMapper.get(d_Camera);
            Vector2 origin = camera.getOrigin();
            Vector2 pos = position.getPosition() + new Vector2(0, -damage.Lifetime/10);

            String dmg;
            Color color = Color.Yellow;
            
            if (damage.DamageAmount == 0)
            {
                dmg = "miss";
                color = Color.White;
            }
            else
                dmg = "" + damage.DamageAmount;

            d_SpriteBatch.Begin();
            
            //background
            d_SpriteBatch.DrawString(d_Font, dmg, pos - origin + new Vector2(1, 0), Color.Black);
            d_SpriteBatch.DrawString(d_Font, dmg, pos - origin + new Vector2(-1, 0), Color.Black);
            d_SpriteBatch.DrawString(d_Font, dmg, pos - origin + new Vector2(0, 1), Color.Black);
            d_SpriteBatch.DrawString(d_Font, dmg, pos - origin + new Vector2(0, -1), Color.Black);
            //foreground
            d_SpriteBatch.DrawString(d_Font, dmg, pos - origin, color);

            d_SpriteBatch.End();
        }


    }
}
