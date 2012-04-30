using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

using Vaerydian.Characters.Experience;
using Vaerydian.Components;
using Vaerydian.Components.Items;
using Vaerydian.Utils;
using Vaerydian.Factories;

namespace Vaerydian.Systems.Update
{
    class AttackSystem : EntityProcessingSystem
    {
        private ComponentMapper a_AttackMapper;
        private ComponentMapper a_PositionMapper;
        private ComponentMapper a_SkillMapper;
        private ComponentMapper a_AttributeMapper;
        private ComponentMapper a_ExperienceMapper;
        private ComponentMapper a_EquipmentMapper;
        private ComponentMapper a_ItemMapper;
        private ComponentMapper a_WeaponMapper;
        private ComponentMapper a_ArmorMapper;

        private Entity a_CurrentEntity;

        private UtilFactory a_Factory;

        private Random rand = new Random();

        public AttackSystem() { }
        
        public override void initialize()
        {
            a_AttackMapper = new ComponentMapper(new Attack(), e_ECSInstance);
            a_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            a_SkillMapper = new ComponentMapper(new Skills(), e_ECSInstance);
            a_AttributeMapper = new ComponentMapper(new Attributes(), e_ECSInstance);
            a_ExperienceMapper = new ComponentMapper(new Experiences(), e_ECSInstance);
            a_EquipmentMapper = new ComponentMapper(new Equipment(), e_ECSInstance);
            a_ItemMapper = new ComponentMapper(new Item(), e_ECSInstance);
            a_WeaponMapper = new ComponentMapper(new Weapon(), e_ECSInstance);
            a_ArmorMapper = new ComponentMapper(new Armor(), e_ECSInstance);


            a_Factory = new UtilFactory(e_ECSInstance);
        }

        protected override void preLoadContent(ECSFramework.Utils.Bag<Entity> entities)
        {
            
        }
        
        protected override void process(Entity entity)
        {
            //retrieve this attack
            a_CurrentEntity = entity;
            Attack attack = (Attack)a_AttackMapper.get(entity);

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
            Vector2 pos = position.getPosition();
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
            Attributes attAttr = (Attributes)a_AttributeMapper.get(attack.Attacker);
            Attributes defAttr = (Attributes)a_AttributeMapper.get(attack.Defender);

            //dont continue if either of these are null
            if (attAttr == null || defAttr == null)
                return;

            //get Experience
            Experiences attXp = (Experiences)a_ExperienceMapper.get(attack.Attacker);
            Experiences defXp = (Experiences)a_ExperienceMapper.get(attack.Defender);

            //dont continue if null
            if (attXp == null || defXp == null)
                return;

            //get Skills
            Skills attSkills = (Skills)a_SkillMapper.get(attack.Attacker);
            Skills defSkills = (Skills)a_SkillMapper.get(attack.Defender);

            //dont continue if either of these are null
            if (attSkills == null || defSkills == null)
                return;

            int atkSkill = attSkills.SkillSet[SkillNames.Melee].Value;
            int defSkill = defSkills.SkillSet[SkillNames.Avoidance].Value;

            float probHit = atkSkill / 4 + attAttr.Perception.Value / 4 + attXp.GeneralExperience[MobGroup.Test].Value + weapon.Speed;
            float probDef = defSkill / 4 + defAttr.Quickness.Value / 4 + defXp.GeneralExperience[MobGroup.Test].Value + armor.Mobility;

            float hitProb = (probHit / (probHit + probDef)) * 1.75f + (probDef / (probHit + probDef)) * 0.15f;

            float toHit = (float)rand.NextDouble();

            int damage = 0;

            if (toHit < hitProb)
            {

                float overhit = 0f;

                if (hitProb > 1f)
                    overhit = hitProb - 1f;

                int maxDmg = (int)((overhit + 1f) * (atkSkill / 5 + attAttr.Perception.Value / 4 + (weapon.Lethality - armor.Mitigation + 1)));

                damage = rand.Next(maxDmg / 2, maxDmg);

                if (damage < 0)
                    damage = 0;
            }

            a_Factory.createDirectDamage(damage, DamageType.Common, attack.Defender, newPos);
            
            
            
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
            Vector2 pos = position.getPosition();
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
            Attributes attAttr = (Attributes)a_AttributeMapper.get(attack.Attacker);
            Attributes defAttr = (Attributes)a_AttributeMapper.get(attack.Defender);

            //dont continue if either of these are null
            if (attAttr == null || defAttr == null)
                return;

            //get Experience
            Experiences attXp = (Experiences)a_ExperienceMapper.get(attack.Attacker);
            Experiences defXp = (Experiences)a_ExperienceMapper.get(attack.Defender);

            //dont continue if null
            if (attXp == null || defXp == null)
                return;

            //get Skills
            Skills attSkills = (Skills)a_SkillMapper.get(attack.Attacker);
            Skills defSkills = (Skills)a_SkillMapper.get(attack.Defender);

            //dont continue if either of these are null
            if (attSkills == null || defSkills == null)
                return;

            int atkSkill = attSkills.SkillSet[SkillNames.Ranged].Value;
            int defSkill = defSkills.SkillSet[SkillNames.Avoidance].Value;

            float probHit = atkSkill / 4 + attAttr.Perception.Value / 4 + attXp.GeneralExperience[MobGroup.Test].Value + weapon.Speed;
            float probDef = defSkill / 4 + defAttr.Quickness.Value / 4 + defXp.GeneralExperience[MobGroup.Test].Value + armor.Mobility;

            float hitProb = (probHit / (probHit + probDef)) * 1.75f + (probDef / (probHit + probDef)) * 0.15f;

            float toHit = (float)rand.NextDouble();

            int damage = 0;

            if (toHit < hitProb)
            {

                float overhit = 0f;

                if (hitProb > 1f)
                    overhit = hitProb - 1f;

                int maxDmg = (int)((overhit + 1f) * (atkSkill / 5 + attAttr.Perception.Value / 4 + (weapon.Lethality - armor.Mitigation + 1)));

                damage = rand.Next(maxDmg / 2, maxDmg);

                if (damage < 0)
                    damage = 0;
            }

            a_Factory.createDirectDamage(damage, DamageType.Common, attack.Defender, newPos);

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
