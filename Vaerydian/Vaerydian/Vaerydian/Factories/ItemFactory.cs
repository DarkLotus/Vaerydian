using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Components;
using Vaerydian.Components.Items;
using Vaerydian.Utils;

namespace Vaerydian.Factories
{
    class ItemFactory
    {
        private ECSInstance i_EcsInstance;
        private static GameContainer i_Container;
        private Random rand = new Random();

        public ItemFactory(ECSInstance ecsInstance, GameContainer container)
        {
            i_EcsInstance = ecsInstance;
            i_Container = container;
        }

        public ItemFactory(ECSInstance ecsInstance) 
        {
            i_EcsInstance = ecsInstance;
        }


        public Entity createTestRangedWeapon()
        {
            Entity e = i_EcsInstance.create();

            Item item = new Item("TestRangedWeapon", 0, 100);

            Weapon weapon = new Weapon(5, 5, 0, 1f, WeaponType.Ranged, DamageType.Common);
            weapon.RangedWeaponType = RangedWeaponType.Blaster;

            i_EcsInstance.EntityManager.addComponent(e, item);
            i_EcsInstance.EntityManager.addComponent(e, weapon);

            i_EcsInstance.refresh(e);

            return e;
        }

        public Entity createTestArmor()
        {
            Entity e = i_EcsInstance.create();

            Item item = new Item("TestArmor", 0, 100);

            Armor armor = new Armor(4, 5);

            i_EcsInstance.EntityManager.addComponent(e, item);
            i_EcsInstance.EntityManager.addComponent(e, armor);

            i_EcsInstance.refresh(e);

            return e;
        }

        public Equipment createTestEquipment()
        {
            Equipment equipment = new Equipment();

            equipment.RangedWeapon = createTestRangedWeapon();
            equipment.Armor = createTestArmor();

            return equipment;
        }

    }
}
