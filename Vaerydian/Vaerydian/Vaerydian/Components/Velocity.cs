using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

namespace Vaerydian.Components
{
    public class Velocity : IComponent
    {
        private static int v_TypeID;
        private static int v_EntityID;

        private float v_Velocity;

        public Velocity() { }

        public Velocity(float velocity)
        {
            v_Velocity = velocity;
        }

        public int getEntityId()
        {
            return v_EntityID;
        }

        public int getTypeId()
        {
            return v_TypeID;
        }

        public float getVelocity()
        {
            return v_Velocity;
        }

        public void setEntityId(int entityId)
        {
            v_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            v_TypeID = typeId;
        }

        public void setVelocity(float velocity)
        {
            v_Velocity = velocity;
        }
    }
}
