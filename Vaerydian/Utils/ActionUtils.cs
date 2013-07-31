/*
 Author:
      Thomas H. Jonell <thjonell AT gmail DOT com>
 
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
using ECSFramework;
using Vaerydian.Components.Actions;
using Vaerydian.Utils;
using Vaerydian.Factories;
using Vaerydian.Components.Spatials;
using Microsoft.Xna.Framework;
using Vaerydian.Characters;
using Vaerydian.Components.Characters;
using Vaerydian.Components.Items;

namespace Vaerydian.Utils
{
	public struct ActionDef{
		public string Name;
		public ActionType ActionType;
		public ImpactType ImpactType;
		public DamageDef DamageDef;
		public ModifyType ModifyType;
		public ModifyDuration ModifyDurtion;
		public CreateType CreateType;
		public DestoryType DestoryType;
	}
	
	public enum ActionType{
		NONE = 0,
		DAMAGE = 1,
		MODIFY = 2,
		CREATE = 3,
		DESTROY = 4,
		INTERACT = 5
	}
	
	public enum ImpactType{
		NONE = 0,
		DIRECT = 1,
		AREA = 2,
		CONE = 3,
		OVERTIME = 4	
	}
	
	public enum ModifyType{
		NONE = 0,
		SKILL = 1,
		ITEM = 2,
		MECHANIC = 3,
		ATTRIBUTE = 4,
		KNOWLEDGE = 5,
	}
	
	public enum ModifyDuration{
		NONE = 0,
		TEMPORARY = 1,
		PERMANENT = 2,
		LOCATION = 3
	}
	
	public enum CreateType{
		NONE = 0,
		OBJECT = 1,
		CHARACTER = 2,
		ITEM = 3,
		FEATURE = 4
	}
	
	public enum DestoryType{
		NONE = 0,
		OBJECT = 1,
		CHARACTER = 2,
		ITEM = 3,
		FEATURE = 4
	}

	struct ActionPackage{
		public Entity Owner;
		public Entity Target;
		public ActionDef ActionDef;
	}

	public static class ActionUtils
	{
		private static Random rand = new Random();

		public static void doAction (Entity owner, Entity target, ActionDef aDef)
		{
			ActionPackage aPack;
			aPack.Owner = owner;
			aPack.Target = target;
			aPack.ActionDef = aDef;

			switch (aPack.ActionDef.ActionType) {
			case ActionType.DAMAGE:
				doDamageAction(aPack);
				break;
			case ActionType.MODIFY:
				break;
			case ActionType.CREATE:
				break;
			case ActionType.DESTROY:
				break;
			case ActionType.INTERACT:
				break;
			default:
				break;
			}
		}

		private static void doDamageAction(ActionPackage aPack){

			Aggrivation aggro = ComponentMapper.get<Aggrivation> (aPack.Target);
			//check aggro and add if not present
			if(aggro != null)
				if (!aggro.HateList.Contains(aPack.Owner))
				    aggro.HateList.Add(aPack.Owner);

			switch (aPack.ActionDef.DamageDef.DamageBasis) {
			case DamageBasis.ATTRIBUTE:
				break;
			case DamageBasis.ITEM:
				break;
			case DamageBasis.NONE:
				break;
			case DamageBasis.SKILL:
				break;
			case DamageBasis.STATIC:
				doStaticDamage(aPack);
				break;
			case DamageBasis.WEAPON:
				doWeaponDamage(aPack);
				break;
			default:
				return;
			}
		}

		private static void doModifyAction(ActionPackage aPack){
			switch (aPack.ActionDef.ModifyType) {
			case ModifyType.ATTRIBUTE:
				break;
			case ModifyType.ITEM:
				break;
			case ModifyType.KNOWLEDGE:
				break;
			case ModifyType.MECHANIC:
				break;
			case ModifyType.NONE:
				break;
			case ModifyType.SKILL:
				break;
			default:
				return;
			}
		}

		private static void doCreateAction(ActionPackage aPack){
		}

		private static void doDestroyAction(ActionPackage aPack){
		}

		private static void doInteractionAction(ActionPackage aPack){
		}

		public static float getStatProbability(float skillValue, float attributeValue, float knowledgeValue, float speedValue){
			return skillValue / 4f + attributeValue / 4f + knowledgeValue + speedValue;
		}

		public static float getHitProbability(float defender, float attacker, float max, float min){
			return (attacker/(defender+attacker))*max + (defender/(defender+attacker))*min;
		}

		public static int getDamage(float hitProb, float attackSkill, float attackAttribute,
			                              float lethality, float mitigation, float absorbValue){

			float hit = (float)rand.NextDouble ();

			int damage = 0;

			if (hit < hitProb) {
				float overhit = 0f;

				if(hitProb > 1f)
					overhit = hitProb - 1f;

				int maxdmg = (int)((overhit + 1f) * (attackSkill/5 + attackAttribute/4) * (lethality/mitigation) - absorbValue/10);

				damage = rand.Next(maxdmg/2,maxdmg);

				if(damage<0)
					damage = 0;
			}


			return damage;
		}

		private static float getSkill(Entity entity, SkillName skillname){
			Skills skills = ComponentMapper.get<Skills> (entity);

			switch (skillname) {
			case SkillName.AVOIDANCE:
				return skills.Avoidance.Value;
			case SkillName.MELEE:
				return skills.Melee.Value;
			case SkillName.RANGED:
				return skills.Ranged.Value;
			default:
				return default(Skill).Value;
			}
		}

		private static float getStat(Entity entity, StatType stat){
			Statistics stats = ComponentMapper.get<Statistics> (entity);

			switch (stat) {
			case StatType.ENDURANCE:
				return stats.Endurance.Value;
			case StatType.FOCUS:
				return stats.Focus.Value;
			case StatType.MIND:
				return stats.Mind.Value;
			case StatType.MUSCLE:
				return stats.Muscle.Value;
			case StatType.PERCEPTION:
				return stats.Perception.Value;
			case StatType.PERSONALITY:
				return stats.Personality.Value;
			case StatType.QUICKNESS:
				return stats.Quickness.Value;
			default:
				return default(Statistic).Value;
			}
		}

		private static Weapon getWeapon(Entity entity){
			Equipment equip = ComponentMapper.get<Equipment> (entity);
			return ComponentMapper.get<Weapon> (equip.RangedWeapon);
		}

		private static Armor getArmor(Entity entity){
			Equipment equip = ComponentMapper.get<Equipment> (entity);
			return ComponentMapper.get<Armor> (equip.Armor);
		}

		private static float getKnowledge(Entity entity, Entity target){
			Information info = ComponentMapper.get<Information> (target);
			return ComponentMapper.get<Knowledges> (entity).GeneralKnowledge [info.CreatureGeneralGroup].Value;
		}

		private static StatType getOppositeStat(StatType stat){
			switch (stat) {
			case StatType.ENDURANCE:
				return StatType.MUSCLE;
			case StatType.FOCUS:
				return StatType.PERSONALITY;
			case StatType.MIND:
				return StatType.MIND;
			case StatType.MUSCLE:
				return StatType.ENDURANCE;
			case StatType.PERCEPTION:
				return StatType.QUICKNESS;
			case StatType.PERSONALITY:
				return StatType.FOCUS;
			case StatType.QUICKNESS:
				return StatType.FOCUS;
			case StatType.NONE:
				return StatType.NONE;
			default:
				return StatType.NONE;
			}
		}

		private static SkillName getOppositeSkill(SkillName skill){
			return SkillName.NONE;
		}

		private static void doWeaponDamage(ActionPackage aPack){
			Weapon aWeapon = ActionUtils.getWeapon (aPack.Owner);
			Armor dArmor = ActionUtils.getArmor(aPack.Target);

			float aSkill = ActionUtils.getSkill (aPack.Owner, aPack.ActionDef.DamageDef.SkillName);
			float dSkill = ActionUtils.getSkill (aPack.Target, SkillName.AVOIDANCE);

			float aStat = ActionUtils.getStat (aPack.Owner, aPack.ActionDef.DamageDef.StatType);
			float dStat = ActionUtils.getStat (aPack.Target, ActionUtils.getOppositeStat (aPack.ActionDef.DamageDef.StatType));

			float aKnow = ActionUtils.getKnowledge (aPack.Owner, aPack.Target);
			float dKnow = ActionUtils.getKnowledge (aPack.Target, aPack.Owner);

			float aProb = ActionUtils.getStatProbability ((float)aSkill, (float)aStat, aKnow, (float)aWeapon.Speed);
			float dProb = ActionUtils.getStatProbability ((float)dSkill, (float)dStat, dKnow, (float)dArmor.Mobility);

			float hitProb = ActionUtils.getHitProbability (dProb, aProb, 1.75f, 0.15f);

			int damage = ActionUtils.getDamage (hitProb, aSkill, aStat, aWeapon.Lethality, dArmor.Mitigation,
			                                    ActionUtils.getStat(aPack.Target,StatType.ENDURANCE));

			Position pos = ComponentMapper.get<Position> (aPack.Target);

			UtilFactory.createDirectDamage (damage,
			                               aPack.ActionDef.DamageDef.DamageType,
			                               aPack.Target,
			                               pos);

			Position newPos = new Position(pos.Pos + new Vector2(rand.Next(16)+8, 0), Vector2.Zero);

			if (damage == 0) {
				UIFactory.createFloatingText ("MISS",
				                              "DAMAGE",
				                              Color.White,
				                              500,
				                              newPos);
			} else {
				UIFactory.createFloatingText ("" + damage,
			                             "DAMAGE",
			                             DamageUtils.getDamageColor (aPack.ActionDef.DamageDef.DamageType),
			                             500,
			                             newPos);
			}

			AwardUtils.attemptSkillAward (aPack.Owner, aPack.Target, aSkill, dSkill, aPack.ActionDef.DamageDef.SkillName, 1);
			AwardUtils.attemptStatAward (aPack.Owner, aPack.Target, aStat, dStat, aPack.ActionDef.DamageDef.StatType, 1);
			AwardUtils.attemptStatAward (aPack.Target, aPack.Owner,
			                             ActionUtils.getStat(aPack.Target,StatType.ENDURANCE), aStat,
			                             StatType.ENDURANCE, 1);

		}

		private static void doStaticDamage(ActionPackage aPack){

			int dmg = rand.Next (aPack.ActionDef.DamageDef.Min, aPack.ActionDef.DamageDef.Max);
			Position pos = ComponentMapper.get<Position> (aPack.Target);
			UtilFactory.createDirectDamage (dmg,
			                               aPack.ActionDef.DamageDef.DamageType,
			                               aPack.Target,
			                               pos);

			Position newPos = new Position(pos.Pos + new Vector2(rand.Next(16)+8, 0), Vector2.Zero);
			UIFactory.createFloatingText("" + dmg,
			                             "DAMAGE",
			                             DamageUtils.getDamageColor(aPack.ActionDef.DamageDef.DamageType),
			                             500,
			                             newPos);
		}
		
	}
}

