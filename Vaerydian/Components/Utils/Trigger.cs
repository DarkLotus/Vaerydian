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

namespace Vaerydian.Components.Utils
{

    public delegate void TriggerActionHandler(ECSInstance ecsInstance, Object[] parameters);

    public class Trigger : IComponent
    {
        private static int t_TypeID;
        private int t_EntityID;

        private Object[] t_Params;

        public Trigger() { }

        public Trigger(params Object[] parameters) 
        {
            t_Params = parameters;
        }

        public int getEntityId()
        {
            return t_EntityID;
        }

        public int getTypeId()
        {
            return t_TypeID;
        }

        public void setEntityId(int entityId)
        {
            t_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            t_TypeID = typeId;
        }

        private bool t_IsActive = false;
        /// <summary>
        /// will the trigger fire the next cycle?
        /// </summary>
        public bool IsActive
        {
            get { return t_IsActive; }
            set { t_IsActive = value; }
        }

        private bool t_IsRecurring = false;
        /// <summary>
        /// is the trigger allowed to fire more than once?
        /// </summary>
        public bool IsRecurring
        {
            get { return t_IsRecurring; }
            set { t_IsRecurring = value; }
        }

        private int t_ElapsedTimeRecurring = 0;
        /// <summary>
        /// elapsed time for recurring time
        /// </summary>
        public int ElapsedTimeRecurring
        {
            get { return t_ElapsedTimeRecurring; }
            set { t_ElapsedTimeRecurring = value; }
        }

        private int t_RecurrancePeriod = 0;
        /// <summary>
        /// time to recurr
        /// </summary>
        public int RecurrancePeriod
        {
            get { return t_RecurrancePeriod; }
            set { t_RecurrancePeriod = value; }
        }

        private int t_ElapsedTimeDelay = 0;
        /// <summary>
        /// elapsed time for initial trigger delay
        /// </summary>
        public int ElapsedTimeDelay
        {
            get { return t_ElapsedTimeDelay; }
            set { t_ElapsedTimeDelay = value; }
        }

        private int t_TimeDelay = 0;
        /// <summary>
        /// initial trigger delay
        /// </summary>
        public int TimeDelay
        {
            get { return t_TimeDelay; }
            set { t_TimeDelay = value; }
        }

        private bool t_HasFired = false;
        /// <summary>
        /// has the trigger fired once?
        /// </summary>
        public bool HasFired
        {
            get { return t_HasFired; }
            set { t_HasFired = value; }
        }

        private bool t_KillTriggerNow = false;
        /// <summary>
        /// kill the trigger now
        /// </summary>
        public bool KillTriggerNow
        {
            get { return t_KillTriggerNow; }
            set { t_KillTriggerNow = value; }
        }

        /// <summary>
        /// action to be triggered
        /// </summary>
        public event TriggerActionHandler TriggerAction;

        /// <summary>
        /// trigger the action if active
        /// </summary>
        /// <param name="ecsInstance">pass a copy of the ecsInstance</param>
        public void fire(ECSInstance ecsInstance)
        {
            if (TriggerAction != null)
            {
                TriggerAction(ecsInstance, t_Params);
                t_HasFired = true;
            }
        }

        public void clearAction()
        {
            TriggerAction = null;
        }
    }
}
