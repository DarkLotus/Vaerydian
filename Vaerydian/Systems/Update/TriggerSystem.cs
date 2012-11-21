using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;
using Vaerydian.Components.Utils;

namespace Vaerydian.Systems.Update
{
    class TriggerSystem : EntityProcessingSystem
    {
        private ComponentMapper t_TriggerMapper;
        

        public override void initialize()
        {
            t_TriggerMapper = new ComponentMapper(new Trigger(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            
        }

        protected override void process(Entity entity)
        {
            Trigger trigger = (Trigger)t_TriggerMapper.get(entity);

            //update delay time
            trigger.ElapsedTimeDelay += e_ECSInstance.ElapsedTime;

            //is the trigger ready to fire?
            if (trigger.ElapsedTimeDelay >= trigger.TimeDelay)
            {
                //update recurring time
                trigger.ElapsedTimeRecurring += e_ECSInstance.ElapsedTime;

                //if the trigger has not fired yet, fire it
                if (!trigger.HasFired)
                    trigger.IsActive = true;

                //should the trigger be re-activated?
                if (trigger.HasFired && trigger.IsRecurring && (trigger.ElapsedTimeRecurring >= trigger.RecurrancePeriod))
                {
                    trigger.IsActive = true;
                    trigger.ElapsedTimeRecurring = 0;
                }
            }

            //attempt to fire trigger
            if (trigger.IsActive)
            {
                trigger.fire(e_ECSInstance);
                trigger.IsActive = false;
            }

            //cleanup trigger if appropriate
            if ((trigger.HasFired && !trigger.IsRecurring) || trigger.KillTriggerNow)
            {
                trigger.clearAction();
                e_ECSInstance.deleteEntity(entity);
            }
        }

        protected override void cleanUp(Bag<Entity> entities)
        {
            
        }

    }
}
