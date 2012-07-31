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

        public Entity createTestMeleeWeapon()
        {
            Entity e = i_EcsInstance.create();

            Item item = new Item("TestMeleeWeapon", 0, 100);

            Weapon weapon = new Weapon(10, 5, 0, 48, WeaponType.Melee, DamageType.Common);
            weapon.MeleeWeaponType = MeleeWeaponType.Sword;

            i_EcsInstance.EntityManager.addComponent(e, item);
            i_EcsInstance.EntityManager.addComponent(e, weapon);

            i_EcsInstance.refresh(e);
            return e;
        }

        public Entity createTestRangedWeapon()
        {
            Entity e = i_EcsInstance.create();

            Item item = new Item("TestRangedWeapon", 0, 100);

            Weapon weapon = new Weapon(5, 5, 100, 300, WeaponType.Ranged, DamageType.Common);
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

            Armor armor = new Armor(5, 5);

            i_EcsInstance.EntityManager.addComponent(e, item);
            i_EcsInstance.EntityManager.addComponent(e, armor);

            i_EcsInstance.refresh(e);

            return e;
        }

        public Equipment createTestEquipment()
        {
            Equipment equipment = new Equipment();

            equipment.MeleeWeapon = createTestMeleeWeapon();
            equipment.RangedWeapon = createTestRangedWeapon();
            equipment.Armor = createTestArmor();

            return equipment;
        }

        public void destoryEquipment(Entity entity)
        {
            ComponentMapper equipMapper = new ComponentMapper(new Equipment(),i_EcsInstance);
            ComponentMapper itemMapper = new ComponentMapper(new Item(),i_EcsInstance);

            Equipment equip = (Equipment)equipMapper.get(entity);

            if (equip == null)
                return;

            //remove melee weapon
            Item meleeWeapon = (Item)itemMapper.get(equip.MeleeWeapon);
            if (meleeWeapon != null)
                i_EcsInstance.deleteEntity(equip.MeleeWeapon);


            //remove ranged weapon
            Item rangedWeapon = (Item)itemMapper.get(equip.RangedWeapon);
            if (rangedWeapon != null)
                i_EcsInstance.deleteEntity(equip.RangedWeapon);

            //remove armor
            Item armor = (Item)itemMapper.get(equip.Armor);
            if (armor != null)
                i_EcsInstance.deleteEntity(equip.Armor);


            return;
        }

    }
}
