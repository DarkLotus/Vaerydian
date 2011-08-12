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

    class Weapon : Item
    {
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

        

    }
}
