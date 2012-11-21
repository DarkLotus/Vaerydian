using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

namespace Vaerydian.Components.Spatials
{
    public class Velocity : IComponent
    {
        private static int v_TypeID;
        private int v_EntityID;

        private float v_Velocity;

        public float Vel
        {
            get { return v_Velocity; }
            set { v_Velocity = value; }
        }

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

        public void setEntityId(int entityId)
        {
            v_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            v_TypeID = typeId;
        }
    }
}
