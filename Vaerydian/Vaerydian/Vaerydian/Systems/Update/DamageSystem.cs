using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Utils;
using Vaerydian.Components;

namespace Vaerydian.Systems.Update
{
    class DamageSystem : EntityProcessingSystem
    {
        private ComponentMapper d_DamageMapper;
        private ComponentMapper d_HealthMapper;


        public DamageSystem() { }

        public override void initialize()
        {
            d_DamageMapper = new ComponentMapper(new Damage(), e_ECSInstance);
            d_HealthMapper = new ComponentMapper(new Health(), e_ECSInstance);
        }

        protected override void preLoadContent(ECSFramework.Utils.Bag<Entity> entities)
        {
         
        }
        
        protected override void process(Entity entity)
        {
            Damage damage = (Damage)d_DamageMapper.get(entity);

            if (damage.IsActive)
            {
                switch (damage.DamageClass)
                {
                    case DamageClass.Direct:
                        handleDirectDamage(damage);
                        break;
                    case DamageClass.OverTime:
                        break;
                    case DamageClass.AreaOfEffect:
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
