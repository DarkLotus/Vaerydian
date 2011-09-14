using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Items
{
    public class Equipment
    {

        private Weapon e_Weapon;

        public Weapon Weapon
        {
            get { return e_Weapon; }
            set { e_Weapon = value; }
        }

        private Armor e_ArmorChest;

        public Armor ArmorChest
        {
            get { return e_ArmorChest; }
            set { e_ArmorChest = value; }
        }


    }
}
