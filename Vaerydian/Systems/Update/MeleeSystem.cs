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
using Vaerydian.Factories;
using Vaerydian.Components.Actions;
using Vaerydian.Utils;
using Vaerydian.Components.Characters;
using Vaerydian.Components.Items;
using Vaerydian.Components.Spatials;

namespace Vaerydian.Systems.Update
{
    class MeleeSystem : EntityProcessingSystem
    {
        ComponentMapper m_MeleeActionMapper;
        ComponentMapper m_PositionMapper;
        ComponentMapper m_HeadingMapper;
        ComponentMapper m_SpatialMapper;
        ComponentMapper m_LifeMapper;
        ComponentMapper m_InteractionMapper;
        ComponentMapper m_FactionMapper;
        ComponentMapper m_TransformMapper;
        
        
        Entity m_Spatial;
        Entity m_Mouse;

        public MeleeSystem() { }

        public override void initialize()
        {
            m_MeleeActionMapper = new ComponentMapper(new MeleeAction(), e_ECSInstance);
            m_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            m_HeadingMapper = new ComponentMapper(new Heading(), e_ECSInstance);
            m_SpatialMapper = new ComponentMapper(new SpatialPartition(), e_ECSInstance);
            m_LifeMapper = new ComponentMapper(new Life(), e_ECSInstance);
            m_InteractionMapper = new ComponentMapper(new Interactable(), e_ECSInstance);
            m_FactionMapper = new ComponentMapper(new Factions(), e_ECSInstance);
            m_TransformMapper = new ComponentMapper(new Transform(), e_ECSInstance);

        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            m_Spatial = e_ECSInstance.TagManager.getEntityByTag("SPATIAL");
            m_Mouse = e_ECSInstance.TagManager.getEntityByTag("MOUSE");
        }

        protected override void cleanUp(Bag<Entity> entities) { }

        protected override void process(Entity entity)
        {
            MeleeAction action = (MeleeAction)m_MeleeActionMapper.get(entity);
            Position position = (Position)m_PositionMapper.get(entity);
            SpatialPartition spatial = (SpatialPartition)m_SpatialMapper.get(m_Spatial);

            action.ElapsedTime += e_ECSInstance.ElapsedTime;

            //is it time for the melee action to die?
            if (action.ElapsedTime >= action.Lifetime)
            {
                e_ECSInstance.deleteEntity(entity);
                return;
            }

            //retrieve all local entities
            //List<Entity> locals = spatial.QuadTree.retrieveContentsAtLocation(position.Pos);
            List<Entity> locals = spatial.QuadTree.findAllWithinRange(position.Pos, action.Range);

            //is the location good?
            if (locals != null)
            {
                //is there anyone here?
                if (locals.Count > 0)
                {
                    for (int i = 0; i < locals.Count; i++)
                    {
                        //dont attempt to melee owner
                        if (locals[i] == action.Owner)
                            continue;

                        if (action.HitByAction.Contains(locals[i]))
                            continue;

                        Life life = (Life)m_LifeMapper.get(locals[i]);

                        //if no life, dont bother
                        if (life == null)
                            continue;

                        //if not alive, dont bother
                        if (!life.IsAlive)
                            continue;

                        //interaction available?
                        Interactable interactions = (Interactable)m_InteractionMapper.get(locals[i]);
                        if (interactions != null)
                        {
                            Position lPosition = (Position)m_PositionMapper.get(locals[i]);

                            Position oPosition = (Position)m_PositionMapper.get(action.Owner);
                            Vector2 lToO = (oPosition.Pos + oPosition.Offset) - (lPosition.Pos + lPosition.Offset);//local to owner
                            Heading head = (Heading)m_HeadingMapper.get(entity);//get the weapon heading
                            bool facing = (Vector2.Dot(head.getHeading(), lToO) < 0);//is the weapon facing the local?

                            //if you're facing, are you within range?
                            if (facing && (Vector2.Distance(lPosition.Pos + lPosition.Offset, position.Pos + position.Offset) <= action.Range))
                            {

                                //does it support this interaction?
                                if (interactions.SupportedInteractions.MELEE_ACTIONABLE &&
                                   interactions.SupportedInteractions.ATTACKABLE)
                                {
                                    Factions lfactions = (Factions)m_FactionMapper.get(locals[i]);
                                    Factions pfactions = (Factions)m_FactionMapper.get(action.Owner);

                                    //dont attack allies
                                    if (lfactions.OwnerFaction.FactionType == pfactions.OwnerFaction.FactionType)
                                        continue;

                                    //add to hit-list so we dont attack it again on swing follow-through
                                    action.HitByAction.Add(locals[i]);

                                    //create melee attack
                                    UtilFactory.createAttack(action.Owner, locals[i], AttackType.Melee);

                                    //destroy melee action
                                    //e_ECSInstance.deleteEntity(entity);
                                    //return;
                                }
                            }
                        }
                    }
                }
            }

            //get info for rotation update
            Heading heading = (Heading)m_HeadingMapper.get(entity);
            Transform transform = (Transform)m_TransformMapper.get(entity);
            
            //rotate melee by degrees over the melee arc
            float rot = (((float)action.Animation.updateFrame(e_ECSInstance.ElapsedTime) / (float)action.Animation.Frames) * action.ArcDegrees) - (action.ArcDegrees/2f);
            transform.Rotation = rot * (((float)Math.PI) / 180f) - VectorHelper.getAngle(new Vector2(1, 0), heading.getHeading());

            //adjust the arc based on current position (i.e., move with the player)
            Position ownerPos = (Position)m_PositionMapper.get(action.Owner);
            Vector2 pos = ownerPos.Pos + new Vector2(16, 0);// +ownerPos.getOffset();
            Vector2 dir = heading.getHeading();
            dir.Normalize();
            position.Pos = pos + dir * 10;
            
        }
    }
}
