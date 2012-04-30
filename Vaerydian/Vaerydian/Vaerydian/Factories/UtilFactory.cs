using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Utils;
using Vaerydian.Components;
using Vaerydian.Components.Audio;
using Glimpse.Controls;
using Microsoft.Xna.Framework;
using Vaerydian.Components.Actions;

namespace Vaerydian.Factories
{
    public class UtilFactory
    {
        private ECSInstance u_EcsInstance;
        private static GameContainer u_Container;
        private Random rand = new Random();

        public UtilFactory(ECSInstance ecsInstance, GameContainer container)
        {
            u_EcsInstance = ecsInstance;
            u_Container = container;
        }

        public UtilFactory(ECSInstance ecsInstance) 
        {
            u_EcsInstance = ecsInstance;
        }

        /// <summary>
        /// create an attack
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <param name="attackType"></param>
        public void createAttack(Entity attacker, Entity defender, AttackType attackType)
        {
            Entity e = u_EcsInstance.create();

            u_EcsInstance.EntityManager.addComponent(e, new Attack(attacker, defender, attackType));

            u_EcsInstance.refresh(e);
        }


        public void createDirectDamage(int amount, DamageType type, Entity target,Position pos)
        {
            Entity e = u_EcsInstance.create();

            Damage damage = new Damage();

            damage.DamageAmount = amount;
            damage.Target = target;
            damage.DamageClass = DamageClass.Direct;
            damage.DamageType = type;
            damage.Lifespan = 500;//1 second

            u_EcsInstance.EntityManager.addComponent(e, damage);
            u_EcsInstance.EntityManager.addComponent(e, pos);

            u_EcsInstance.refresh(e);
        }

        public void createSound(String name, bool play, float volume)
        {
            Entity e = u_EcsInstance.create();

            Audio audio = new Audio(name,play,volume);

            u_EcsInstance.EntityManager.addComponent(e, audio);

            u_EcsInstance.refresh(e);

        }

        public void createFireSound(IUserInterface sender, InterfaceArgs args)
        {
            Entity e = u_EcsInstance.create();

            Audio audio = new Audio("audio\\effects\\fire", true, 1f);

            u_EcsInstance.EntityManager.addComponent(e, audio);

            u_EcsInstance.refresh(e);
        }

        public void createSound(String name, bool play, float volume, float pitch)
        {
            Entity e = u_EcsInstance.create();

            Audio audio = new Audio(name, play, volume, pitch);

            u_EcsInstance.EntityManager.addComponent(e, audio);

            u_EcsInstance.refresh(e);

        }

        /// <summary>
        /// 
        /// </summary>
        public void createMeleeAction(Vector2 position, Vector2 heading, Transform transform, Entity owner)
        {
            Entity e = u_EcsInstance.create();

            Sprite sprite = new Sprite("sword", "sword", 32, 32, 0, 0);

            MeleeAction action = new MeleeAction();
            action.Animation = new Animation(9, 20);
            action.ArcDegrees = 180;
            action.Owner = owner;
            action.Lifetime = 250;
            action.Range = 32;

            u_EcsInstance.EntityManager.addComponent(e, new Position(position,new Vector2(16)));
            u_EcsInstance.EntityManager.addComponent(e, new Heading(heading));
            u_EcsInstance.EntityManager.addComponent(e, transform);
            u_EcsInstance.EntityManager.addComponent(e, sprite);
            u_EcsInstance.EntityManager.addComponent(e, action);

            u_EcsInstance.refresh(e);
        }

    }
}
