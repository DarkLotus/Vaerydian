using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

namespace Vaerydian.Components.Spatials
{
    public class MousePosition : IComponent
    {
        private static int m_TypeID;
        private int m_EntityID;

        public MousePosition() { }

        public int getEntityId()
        {
            return m_EntityID;
        }

        public int getTypeId()
        {
            return m_TypeID;
        }

        public void setEntityId(int entityId)
        {
            m_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            m_TypeID = typeId;
        }
    }
}
