using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Components.Utils
{

    public delegate void TriggerActionHandler(params object[] args);

    public class Trigger
    {
        private static int t_TypeID;
        private int t_EntityID;
        private int t_TimeDelay;

        public Trigger() { }

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

        public event TriggerActionHandler TriggerAction;

        public void doAction()
        {
            TriggerAction();
        }

        public void clearAction()
        {
            TriggerAction = null;
        }
    }
}
