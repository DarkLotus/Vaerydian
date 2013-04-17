using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

using Vaerydian.Characters;
using Vaerydian.Components;
using Vaerydian.Components.Items;
using Vaerydian.Components.Utils;
using Vaerydian.Utils;
using Vaerydian.Factories;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Actions;
using Vaerydian.Components.Characters;


namespace Vaerydian.Systems.Update
{
    class AttackSystem : EntityProcessingSystem
    {
        private ComponentMapper a_AttackMapper;
        private ComponentMapper a_PositionMapper;
        private ComponentMapper a_SkillMapper;
        private ComponentMapper a_AttributeMapper;
        private ComponentMapper a_KnowledgeMapper;
        private ComponentMapper a_EquipmentMapper;
        private ComponentMapper a_ItemMapper;
        private ComponentMapper a_WeaponMapper;
        private ComponentMapper a_ArmorMapper;
        private ComponentMapper a_AggroMapper;
        private ComponentMapper a_InfoMapper;
        private ComponentMapper a_InteractMapper;

        private Entity a_CurrentEntity;

        private UtilFactory a_UtilFactory;
        private UIFactory a_UIFactory;

        private Random rand = new Random();

        public AttackSystem() { }
        
        public override void initialize()
        {
            a_AttackMapper = new ComponentMapper(new Attack(), e_ECSInstance);
            a_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            a_SkillMapper = new ComponentMapper(new Skills(), e_ECSInstance);
            a_AttributeMapper = new ComponentMapper(new Statistics(), e_ECSInstance);
            a_KnowledgeMapper = new ComponentMapper(new Knowledges(), e_ECSInstance);
            a_EquipmentMapper = new ComponentMapper(new Equipment(), e_ECSInstance);
            a_ItemMapper = new ComponentMapper(new Item(), e_ECSInstance);
            a_WeaponMapper = new ComponentMapper(new Weapon(), e_ECSInstance);
            a_ArmorMapper = new ComponentMapper(new Armor(), e_ECSInstance);
            a_AggroMapper = new ComponentMapper(new Aggrivation(), e_ECSInstance);
            a_InfoMapper = new ComponentMapper(new Information(), e_ECSInstance);
            a_InteractMapper = new ComponentMapper(new Interactable(), e_ECSInstance);
            


            a_UtilFactory = new UtilFactory(e_ECSInstance);
            a_UIFactory = new UIFactory(e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            
        }

        protected override void cleanUp(Bag<Entity> entities) { }
        
        protected override void process(Entity entity)
        {
            //retrieve this attack
            a_CurrentEntity = entity;
            Attack attack = (Attack)a_AttackMapper.get(entity);

            //see if defender is aggroable
            Aggrivation aggro = (Aggrivation)a_AggroMapper.get(attack.Defender);
            if (aggro != null)
            {
                //set aggro
                if (!aggro.HateList.Contains(attack.Attacker))
                    aggro.HateList.Add(attack.Attacker);
            }


            //determine type of attack and handle it
            switch (attack.AttackType)
            {
                case AttackType.Melee:
                    handleMelee(attack);
                    break;
                case AttackType.Projectile:
                    handleProjectile(attack);
                    break;
                case AttackType.Ability:
                    handleAbility(attack);
                    break;
                default:
                    e_ECSInstance.deleteEntity(entity);
                    break;
            }
        }

        /// <summary>
        /// handle melee attacks
        /// </summary>
        /// <param name="attack">attack to handle</param>
        private void handleMelee(Attack attack)
        {
            Position position = (Position)a_PositionMapper.get(attack.Defender);

            //dont continue if this attack has no position
            if (position == null)
                return;

            //calculate position
            Vector2 pos = position.Pos;
            Position newPos = new Position(pos + new Vector2(rand.Next(16) + 8, 0), Vector2.Zero);

            //get equipment
            Equipment attEquip = (Equipment)a_EquipmentMapper.get(attack.Attacker);
            Equipment defEquip = (Equipment)a_EquipmentMapper.get(attack.Defender);

            //dont continue if we have no equipment to use
            if (attEquip == null || defEquip == null)
                return;

            //get weapon and armor
            Weapon weapon = (Weapon)a_WeaponMapper.get(attEquip.MeleeWeapon);
            Armor armor = (Armor)a_ArmorMapper.get(defEquip.Armor);

            //dont continue if either of these are null
            if (weapon == null || armor == null)
                return;

            //get attributes
            Statistics attAttr = (Statistics)a_AttributeMapper.get(attack.Attacker);
            Statistics defAttr = (Statistics)a_AttributeMapper.get(attack.Defender);

            //dont continue if either of these are null
            if (attAttr == null || defAttr == null)
                return;

            int perception = attAttr.StatisticSet[StatType.Perception];
            int muscle = attAttr.StatisticSet[StatType.Muscle];
            int quickness = defAttr.StatisticSet[StatType.Quickness];
            int endurance = defAttr.StatisticSet[StatType.Endurance];

            //get Experience
            Knowledges attKnw = (Knowledges)a_KnowledgeMapper.get(attack.Attacker);
            Knowledges defKnw = (Knowledges)a_KnowledgeMapper.get(attack.Defender);

            //dont continue if null
            if (attKnw == null || defKnw == null)
                return;

            //get Skills
            Skills attSkills = (Skills)a_SkillMapper.get(attack.Attacker);
            Skills defSkills = (Skills)a_SkillMapper.get(attack.Defender);

            //dont continue if either of these are null
            if (attSkills == null || defSkills == null)
                return;

            int atkSkill = attSkills.SkillSet[SkillName.Melee].Value;
            int defSkill = defSkills.SkillSet[SkillName.Avoidance].Value;

            Information infoDef = (Information)a_InfoMapper.get(attack.Defender);
            Information infoAtk = (Information)a_InfoMapper.get(attack.Attacker);

            //dont continue if you dont have info
            if (infoDef == null || infoAtk == null)
                return;

            float probHit = atkSkill / 4 + perception / 4 + attKnw.GeneralKnowledge[infoDef.CreatureGeneralGroup].Value + weapon.Speed;
            float probDef = defSkill / 4 + quickness / 4 + defKnw.GeneralKnowledge[infoAtk.CreatureGeneralGroup].Value + armor.Mobility;

            float hitProb = (probHit / (probHit + probDef)) * 1.75f + (probDef / (probHit + probDef)) * 0.15f;

            float toHit = (float)rand.NextDouble();

            int damage = 0;

            if (toHit < hitProb)
            {

                float overhit = 0f;

                if (hitProb > 1f)
                    overhit = hitProb - 1f;

                //int maxDmg = (int)((overhit + 1f) * ((atkSkill / 5 + muscle / 4) / (endurance / 10)) * (weapon.Lethality / armor.Mitigation));
                int maxDmg = (int)((overhit + 1f) * ((atkSkill / 5 + muscle / 4)) * (weapon.Lethality / armor.Mitigation)) - (endurance/10);

                damage = rand.Next(maxDmg / 2, maxDmg);

                if (damage < 0)
                    damage = 0;
            }

            a_UtilFactory.createDirectDamage(damage, weapon.DamageType, attack.Defender, newPos);

            if (damage == 0)
            {
                a_UIFactory.createFloatingText("MISS","DAMAGE", Color.White,500, new Position(newPos.Pos,newPos.Offset));
            }
            else
            {
                a_UIFactory.createFloatingText(""+damage, "DAMAGE", Color.Yellow, 500, new Position(newPos.Pos, newPos.Offset));
            }

            Interactable interactor = (Interactable)a_InteractMapper.get(attack.Attacker);
            Interactable interactee = (Interactable)a_InteractMapper.get(attack.Defender);

            //only do if interaction supported
            if (interactee != null && interactor != null)
            {
                //only skill-up if you can
                if (interactor.SupportedInteractions.MAY_ADVANCE &&
                    interactee.SupportedInteractions.CAUSES_ADVANCEMENT)
                {
                    //if still possible to skill-up
                    if (atkSkill < defSkill)
                    {
                        if (rand.NextDouble() <= ((double)(defSkill - atkSkill) / (double)defSkill) * 0.5)
                            a_UtilFactory.createSkillupAward(attack.Defender, attack.Attacker, SkillName.Melee, 1);
                    }

                    if (perception < quickness)
                    {
                        if (rand.NextDouble() <= ((double)(quickness - perception) / (double)quickness) * 0.25)
                            a_UtilFactory.createAttributeAward(attack.Defender, attack.Attacker, StatType.Perception, 1);
                    }

                    if (muscle < endurance)
                    {
                        if (rand.NextDouble() <= ((double)(endurance - muscle) / (double)endurance) * 0.25)
                            a_UtilFactory.createAttributeAward(attack.Defender, attack.Attacker, StatType.Muscle, 1);
                    }
                }

                if (interactor.SupportedInteractions.CAUSES_ADVANCEMENT &&
                    interactee.SupportedInteractions.MAY_ADVANCE)
                {
                    //if still possible to skill-up
                    if (defSkill < atkSkill)
                    {
                        if (rand.NextDouble() <= ((double)(atkSkill - defSkill) / (double)atkSkill) * 0.5)
                            a_UtilFactory.createSkillupAward(attack.Attacker, attack.Defender, SkillName.Avoidance, 1);
                    }

                    if (quickness < perception)
                    {
                        if (rand.NextDouble() <= ((double)(perception - quickness) / (double)perception) * 0.25)
                            a_UtilFactory.createAttributeAward(attack.Attacker, attack.Defender, StatType.Quickness, 1);
                    }


                    if (endurance < muscle)
                    {
                        if (rand.NextDouble() <= ((double)(muscle - endurance) / (double)muscle) * 0.25)
                            a_UtilFactory.createAttributeAward(attack.Attacker, attack.Defender, StatType.Endurance, 1);
                    }
                }
            }


            //remove attack
            e_ECSInstance.deleteEntity(a_CurrentEntity);
        }

        /// <summary>
        /// handle projectile attacks
        /// </summary>
        /// <param name="attack">attack to handle</param>
        private void handleProjectile(Attack attack) 
        {
            Position position = (Position)a_PositionMapper.get(attack.Defender);

            //dont continue if this attack has no position
            if (position == null)
                return;

            //calculate position
            Vector2 pos = position.Pos;
            Position newPos = new Position(pos + new Vector2(rand.Next(16)+8, 0), Vector2.Zero);

            //get equipment
            Equipment attEquip = (Equipment)a_EquipmentMapper.get(attack.Attacker);
            Equipment defEquip = (Equipment)a_EquipmentMapper.get(attack.Defender);

            //dont continue if we have no equipment to use
            if (attEquip == null || defEquip == null)
                return;

            //get weapon and armor
            Weapon weapon = (Weapon) a_WeaponMapper.get(attEquip.RangedWeapon);
            Armor armor = (Armor) a_ArmorMapper.get(defEquip.Armor);

            //dont continue if either of these are null
            if (weapon == null || armor == null)
                return;

            //get attributes
            Statistics attAttr = (Statistics)a_AttributeMapper.get(attack.Attacker);
            Statistics defAttr = (Statistics)a_AttributeMapper.get(attack.Defender);

            //dont continue if either of these are null
            if (attAttr == null || defAttr == null)
                return;

            int perception = attAttr.StatisticSet[StatType.Perception];
            int quickness = defAttr.StatisticSet[StatType.Quickness];
            int focus = attAttr.StatisticSet[StatType.Focus];
            int endurance = defAttr.StatisticSet[StatType.Endurance];

            //get Experience
            Knowledges attKnw = (Knowledges)a_KnowledgeMapper.get(attack.Attacker);
            Knowledges defKnw = (Knowledges)a_KnowledgeMapper.get(attack.Defender);

            //dont continue if null
            if (attKnw == null || defKnw == null)
                return;

            //get Skills
            Skills attSkills = (Skills)a_SkillMapper.get(attack.Attacker);
            Skills defSkills = (Skills)a_SkillMapper.get(attack.Defender);

            //dont continue if either of these are null
            if (attSkills == null || defSkills == null)
                return;

            int atkSkill = attSkills.SkillSet[SkillName.Ranged].Value;
            int defSkill = defSkills.SkillSet[SkillName.Avoidance].Value;


            Information infoDef = (Information)a_InfoMapper.get(attack.Defender);
            Information infoAtk = (Information)a_InfoMapper.get(attack.Attacker);

            //dont continue if you dont have info
            if (infoDef == null || infoAtk == null)
                return;

            float probHit = atkSkill / 4 + perception / 4 + attKnw.GeneralKnowledge[infoDef.CreatureGeneralGroup].Value + weapon.Speed;
            float probDef = defSkill / 4 + quickness / 4 + defKnw.GeneralKnowledge[infoAtk.CreatureGeneralGroup].Value + armor.Mobility;

            float hitProb = (probHit / (probHit + probDef)) * 1.75f + (probDef / (probHit + probDef)) * 0.15f;

            float toHit = (float)rand.NextDouble();

            int damage = 0;

            if (toHit < hitProb)
            {

                float overhit = 0f;

                if (hitProb > 1f)
                    overhit = hitProb - 1f;

                //int maxDmg = (int)((overhit + 1f) * ((atkSkill / 5 + focus / 4) / (endurance / 10)) * (weapon.Lethality / armor.Mitigation));
                int maxDmg = (int)((overhit + 1f) * ((atkSkill / 5 + focus / 4)) * (weapon.Lethality / armor.Mitigation)) - (endurance / 10);

                damage = rand.Next(maxDmg / 2, maxDmg);

                if (damage < 0)
                    damage = 0;
            }

            a_UtilFactory.createDirectDamage(damage, weapon.DamageType, attack.Defender, newPos);

            //create the floating dmg
            if (damage == 0)
            {
                a_UIFactory.createFloatingText("MISS", "DAMAGE", Color.White, 500, new Position(newPos.Pos, newPos.Offset));
            }
            else
            {
                a_UIFactory.createFloatingText("" + damage, "DAMAGE", Color.Yellow, 500, new Position(newPos.Pos, newPos.Offset));
            }


            Interactable interactor = (Interactable)a_InteractMapper.get(attack.Attacker);
            Interactable interactee = (Interactable)a_InteractMapper.get(attack.Defender);

            //only do if interaction supported
            if (interactor != null && interactor != null)
            {
                //only skill-up if you can
                if (interactor.SupportedInteractions.MAY_ADVANCE && 
                    interactee.SupportedInteractions.CAUSES_ADVANCEMENT)
                {
                    //if still possible to skill-up
                    if (atkSkill < defSkill)
                    {
                        if (rand.NextDouble() <= ((double)(defSkill - atkSkill) / (double)defSkill) * 0.5)
                            a_UtilFactory.createSkillupAward(attack.Defender, attack.Attacker, SkillName.Ranged, 1);
                    }

                    if (perception < quickness)
                    {
                        if (rand.NextDouble() <= ((double)(quickness - perception) / (double)quickness) * 0.25)
                            a_UtilFactory.createAttributeAward(attack.Defender, attack.Attacker, StatType.Perception, 1);
                    }

                    if (focus < endurance)
                    {
                        if(rand.NextDouble() <= ((double)(endurance - focus) / (double) endurance) * 0.25)
                            a_UtilFactory.createAttributeAward(attack.Defender, attack.Attacker, StatType.Focus, 1);
                    }
                }

                if (interactor.SupportedInteractions.CAUSES_ADVANCEMENT &&
                    interactee.SupportedInteractions.MAY_ADVANCE)
                {
                    //if still possible to skill-up
                    if (defSkill < atkSkill)
                    {
                        if (rand.NextDouble() <= ((double)(atkSkill - defSkill) / (double)atkSkill) * 0.5)
                            a_UtilFactory.createSkillupAward(attack.Attacker, attack.Defender, SkillName.Avoidance, 1);
                    }

                    if (quickness < perception)
                    {
                        if (rand.NextDouble() <= ((double)(perception - quickness) / (double)perception) * 0.25)
                            a_UtilFactory.createAttributeAward(attack.Attacker, attack.Defender, StatType.Quickness, 1);
                    }


                    if (endurance < focus)
                    {
                        if (rand.NextDouble() <= ((double)(focus - endurance) / (double)focus) * 0.25)
                            a_UtilFactory.createAttributeAward(attack.Attacker, attack.Defender, StatType.Endurance, 1);
                    }
                }
            }

            //remove attack
            e_ECSInstance.deleteEntity(a_CurrentEntity);
        }

        /// <summary>
        /// handle ability attacks
        /// </summary>
        /// <param name="attack">attack to handle</param>
        private void handleAbility(Attack attack)
        {
            //remove attack
            e_ECSInstance.deleteEntity(a_CurrentEntity);
        }
    }
}
