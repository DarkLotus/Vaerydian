using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ECSFramework;

using Vaerydian;
using Vaerydian.Components;

using Glimpse.Input;
using Glimpse.Managers;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Actions;
using Vaerydian.Components.Graphical;

namespace Vaerydian.Systems.Draw
{
    public class FloatingTextDisplaySystem : EntityProcessingSystem
    {
        private GameContainer d_Container;
        private SpriteBatch d_SpriteBatch;

        private ComponentMapper d_DamageMapper;
        private ComponentMapper d_PositionMapper;
        private ComponentMapper d_ViewPortMapper;
        private ComponentMapper d_FloatMapper;

        private SpriteFont d_Font;

        private Entity d_Camera;

        public FloatingTextDisplaySystem(GameContainer container)
        {
            d_Container = container;
            d_SpriteBatch = container.SpriteBatch;
        }


        public override void initialize()   
        {
            d_DamageMapper = new ComponentMapper(new Damage(), e_ECSInstance);
            d_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            d_ViewPortMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
            d_FloatMapper = new ComponentMapper(new FloatingText(), e_ECSInstance);
        }
        
        protected override void preLoadContent(Bag<Entity> entities)
        {
            d_Font = FontManager.Fonts["Damage"];
            d_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
        }

        protected override void process(Entity entity)
        {
            //Damage damage = (Damage)d_DamageMapper.get(entity);
            FloatingText text = (FloatingText)d_FloatMapper.get(entity);

            if (text == null)
                return;
            else
            {
                //see if we should destroy this
                text.ElapsedTime += e_ECSInstance.ElapsedTime;
                if (text.ElapsedTime >= text.Lifetime)
                {
                    e_ECSInstance.deleteEntity(entity);
                    return;
                }
            }

            
            Position position = (Position)d_PositionMapper.get(entity);//damage.Target);

            if (position == null || text == null)
                return;

            ViewPort camera = (ViewPort)d_ViewPortMapper.get(d_Camera);
            Vector2 origin = camera.getOrigin();
            Vector2 pos = position.Pos + new Vector2(0, -text.ElapsedTime / 7);

            //String dmg;
            //Color color = Color.Yellow;
            
            /*
            if (damage.DamageAmount == 0)
            {
                dmg = "miss";
                color = Color.White;
            }
            else
                dmg = "" + damage.DamageAmount;
            */

            float fade = 1f;
            float half = (float)text.Lifetime / 2f;

            if (text.ElapsedTime > half)
                fade = (1f - (text.ElapsedTime - half) / half);
            

            d_SpriteBatch.Begin();
            
            //background
            d_SpriteBatch.DrawString(d_Font, text.Text, pos - origin + new Vector2(1, 0), Color.Black * fade);
            d_SpriteBatch.DrawString(d_Font, text.Text, pos - origin + new Vector2(-1, 0), Color.Black * fade);
            d_SpriteBatch.DrawString(d_Font, text.Text, pos - origin + new Vector2(0, 1), Color.Black * fade);
            d_SpriteBatch.DrawString(d_Font, text.Text, pos - origin + new Vector2(0, -1), Color.Black * fade);
            //foreground
            d_SpriteBatch.DrawString(d_Font, text.Text, pos - origin, text.Color * fade);

            d_SpriteBatch.End();
        }


    }
}
