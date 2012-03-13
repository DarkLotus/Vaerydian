using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Utils;



namespace Vaerydian.Components
{
    class Attack : IComponent
    {
        private static int a_TypeID;
        private int a_EntityID;

        public Attack() { }

        public Attack(Entity attacker, Entity defender, AttackType attackType)
        {
            a_Attacker = attacker;
            a_Defenter = defender;
            a_AttackType = attackType;
        }

        public int getEntityId()
        {
            return a_EntityID;
        }

        public int getTypeId()
        {
            return a_TypeID;
        }

        public void setEntityId(int entityId)
        {
            a_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            a_TypeID = typeId;
        }

        private Entity a_Attacker;

        public Entity Attacker
        {
            get { return a_Attacker; }
            set { a_Attacker = value; }
        }

        private Entity a_Defenter;

        public Entity Defender
        {
            get { return a_Defenter; }
            set { a_Defenter = value; }
        }

        private AttackType a_AttackType;

        public AttackType AttackType
        {
            get { return a_AttackType; }
            set { a_AttackType = value; }
        }


    }
}
