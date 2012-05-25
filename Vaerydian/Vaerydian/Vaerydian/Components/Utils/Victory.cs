using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

namespace Vaerydian.Components.Utils
{
    class Victory : IComponent
    {
        private static int v_TypeID;
        private int v_EntityID;

        public Victory() { }

        public int getEntityId()
        {
            return v_EntityID;
        }

        public int getTypeId()
        {
            return v_TypeID;
        }

        public void setEntityId(int entityId)
        {
            v_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            v_TypeID = typeId;
        }

        private Entity v_Owner;

        public Entity Awarder
        {
            get { return v_Owner; }
            set { v_Owner = value; }
        }

        private Entity v_Receiver;

        public Entity Receiver
        {
            get { return v_Receiver; }
            set { v_Receiver = value; }
        }

        
    }
}
