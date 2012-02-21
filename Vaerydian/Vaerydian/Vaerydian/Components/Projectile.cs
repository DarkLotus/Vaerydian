using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

namespace Vaerydian.Components
{
    class Projectile : IComponent
    {
        private static int p_TypeID;
        private int p_EntityID;

        public Projectile() { }

        public Projectile(int lifetime)
        {
            p_LifeTime = lifetime;
        }

        public int getEntityId()
        {
            return p_EntityID;
        }

        public int getTypeId()
        {
            return p_TypeID;
        }

        public void setEntityId(int entityId)
        {
            p_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            p_TypeID = typeId;
        }

        private int p_LifeTime = 0;

        public int LifeTime
        {
            get { return p_LifeTime; }
            set { p_LifeTime = value; }
        }

        private int p_ElapsedTime = 0;

        public int ElapsedTime
        {
            get { return p_ElapsedTime; }
            set { p_ElapsedTime = value; }
        }
    }
}
