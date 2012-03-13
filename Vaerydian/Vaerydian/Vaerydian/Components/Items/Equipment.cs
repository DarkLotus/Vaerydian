using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Items;

namespace Vaerydian.Components.Items
{
    class Equipment : IComponent
    {
        private static int e_TypeID;
        private int e_EntityID;

        public Equipment() { }

        public int getEntityId()
        {
            return e_EntityID;
        }

        public int getTypeId()
        {
            return e_TypeID;
        }

        public void setEntityId(int entityId)
        {
            e_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            e_TypeID = typeId;
        }

        private Entity a_RangedWeapon;
        /// <summary>
        /// the given entities ranged weapon entity reference
        /// </summary>
        public Entity RangedWeapon
        {
            get { return a_RangedWeapon; }
            set { a_RangedWeapon = value; }
        }

        private Entity a_Armor;
        /// <summary>
        /// the given entities armor entity reference
        /// </summary>
        public Entity Armor
        {
            get { return a_Armor; }
            set { a_Armor = value; }
        }
    }
}
