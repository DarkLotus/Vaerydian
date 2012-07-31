using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Utils;

namespace Vaerydian.Components.Actions
{
    class Interactable : IComponent
    {

        private static int i_TypeID;
        private int i_EntityID;

        public Interactable() { }

        public int getEntityId()
        {
            return i_EntityID;
        }

        public int getTypeId()
        {
            return i_TypeID;
        }

        public void setEntityId(int entityId)
        {
            i_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            i_TypeID = typeId;
        }

        private SupportedInteractions i_SupportedInteractions = new SupportedInteractions();

        public SupportedInteractions SupportedInteractions
        {
            get { return i_SupportedInteractions; }
            set { i_SupportedInteractions = value; }
        }



    }
}
