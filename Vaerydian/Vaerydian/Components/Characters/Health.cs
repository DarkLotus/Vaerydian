using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

namespace Vaerydian.Components.Characters
{
    class Health: IComponent
    {
        private static int h_TypeID;
        private int h_EntityID;

        public Health() { }

        public Health(int max)
        {
            h_MaxHealth = max;
            h_CurrentHealth = max;
        }

        public int getEntityId()
        {
            return h_EntityID;
        }

        public int getTypeId()
        {
            return h_TypeID;
        }

        public void setEntityId(int entityId)
        {
            h_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            h_TypeID = typeId;
        }

        private int h_MaxHealth;

        public int MaxHealth
        {
            get { return h_MaxHealth; }
            set { h_MaxHealth = value; }
        }

        private int h_CurrentHealth;

        public int CurrentHealth
        {
            get { return h_CurrentHealth; }
            set { h_CurrentHealth = value; }
        }

        private int h_RecoveryRate;

        public int RecoveryRate
        {
            get { return h_RecoveryRate; }
            set { h_RecoveryRate = value; }
        }

        private int h_RecoveryAmmount;

        public int RecoveryAmmount
        {
            get { return h_RecoveryAmmount; }
            set { h_RecoveryAmmount = value; }
        }

        private int h_TimeSinceLastRecover;

        public int TimeSinceLastRecover
        {
            get { return h_TimeSinceLastRecover; }
            set { h_TimeSinceLastRecover = value; }
        }
    }
}
