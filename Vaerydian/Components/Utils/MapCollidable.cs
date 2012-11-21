using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

namespace Vaerydian.Components.Utils
{
    class MapCollidable : IComponent
    {
        private static int m_TypeID;
        private int m_EntityID;

        public MapCollidable() { }

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

        private bool m_Collided = false;

        public bool Collided
        {
            get { return m_Collided; }
            set { m_Collided = value; }
        }

        private Vector2 m_ResponseVector;

        public Vector2 ResponseVector
        {
            get { return m_ResponseVector; }
            set { m_ResponseVector = value; }
        }


    }
}
