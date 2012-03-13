using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vaerydian.Utils;

namespace Vaerydian.Items
{
    public enum WeaponType
    {
        Melee,
        Ranged
    }

    public enum MeleeWeaponType
    {
        Axe,
        Dagger,
        Mace,
        Sword,
    }

    public enum RangedWeaponType
    {
        Bow,
        Crossbow,
    }

    public class Weapon : Item
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public Weapon() { }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="lethality"></param>
        /// <param name="speed"></param>
        /// <param name="range"></param>
        /// <param name="damageType"></param>
        public Weapon(int lethality, int speed, float minRange, float maxRange, DamageType damageType)
        {
            w_Lethality = lethality;
            w_Speed = speed;
            w_RangeMin = minRange;
            w_RangeMax = maxRange;
            w_DamageType = damageType;
        }


        /// <summary>
        /// how lethal the weapon is
        /// </summary>
        private int w_Lethality;

        /// <summary>
        /// how lethal the weapon is
        /// </summary>
        public int Lethality
        {
            get { return w_Lethality; }
            set { w_Lethality = value; }
        }

        /// <summary>
        /// Min range of the weapon
        /// </summary>
        private float w_RangeMin;

        /// <summary>
        /// Min range of the weapon
        /// </summary>
        public float RangeMin
        {
            get { return w_RangeMin; }
            set { w_RangeMin = value; }
        }

        /// <summary>
        /// Max range of the weapon
        /// </summary>
        private float w_RangeMax;

        /// <summary>
        /// Max range of the weapon
        /// </summary>
        public float RangeMax
        {
            get { return w_RangeMax; }
            set { w_RangeMax = value; }
        }

        /// <summary>
        /// Speed of the weapon
        /// </summary>
        private int w_Speed;

        /// <summary>
        /// Speed of the weapon
        /// </summary>
        public int Speed
        {
            get { return w_Speed; }
            set { w_Speed = value; }
        }

        /// <summary>
        /// damage type of weapon
        /// </summary>
        private DamageType w_DamageType;

        /// <summary>
        /// damage type of weapon
        /// </summary>
        public DamageType DamageType
        {
            get { return w_DamageType; }
            set { w_DamageType = value; }
        }

    }
}
