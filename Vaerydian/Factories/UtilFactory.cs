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
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Actions;
using Vaerydian.Components.Graphical;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Characters;

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


        /// <summary>
        /// creates direct damage
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <param name="pos"></param>
        public void createDirectDamage(int amount, DamageType_Old type, Entity target,Position pos)
        {
            Entity e = u_EcsInstance.create();

            Damage damage = new Damage();

            damage.DamageAmount = amount;
            damage.Target = target;
            damage.DamageClass = DamageClass.Direct;
            damage.DamageType = type;
            damage.Lifespan = 500;//.5 second

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

            Sprite sprite = new Sprite("sword", "swordnormal", 32, 32, 0, 0);

            MeleeAction action = new MeleeAction();
            action.Animation = new SpriteAnimation(9, 20);
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

        /// <summary>
        /// creates a victory entity component
        /// </summary>
        public void createVictoryAward(Entity awarder, Entity receiver, int maxAwardable)
        {
            Entity e = u_EcsInstance.create();

            Award victory = new Award();
            victory.AwardType = AwardType.Victory;
            victory.Awarder = awarder;
            victory.Receiver = receiver;
            victory.MaxAwardable = maxAwardable;

            u_EcsInstance.EntityManager.addComponent(e, victory);

            u_EcsInstance.refresh(e);
        }

        public void createSkillupAward(Entity awarder, Entity receiver, SkillName skill, int maxAwardable)
        {
            Entity e = u_EcsInstance.create();

            Award award = new Award();
            award.AwardType = AwardType.SkillUp;
            award.Awarder = awarder;
            award.Receiver = receiver;
            award.MaxAwardable = maxAwardable;
            award.SkillName = skill;

            u_EcsInstance.EntityManager.addComponent(e, award);

            u_EcsInstance.refresh(e);
        }

        public void createAttributeAward(Entity awarder, Entity receiver, AttributeType attribute, int maxAwardable)
        {
            Entity e = u_EcsInstance.create();

            Award award = new Award();
            award.AwardType = AwardType.Attribute;
            award.Awarder = awarder;
            award.Receiver = receiver;
            award.MaxAwardable = maxAwardable;
            award.AttributeType = attribute;

            u_EcsInstance.EntityManager.addComponent(e, award);

            u_EcsInstance.refresh(e);
        }

        public void createHealthAward(Entity receiver, int maxAwardable)
        {
            Entity e = u_EcsInstance.create();

            Award award = new Award();
            award.AwardType = AwardType.Health;
            award.Receiver = receiver;
            award.MaxAwardable = maxAwardable;

            u_EcsInstance.EntityManager.addComponent(e, award);

            u_EcsInstance.refresh(e);
        }

    }
}
