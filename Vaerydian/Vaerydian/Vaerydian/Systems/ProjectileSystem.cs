using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;
using ECSFramework.Utils;

using Vaerydian.Components;



namespace Vaerydian.Systems
{
    class ProjectileSystem : EntityProcessingSystem
    {
        private ComponentMapper p_ProjectileMapper;
        private ComponentMapper p_PositionMapper;
        private ComponentMapper p_VelocityMapper;
        private ComponentMapper p_HeadingMapper;
        private ComponentMapper p_ViewPortMapper;

        private Entity p_Camera;

        public ProjectileSystem() { }

        public override void initialize()
        {
            p_ProjectileMapper = new ComponentMapper(new Projectile(), e_ECSInstance);
            p_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            p_VelocityMapper = new ComponentMapper(new Velocity(), e_ECSInstance);
            p_HeadingMapper = new ComponentMapper(new Heading(), e_ECSInstance);
            p_ViewPortMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            p_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
        }

        protected override void process(Entity entity)
        {
                       
            Projectile projectile = (Projectile)p_ProjectileMapper.get(entity);

            projectile.ElapsedTime += e_ECSInstance.ElapsedTime;

            //is it time for the projectile to die?
            if (projectile.ElapsedTime > projectile.LifeTime)
            {
                e_ECSInstance.deleteEntity(entity);
                return;
            }
                
            Position position = (Position)p_PositionMapper.get(entity);
            Velocity velocity = (Velocity)p_VelocityMapper.get(entity);
            Heading heading = (Heading)p_HeadingMapper.get(entity);
            ViewPort viewport = (ViewPort)p_ViewPortMapper.get(p_Camera);

            Vector2 origin = viewport.getOrigin();
            Vector2 center = viewport.getDimensions() / 2;
            Vector2 pos = position.getPosition();

            pos += heading.getHeading() * velocity.getVelocity();

            position.setPosition(pos);
        }
    }
}
