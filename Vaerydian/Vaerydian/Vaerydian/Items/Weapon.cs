using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    public enum DamageType
    {
        Common,
        Fire,
        Ice,
        Poison,
        Disease,
        Order,
        Chaos,
        Light,
        Dark,
        Arcane,
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
        public Weapon(int lethality, int speed, int range, DamageType damageType)
        {
            w_Lethality = lethality;
            w_Speed = speed;
            w_Range = range;
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
        /// range of the weapon
        /// </summary>
        private int w_Range;

        /// <summary>
        /// range of the weapon
        /// </summary>
        public int Range
        {
            get { return w_Range; }
            set { w_Range = value; }
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
