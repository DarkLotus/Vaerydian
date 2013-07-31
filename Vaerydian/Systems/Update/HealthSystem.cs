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

using ECSFramework;

using Vaerydian.Components;
using Vaerydian.Components.Characters;
using Vaerydian.Components.Items;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Actions;
using Vaerydian.Factories;

namespace Vaerydian.Systems.Update
{
    class HealthSystem : EntityProcessingSystem
    {
        private ComponentMapper h_HealthMapper;
        private ComponentMapper h_LifeMapper;
        private ComponentMapper h_AggroMapper;
        private ComponentMapper h_InteractionMapper;
        
        public HealthSystem() { }

        public override void initialize()
        {
            h_HealthMapper = new ComponentMapper(new Health(), e_ECSInstance);
            h_LifeMapper = new ComponentMapper(new Life(), e_ECSInstance);
            h_AggroMapper = new ComponentMapper(new Aggrivation(), e_ECSInstance);
            h_InteractionMapper = new ComponentMapper(new Interactable(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            
        }

        protected override void cleanUp(Bag<Entity> entities) { }

        protected override void process(Entity entity)
        {
            Health health = (Health)h_HealthMapper.get(entity);

            if (health.CurrentHealth <= 0)
            {
                

                Life life = (Life)h_LifeMapper.get(entity);

                if (life.IsAlive)
                {
                    
					UtilFactory.createSound("audio\\effects\\death", true, 1f);

                    //issue victory
                    Aggrivation aggro = (Aggrivation)h_AggroMapper.get(entity);

                    if (aggro != null)
                    {
                        foreach (Entity receiver in aggro.HateList)
                        {
                            Interactable interactor = (Interactable)h_InteractionMapper.get(entity);
                            Interactable interactee = (Interactable)h_InteractionMapper.get(receiver);

                            if (interactor == null || interactee == null)
                                continue;
                            
                            if(interactor.SupportedInteractions.AWARDS_VICTORY &&
                               interactee.SupportedInteractions.MAY_RECEIVE_VICTORY)
								UtilFactory.createVictoryAward(entity, receiver, 10);
                        }
                    }

                }
                
                life.IsAlive = false;


                return;
            }

            health.TimeSinceLastRecover += e_ECSInstance.ElapsedTime;

            if (health.TimeSinceLastRecover > health.RecoveryRate)
            {
                health.TimeSinceLastRecover = 0;
                
                health.CurrentHealth += health.RecoveryAmmount;
                
                if (health.CurrentHealth > health.MaxHealth)
                    health.CurrentHealth = health.MaxHealth;
            }

        }


    }
}
