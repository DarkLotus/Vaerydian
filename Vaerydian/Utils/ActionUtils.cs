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

		public static float getStatProbability(int skillValue, int attributeValue, float knowledgeValue, int speedValue){
			return skillValue / 4f + attributeValue / 4f + knowledgeValue + speedValue;
		}

		public static float getHitProbability(float defender, float attacker, float max, float min){
			return (attacker/(defender+attacker))*max + (defender/(defender+attacker))*min;
		}

		public static float getDamage(float overhit, float attackSkill, float attackAttribute,
			                              float lethality, float mitigation, float absorbValue){
			return (overhit + 1f) * (attackSkill/5 + attackAttribute/4) * (lethality/mitigation) - absorbValue/10;
		}

		private static float getSkill(Entity entity, SkillName skillname){
			return ComponentMapper.get<Skills> (entity).SkillSet [skillname].Value;
		}

		private static float getStat(Entity entity, StatType stat){
			return (float)ComponentMapper.get<Statistics> (entity).StatisticSet [stat];
		}

		private static Weapon getWeapon(Entity entity){
			Equipment equip = ComponentMapper.get<Equipment> (entity);
			return ComponentMapper.get<Weapon> (equip.MeleeWeapon);
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
			Weapon dWeapon = ActionUtils.getWeapon (aPack.Target);

			float aSkill = ActionUtils.getSkill (aPack.Owner, aPack.ActionDef.DamageDef.SkillName);
			float dSkill = ActionUtils.getSkill (aPack.Target, SkillName.AVOIDANCE);

			float aStat = ActionUtils.getStat (aPack.Owner, aPack.ActionDef.DamageDef.StatType);
			float dStat = ActionUtils.getStat (aPack.Target, ActionUtils.getOppositeStat (aPack.ActionDef.DamageDef.StatType));
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

