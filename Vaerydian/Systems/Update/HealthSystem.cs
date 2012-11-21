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
                    UtilFactory uf = new UtilFactory(e_ECSInstance);
                    uf.createSound("audio\\effects\\death", true, 1f);

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
                                uf.createVictoryAward(entity, receiver, 10);
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
