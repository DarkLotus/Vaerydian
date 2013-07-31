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
