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

using Vaerydian.Utils;
using Vaerydian.Components;
using Vaerydian.Factories;
using Vaerydian.Components.Actions;
using Vaerydian.Components.Characters;
using Vaerydian.Characters;

namespace Vaerydian.Systems.Update
{
    class DamageSystem : EntityProcessingSystem
    {
        private ComponentMapper d_DamageMapper;
        private ComponentMapper d_HealthMapper;
        private ComponentMapper d_AttributeMapper;
        private ComponentMapper d_InteractMapper;

        private Random d_Rand = new Random();

        public DamageSystem() { }

        public override void initialize()
        {
            d_DamageMapper = new ComponentMapper(new Damage(), e_ECSInstance);
            d_HealthMapper = new ComponentMapper(new Health(), e_ECSInstance);
            d_AttributeMapper = new ComponentMapper(new Statistics(), e_ECSInstance);
            d_InteractMapper = new ComponentMapper(new Interactable(), e_ECSInstance);

        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
         
        }

        protected override void cleanUp(Bag<Entity> entities) { }
        
        protected override void process(Entity entity)
        {
            Damage damage = (Damage)d_DamageMapper.get(entity);

            if (damage.IsActive)
            {
                switch (damage.DamageClass)
                {
                    case DamageClass.DIRECT:
                        handleDirectDamage(damage);
                        break;
                    case DamageClass.OVERTIME:
                        break;
                    case DamageClass.AREA:
                        break;
                    default:
                        break;
                }
            }

            damage.Lifetime += e_ECSInstance.ElapsedTime;

            if (damage.Lifetime > damage.Lifespan)
            {
                e_ECSInstance.deleteEntity(entity);
                return;
            }
        }

        private void handleDirectDamage(Damage damage)
        {
            Health health = (Health)d_HealthMapper.get(damage.Target);

            if (health != null)
            {
                //damage target
                health.CurrentHealth -= damage.DamageAmount;

                if (damage.DamageAmount > 0)
                {
                    if (((Interactable)d_InteractMapper.get(damage.Target)).SupportedInteractions.MAY_ADVANCE)
                    {
                        int endurance = ((Statistics)d_AttributeMapper.get(damage.Target)).Endurance.Value;

                        if (health.MaxHealth < (endurance * 5))
                        {

                            if (d_Rand.NextDouble() <= ((double)(endurance*5) - (double)health.MaxHealth)/(double)(endurance*5))
                            {
                                UtilFactory.createHealthAward(damage.Target, 1);
                            }
                        }
                    }
                    

					//FIX
                    switch (d_Rand.Next(0, 7))
                    {
                        case 1:
                            UtilFactory.createSound("audio\\effects\\hurt", true, 1f);
                            break;
                        case 3:
                            UtilFactory.createSound("audio\\effects\\hurt2", true, 1f);
                            break;
                        case 5:
                            UtilFactory.createSound("audio\\effects\\hurt3", true, 1f);
                            break;
                        case 7:
                            UtilFactory.createSound("audio\\effects\\hurt4", true, 1f);
                            break;
                        default:
                            break;
                    }
                    
                    
                }
            }

            damage.IsActive = false;
        }

        private void handleDamageOverTime(Damage damage)
        {
        }

        private void handleAreaOfEffect(Damage damage)
        {
        }


    }
}
