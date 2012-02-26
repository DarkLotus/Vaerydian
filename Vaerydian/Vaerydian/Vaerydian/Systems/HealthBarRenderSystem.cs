using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ECSFramework;

using Vaerydian;
using Vaerydian.Components;


namespace Vaerydian.Systems
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

        protected override void preLoadContent(ECSFramework.Utils.Bag<Entity> entities)
        {
            h_Texture = h_Container.ContentManager.Load<Texture2D>("export");

            h_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
        }

        protected override void process(Entity entity)
        {
            Position position = (Position)h_PositionMapper.get(entity);
            ViewPort camera = (ViewPort)h_ViewportMapper.get(h_Camera);
            Health health = (Health)h_HealthMapper.get(entity);

            Vector2 pos = position.getPosition();
            Vector2 origin = camera.getOrigin();

            float percentage = (float)health.CurrentHealth / (float)health.MaxHealth;

            int max = (int)(32 * percentage);

            Rectangle rect = new Rectangle((int)(pos.X-origin.X),(int)(pos.Y - 10 -origin.Y),max,5);

            h_SpriteBatch.Begin();

            h_SpriteBatch.Draw(h_Texture, rect, Color.Red);

            h_SpriteBatch.End();
        }


    }
}
