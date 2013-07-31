/*
 Author:
      Thomas H. Jonell <@Net_Gnome>
 
 Copyright (c) 2013 Thomas H. Jonell

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Lesser General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
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
using Vaerydian.Characters;


namespace Vaerydian.Factories
{
    public static class UtilFactory
    {
        public static ECSInstance ECSInstance;
        public static GameContainer Container;
        private static Random rand = new Random();

        /// <summary>
        /// create an attack
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <param name="attackType"></param>
        public static void createAttack(Entity attacker, Entity defender, AttackType attackType)
        {
            Entity e = ECSInstance.create();

            ECSInstance.EntityManager.addComponent(e, new Attack(attacker, defender, attackType));

            ECSInstance.refresh(e);
        }


        /// <summary>
        /// creates direct damage
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <param name="pos"></param>
        public static void createDirectDamage(int amount, DamageType type, Entity target,Position pos)
        {
            Entity e = ECSInstance.create();

            Damage damage = new Damage();

            damage.DamageAmount = amount;
            damage.Target = target;
            damage.DamageClass = DamageClass.DIRECT;
            damage.DamageType = type;
            damage.Lifespan = 500;//.5 second

            ECSInstance.EntityManager.addComponent(e, damage);
            ECSInstance.EntityManager.addComponent(e, pos);

            ECSInstance.refresh(e);
        }

        public static void createSound(String name, bool play, float volume)
        {
            Entity e = ECSInstance.create();

            Audio audio = new Audio(name,play,volume);

            ECSInstance.EntityManager.addComponent(e, audio);

            ECSInstance.refresh(e);

        }

        public static void createFireSound(IUserInterface sender, InterfaceArgs args)
        {
            Entity e = ECSInstance.create();

            Audio audio = new Audio("audio\\effects\\fire", true, 1f);

            ECSInstance.EntityManager.addComponent(e, audio);

            ECSInstance.refresh(e);
        }

        public static void createSound(String name, bool play, float volume, float pitch)
        {
            Entity e = ECSInstance.create();

            Audio audio = new Audio(name, play, volume, pitch);

            ECSInstance.EntityManager.addComponent(e, audio);

            ECSInstance.refresh(e);

        }

        /// <summary>
        /// 
        /// </summary>
        public static void createMeleeAction(Vector2 position, Vector2 heading, Transform transform, Entity owner)
        {
            Entity e = ECSInstance.create();

            Sprite sprite = new Sprite("sword", "swordnormal", 32, 32, 0, 0);

            MeleeAction action = new MeleeAction();
            action.Animation = new SpriteAnimation(9, 20);
            action.ArcDegrees = 180;
            action.Owner = owner;
            action.Lifetime = 250;
            action.Range = 32;

            ECSInstance.EntityManager.addComponent(e, new Position(position,new Vector2(16)));
            ECSInstance.EntityManager.addComponent(e, new Heading(heading));
            ECSInstance.EntityManager.addComponent(e, transform);
            ECSInstance.EntityManager.addComponent(e, sprite);
            ECSInstance.EntityManager.addComponent(e, action);

            ECSInstance.refresh(e);
        }

        /// <summary>
        /// creates a victory entity component
        /// </summary>
        public static void createVictoryAward(Entity awarder, Entity receiver, int maxAwardable)
        {
            Entity e = ECSInstance.create();

            Award victory = new Award();
            victory.AwardType = AwardType.Victory;
            victory.Awarder = awarder;
            victory.Receiver = receiver;
            victory.MaxAwardable = maxAwardable;

            ECSInstance.EntityManager.addComponent(e, victory);

            ECSInstance.refresh(e);
        }

        public static void createSkillupAward(Entity awarder, Entity receiver, SkillName skill, int maxAwardable)
        {
            Entity e = ECSInstance.create();

            Award award = new Award();
            award.AwardType = AwardType.SkillUp;
            award.Awarder = awarder;
            award.Receiver = receiver;
            award.MaxAwardable = maxAwardable;
            award.SkillName = skill;

            ECSInstance.EntityManager.addComponent(e, award);

            ECSInstance.refresh(e);
        }

        public static void createAttributeAward(Entity awarder, Entity receiver, StatType attribute, int maxAwardable)
        {
            Entity e = ECSInstance.create();

            Award award = new Award();
            award.AwardType = AwardType.Attribute;
            award.Awarder = awarder;
            award.Receiver = receiver;
            award.MaxAwardable = maxAwardable;
            award.StatType = attribute;

            ECSInstance.EntityManager.addComponent(e, award);

            ECSInstance.refresh(e);
        }

        public static void createHealthAward(Entity receiver, int maxAwardable)
        {
            Entity e = ECSInstance.create();

            Award award = new Award();
            award.AwardType = AwardType.Health;
            award.Receiver = receiver;
            award.MaxAwardable = maxAwardable;

            ECSInstance.EntityManager.addComponent(e, award);

            ECSInstance.refresh(e);
        }

		public static void createTarget(){

			Entity e = ECSInstance.create ();
			Target target = new Target ();
			target.TargetEntity = null;

			ECSInstance.EntityManager.addComponent (e, target);
			ECSInstance.EntityManager.addComponent (e, new Position(new Vector2(0),new Vector2(24)));

			Sprite sprite = new Sprite ("reticle", "reticle_normal", 48, 48, 0, 0);
			sprite.Visible = false;
			ECSInstance.EntityManager.addComponent (e, sprite);

			ECSInstance.TagManager.tagEntity("TARGET",e);

			ECSInstance.refresh (e);
		}
    }
}
