using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;
using ECSFramework.Utils;

using Vaerydian.Components;
using Vaerydian.Utils;
using Vaerydian.Factories;


namespace Vaerydian.Systems.Update
{
    class ProjectileSystem : EntityProcessingSystem
    {
        private ComponentMapper p_ProjectileMapper;
        private ComponentMapper p_PositionMapper;
        private ComponentMapper p_VelocityMapper;
        private ComponentMapper p_HeadingMapper;
        private ComponentMapper p_MapCollidableMapper;
        private ComponentMapper p_TransformMapper;
        private ComponentMapper p_SpatialMapper;
        private ComponentMapper p_InteractionMapper;
        private ComponentMapper p_HealthMapper;

        private Entity p_Spatial;

        private UtilFactory p_Factory;

        public ProjectileSystem() { }

        public override void initialize()
        {
            p_ProjectileMapper = new ComponentMapper(new Projectile(), e_ECSInstance);
            p_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            p_VelocityMapper = new ComponentMapper(new Velocity(), e_ECSInstance);
            p_HeadingMapper = new ComponentMapper(new Heading(), e_ECSInstance);
            p_MapCollidableMapper = new ComponentMapper(new MapCollidable(), e_ECSInstance);
            p_TransformMapper = new ComponentMapper(new Transform(), e_ECSInstance);
            p_SpatialMapper = new ComponentMapper(new SpatialPartition(), e_ECSInstance);
            p_InteractionMapper = new ComponentMapper(new Interactable(), e_ECSInstance);
            p_HealthMapper = new ComponentMapper(new Health(), e_ECSInstance);

            p_Factory = new UtilFactory(e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            p_Spatial = e_ECSInstance.TagManager.getEntityByTag("SPATIAL");

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
            SpatialPartition spatial = (SpatialPartition)p_SpatialMapper.get(p_Spatial);

            Vector2 pos = position.getPosition();

            List<Entity> locals = spatial.QuadTree.retrieveContentsAtLocation(pos);

            //anything retrieved?
            if (locals != null)
            {   //anyone aruond?
                if (locals.Count > 0)
                {
                    //for all the locals see if we should do anything
                    for (int i = 0; i < locals.Count; i++)
                    {
                        //dont interact with whom fired you
                        if (locals[i] == projectile.Originator)
                            continue;

                        //is there an interaction available?
                        Interactable interaction = (Interactable)p_InteractionMapper.get(locals[i]);
                        if (interaction != null)
                        {
                            //get this local's position
                            Position localPosition = (Position)p_PositionMapper.get(locals[i]);

                            //are we close to it?
                            //23 - minimal radial distance for collision to occur
                            if (Vector2.Distance(pos + position.getOffset(), localPosition.getPosition() + localPosition.getOffset()) < 23)
                            {
                                //can we do anything to it?
                                if(interaction.SupportedInteractions.PROJECTILE_COLLIDABLE &&
                                    interaction.SupportedInteractions.ATTACKABLE)
                                {

                                    p_Factory.createAttack(projectile.Originator, locals[i], AttackType.Projectile);


                                    //destory yourself
                                    e_ECSInstance.deleteEntity(entity);
                                    return;
                                }
                            }
                        }

                    }
                }
            }

            MapCollidable mapCollide = (MapCollidable)p_MapCollidableMapper.get(entity);

            if (mapCollide != null)
            {
                if (mapCollide.Collided)
                {
                    Vector2 norm = mapCollide.ResponseVector;
                    norm.Normalize();
                    Vector2 reflect = Vector2.Reflect(heading.getHeading(), norm);
                    reflect.Normalize();

                    //Transform trans = (Transform)p_TransformMapper.get(entity);

                    //trans.Rotation = -VectorHelper.getAngle2(new Vector2(1,0), reflect);

                    heading.setHeading(reflect);
                }
            }
            
            pos += heading.getHeading() * velocity.getVelocity();
            
            position.setPosition(pos);
        }
    }
}
