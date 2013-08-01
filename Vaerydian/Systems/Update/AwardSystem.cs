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


using Vaerydian.Components.Characters;
using Vaerydian.Components.Utils;
using Vaerydian.Characters;
using Vaerydian.Components.Spatials;
using Vaerydian.Factories;

using Microsoft.Xna.Framework;


namespace Vaerydian.Systems.Update
{
    class AwardSystem : EntityProcessingSystem
    {
        private ComponentMapper v_VictoryMapper;
        private ComponentMapper v_KnowledgeMapper;
        private ComponentMapper v_InfoMapper;
        private ComponentMapper v_PositionMapper;
        private ComponentMapper v_SkillMapper;
        private ComponentMapper v_AttributeMapper;
        private ComponentMapper v_HealthMapper;

        public AwardSystem(){ }
        
        public override void initialize()
        {
            v_KnowledgeMapper = new ComponentMapper(new Knowledges(), e_ECSInstance);
            v_VictoryMapper = new ComponentMapper(new Award(), e_ECSInstance);
            v_InfoMapper = new ComponentMapper(new Information(), e_ECSInstance);
            v_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            v_SkillMapper = new ComponentMapper(new Skills(), e_ECSInstance);
            v_AttributeMapper = new ComponentMapper(new Statistics(), e_ECSInstance);
            v_HealthMapper = new ComponentMapper(new Health(), e_ECSInstance);

        }
        
        protected override void preLoadContent(Bag<Entity> entities)
        {
            
        }

        protected override void cleanUp(Bag<Entity> entities) { }
                
        protected override void process(Entity entity)
        {
            Award award = (Award)v_VictoryMapper.get(entity);

            //if for whatever reason either a null, return your ass
            if (award == null)
                return;

            switch (award.AwardType)
            {
                case AwardType.Victory:
                    awardVictory(award);
                    break;
                case AwardType.SkillUp:
                    awardSkillUp(award);
                    break;
                case AwardType.Attribute:
                    awardAttributeUp(award);
                    break;
                case AwardType.Health:
                    awardHealthUp(award);
                    break;
                default:
                    break;
            }


            //end victory
            e_ECSInstance.deleteEntity(entity);
        }

        /// <summary>
        /// awards a victory
        /// </summary>
        /// <param name="entity"></param>
        private void awardVictory(Award award)
        {
            //retrieve knowledges
            Knowledges awarder = (Knowledges)v_KnowledgeMapper.get(award.Awarder);
            Knowledges receiver = (Knowledges)v_KnowledgeMapper.get(award.Receiver);

            //if either is not available, don't continue
            if (awarder == null || receiver == null)
                return;

            //retrieve creature information
            Information info = (Information)v_InfoMapper.get(award.Awarder);

            //cant continue if no info
            if (info == null)
                return;

            //look up skills/knowledge
            Knowledge awdGeneral = awarder.GeneralKnowledge[info.GeneralGroup];
            Knowledge awdVaration = awarder.VariationKnowledge[info.VariationGroup];
            Knowledge awdUnique = awarder.UniqueKnowledge[info.UniqueGroup];

            Knowledge recGeneral = receiver.GeneralKnowledge[info.GeneralGroup];
            Knowledge recVaration = receiver.VariationKnowledge[info.VariationGroup];
            Knowledge recUnique = receiver.UniqueKnowledge[info.UniqueGroup];

            //reward general
            if (recGeneral.Value < awdGeneral.Value)
            {
                //calculate reward
                float val = ((awdGeneral.Value - recGeneral.Value) / awdGeneral.Value) * award.MaxAwardable;
                
                if (val < award.MinAwardable)
                    val = award.MinAwardable;
                
                recGeneral.Value += val;
				receiver.GeneralKnowledge.Remove (info.GeneralGroup);
				receiver.GeneralKnowledge.Add (info.GeneralGroup, recGeneral);

                //announce reward
                Position pos = (Position)v_PositionMapper.get(award.Receiver);
                if (pos != null)
                    UIFactory.createFloatingText("+" + val.ToString("#.0") + " [" + info.GeneralGroup.ToString() + "]", "GENERAL", Color.MediumPurple, 1000, new Position(pos.Pos, pos.Offset));
            }
            //reward variation
            if (recVaration.Value < awdVaration.Value)
            {
                //calculate reward
                float val = ((awdVaration.Value - recVaration.Value) / awdVaration.Value) * award.MaxAwardable;
                
                if (val < award.MinAwardable)
                    val = award.MinAwardable;

                recVaration.Value += val;
				receiver.VariationKnowledge.Remove (info.VariationGroup);
				receiver.VariationKnowledge.Add (info.VariationGroup, recVaration);

                //announce reward
                Position pos = (Position)v_PositionMapper.get(award.Receiver);
                if (pos != null)
                    UIFactory.createFloatingText("+" + val.ToString("#.0") + " [" + info.VariationGroup.ToString() + "]", "GENERAL", Color.MediumPurple, 1000, new Position(pos.Pos, pos.Offset));
            }
            //reward unique
            if (recUnique.Value < awdUnique.Value)
            {
                //calculate reward
                float val = ((awdUnique.Value - recUnique.Value) / awdUnique.Value) * award.MaxAwardable;

                if (val < award.MinAwardable)
                    val = award.MinAwardable;

                recUnique.Value += val;
				receiver.UniqueKnowledge.Remove (info.UniqueGroup);
				receiver.UniqueKnowledge.Add (info.UniqueGroup, recUnique);

                //announce reward
                Position pos = (Position)v_PositionMapper.get(award.Receiver);
                if (pos != null)
                    UIFactory.createFloatingText("+" + val.ToString("#.0") + " [" + info.UniqueGroup.ToString() + "]", "GENERAL", Color.MediumPurple, 1000, new Position(pos.Pos, pos.Offset));
            }


        }

        /// <summary>
        /// awards a skill-up
        /// </summary>
        /// <param name="entity"></param>
        private void awardSkillUp(Award award)
        {
            Skills skills = (Skills)v_SkillMapper.get(award.Receiver);

            //skills.SkillSet[award.SkillName].Value += award.MaxAwardable;
			switch (award.SkillName) {
			case SkillName.AVOIDANCE:
				skills.Avoidance.Value += award.MaxAwardable;
				break;
			case SkillName.MELEE:
				skills.Melee.Value += award.MaxAwardable;
				break;
			case SkillName.RANGED:
				skills.Ranged.Value += award.MaxAwardable;
				break;
			}

            Position pos = (Position)v_PositionMapper.get(award.Receiver);
            if (pos != null)
                UIFactory.createFloatingText("+" + award.MaxAwardable + " [" + award.SkillName.ToString() + "]", "GENERAL", Color.SkyBlue, 1000, new Position(pos.Pos, pos.Offset));

        }

        /// <summary>
        /// awards an attribute-up
        /// </summary>
        /// <param name="award"></param>
        private void awardAttributeUp(Award award)
        {
            Statistics statistics = (Statistics)v_AttributeMapper.get(award.Receiver);

            //attributes.StatisticSet[award.AttributeType] += award.MaxAwardable;
			switch (award.StatType) {
			case StatType.ENDURANCE:
				statistics.Endurance.Value += award.MaxAwardable;
				break;
			case StatType.FOCUS:
				statistics.Focus.Value += award.MaxAwardable;
				break;
			case StatType.MIND:
				statistics.Mind.Value += award.MaxAwardable;
				break;
			case StatType.MUSCLE:
				statistics.Muscle.Value += award.MaxAwardable;
				break;
			case StatType.PERCEPTION:
				statistics.Perception.Value += award.MaxAwardable;
				break;
			case StatType.PERSONALITY:
				statistics.Personality.Value += award.MaxAwardable;
				break;
			case StatType.QUICKNESS:
				statistics.Quickness.Value += award.MaxAwardable;
				break;
			}

            Position pos = (Position)v_PositionMapper.get(award.Receiver);
            if (pos != null)
				UIFactory.createFloatingText("+" + award.MaxAwardable + " [" + award.StatType.ToString() + "]", "GENERAL", Color.Orange, 1000, new Position(pos.Pos, pos.Offset));
        }

        private void awardHealthUp(Award award)
        {
            Health health = (Health)v_HealthMapper.get(award.Receiver);
            Statistics statistics = (Statistics)v_AttributeMapper.get(award.Receiver);

            health.MaxHealth += award.MaxAwardable;
            health.RecoveryAmmount = statistics.Endurance.Value / 5;

            Position pos = (Position)v_PositionMapper.get(award.Receiver);
            if (pos != null)
                UIFactory.createFloatingText("+" + award.MaxAwardable + " [Max HP]", "GENERAL", Color.Red, 1000, new Position(pos.Pos, pos.Offset));
        }
    }
}
