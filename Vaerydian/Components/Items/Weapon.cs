using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;
using Vaerydian.Utils;

namespace Vaerydian.Components.Items
{
    class Weapon : IComponent
    {
        private static int w_TypeID;
        private int w_EntityID;

        public Weapon() { }

        /// <summary>
        /// weapon constructor
        /// </summary>
        /// <param name="lethality">weapon's lethality</param>
        /// <param name="min">weapon's min range</param>
        /// <param name="max">weapon's max range</param>
        /// <param name="weaponType">type of weapon</param>
        /// <param name="damageType">weapon's damage type</param>
        public Weapon(int lethality, int speed, float min, float max, WeaponType weaponType, DamageType_Old damageType)
        {
            w_Lethality = lethality;
            w_MinRange = min;
            w_MaxRange = max;
            w_DamageType = damageType;
        }

        public int getEntityId()
        {
            return w_EntityID;
        }

        public int getTypeId()
        {
            return w_TypeID;
        }

        public void setEntityId(int entityId)
        {
            w_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            w_TypeID = typeId;
        }

        private int w_Lethality;
        /// <summary>
        /// how lethal the weapon is
        /// </summary>
        public int Lethality
        {
            get { return w_Lethality; }
            set { w_Lethality = value; }
        }

        private int w_Speed;
        /// <summary>
        /// weapon speed
        /// </summary>
        public int Speed
        {
            get { return w_Speed; }
            set { w_Speed = value; }
        }
        
        private float w_MinRange;
        /// <summary>
        /// Min range of the weapon
        /// </summary>
        public float MinRange
        {
            get { return w_MinRange; }
            set { w_MinRange = value; }
        }

        private float w_MaxRange;
        /// <summary>
        /// Max range of the weapon
        /// </summary>
        public float MaxRange
        {
            get { return w_MaxRange; }
            set { w_MaxRange = value; }
        }

        private DamageType_Old w_DamageType;

        /// <summary>
        /// damage type of weapon
        /// </summary>
        public DamageType_Old DamageType
        {
            get { return w_DamageType; }
            set { w_DamageType = value; }
        }

        private WeaponType w_WeaponType;
        /// <summary>
        /// type of weapon
        /// </summary>
        public WeaponType WeaponType
        {
            get { return w_WeaponType; }
            set { w_WeaponType = value; }
        }

        private MeleeWeaponType w_MeleeWeaponType;
        /// <summary>
        /// type of melee weapon
        /// </summary>
        public MeleeWeaponType MeleeWeaponType
        {
            get { return w_MeleeWeaponType; }
            set { w_MeleeWeaponType = value; }
        }

        private RangedWeaponType w_RangedWeaponType;
        /// <summary>
        /// type of ranged weapon
        /// </summary>
        public RangedWeaponType RangedWeaponType
        {
            get { return w_RangedWeaponType; }
            set { w_RangedWeaponType = value; }
        }
    }
}
