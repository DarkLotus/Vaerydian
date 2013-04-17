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

        private UtilFactory d_UtilFactory;

        public DamageSystem() { }

        public override void initialize()
        {
            d_DamageMapper = new ComponentMapper(new Damage(), e_ECSInstance);
            d_HealthMapper = new ComponentMapper(new Health(), e_ECSInstance);
            d_AttributeMapper = new ComponentMapper(new Statistics(), e_ECSInstance);
            d_InteractMapper = new ComponentMapper(new Interactable(), e_ECSInstance);

            d_UtilFactory = new UtilFactory(e_ECSInstance);;
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
                        int endurance = ((Statistics)d_AttributeMapper.get(damage.Target)).StatisticSet[StatType.Endurance];

                        if (health.MaxHealth < (endurance * 5))
                        {

                            if (d_Rand.NextDouble() <= ((double)(endurance*5) - (double)health.MaxHealth)/(double)(endurance*5))
                            {
                                d_UtilFactory.createHealthAward(damage.Target, 1);
                            }
                        }
                    }
                    

					//FIX
                    switch (d_Rand.Next(0, 7))
                    {
                        case 1:
                            d_UtilFactory.createSound("audio\\effects\\hurt", true, 1f);
                            break;
                        case 3:
                            d_UtilFactory.createSound("audio\\effects\\hurt2", true, 1f);
                            break;
                        case 5:
                            d_UtilFactory.createSound("audio\\effects\\hurt3", true, 1f);
                            break;
                        case 7:
                            d_UtilFactory.createSound("audio\\effects\\hurt4", true, 1f);
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
