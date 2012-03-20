using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Components;
using Vaerydian.Components.Characters;
using Vaerydian.Components.Items;
using Vaerydian.Factories;

namespace Vaerydian.Systems.Update
{
    class HealthSystem : EntityProcessingSystem
    {
        private ComponentMapper h_HealthMapper;
        private ComponentMapper h_LifeMapper;
        
        public HealthSystem() { }

        public override void initialize()
        {
            h_HealthMapper = new ComponentMapper(new Health(), e_ECSInstance);
            h_LifeMapper = new ComponentMapper(new Life(), e_ECSInstance);
        }

        protected override void preLoadContent(ECSFramework.Utils.Bag<Entity> entities)
        {
            
        }

        protected override void process(Entity entity)
        {
            Health health = (Health)h_HealthMapper.get(entity);

            if (health.CurrentHealth <= 0)
            {
                

                Life life = (Life)h_LifeMapper.get(entity);

                if (life.IsAlive)
                {
                    UtilFactory uf = new UtilFactory(e_ECSInstance);
                    uf.createSound("audio\\effects\\death", true, 1f);
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
