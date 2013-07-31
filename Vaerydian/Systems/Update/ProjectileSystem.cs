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

using ECSFramework;


using Vaerydian.Components;
using Vaerydian.Utils;
using Vaerydian.Factories;
using Vaerydian.Components.Characters;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Actions;
using Vaerydian.Components.Utils;


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
        private ComponentMapper p_FactionMapper;
        private ComponentMapper p_LifeMapper;

        private Entity p_Spatial;


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
            p_FactionMapper = new ComponentMapper(new Factions(), e_ECSInstance);
            p_LifeMapper = new ComponentMapper(new Life(), e_ECSInstance);


        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            p_Spatial = e_ECSInstance.TagManager.getEntityByTag("SPATIAL");

        }

        protected override void cleanUp(Bag<Entity> entities) { }

        protected override void process(Entity entity)
        {
                       
            Projectile projectile = (Projectile)p_ProjectileMapper.get(entity);

            projectile.ElapsedTime += e_ECSInstance.ElapsedTime;

            //is it time for the projectile to die?
            if (projectile.ElapsedTime >= projectile.LifeTime)
            {
                e_ECSInstance.deleteEntity(entity);
                return;
            }
                
            Position position = (Position)p_PositionMapper.get(entity);
            Velocity velocity = (Velocity)p_VelocityMapper.get(entity);
            Heading heading = (Heading)p_HeadingMapper.get(entity);
            SpatialPartition spatial = (SpatialPartition)p_SpatialMapper.get(p_Spatial);

            Vector2 pos = position.Pos;

            //List<Entity> locals = spatial.QuadTree.retrieveContentsAtLocation(pos);
            List<Entity> locals = spatial.QuadTree.findAllWithinRange(pos, 32);

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

                        Life life = (Life)p_LifeMapper.get(locals[i]);

                        //if no life, uh, don't check for it...
                        if (life == null)
                            continue;

                        //if target is dead, dont worry
                        if (!life.IsAlive)
                            continue;


                        //is there an interaction available?
                        Interactable interaction = (Interactable)p_InteractionMapper.get(locals[i]);
                        if (interaction != null)
                        {
                            //get this local's position
                            Position localPosition = (Position)p_PositionMapper.get(locals[i]);

                            //are we close to it?
                            //23 - minimal radial distance for collision to occur
                            if (Vector2.Distance(pos + position.Offset, localPosition.Pos + localPosition.Offset) < 23)
                            {
                                //can we do anything to it?
                                if(interaction.SupportedInteractions.PROJECTILE_COLLIDABLE &&
                                    interaction.SupportedInteractions.ATTACKABLE)
                                {

                                    Factions lfactions = (Factions)p_FactionMapper.get(locals[i]);
                                    Factions pfactions = (Factions)p_FactionMapper.get(projectile.Originator);

                                    if (lfactions.OwnerFaction.FactionType == pfactions.OwnerFaction.FactionType)
                                        continue;

                                    //UtilFactory.createAttack(projectile.Originator, locals[i], AttackType.Projectile);
									ActionDef def = GameConfig.ActionDefs["RANGED_DMG"];
									ActionFactory.createAction(def,projectile.Originator,locals[i]);


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
                    /*
                    Vector2 norm = mapCollide.ResponseVector;
                    norm.Normalize();
                    Vector2 reflect = Vector2.Reflect(heading.getHeading(), norm);
                    reflect.Normalize();

                    //Transform trans = (Transform)p_TransformMapper.get(entity);

                    //trans.Rotation = -VectorHelper.getAngle2(new Vector2(1,0), reflect);

                    heading.setHeading(reflect);
                     */
                    
					UtilFactory.createSound("audio\\effects\\hitwall", true, 0.5f);

                    e_ECSInstance.deleteEntity(entity);
                }
            }

            pos += heading.getHeading() * velocity.Vel;

            position.Pos = pos;
        }
    }
}
