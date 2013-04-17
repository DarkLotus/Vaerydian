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

        private UIFactory v_UIFactory;

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

            v_UIFactory = new UIFactory(e_ECSInstance);
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
            Knowledge awdGeneral = awarder.GeneralKnowledge[info.CreatureGeneralGroup];
            Knowledge awdVaration = awarder.VariationKnowledge[info.CreatureVariationGroup];
            Knowledge awdUnique = awarder.UniqueKnowledge[info.CreatureUniqueGroup];

            Knowledge recGeneral = receiver.GeneralKnowledge[info.CreatureGeneralGroup];
            Knowledge recVaration = receiver.VariationKnowledge[info.CreatureVariationGroup];
            Knowledge recUnique = receiver.UniqueKnowledge[info.CreatureUniqueGroup];

            //reward general
            if (recGeneral.Value < awdGeneral.Value)
            {
                //calculate reward
                float val = ((awdGeneral.Value - recGeneral.Value) / awdGeneral.Value) * award.MaxAwardable;
                
                if (val < award.MinAwardable)
                    val = award.MinAwardable;
                
                recGeneral.Value += val;

                //announce reward
                Position pos = (Position)v_PositionMapper.get(award.Receiver);
                if (pos != null)
                    v_UIFactory.createFloatingText("+" + val.ToString("#.0") + " [" + info.CreatureGeneralGroup.ToString() + "]", "GENERAL", Color.MediumPurple, 1000, new Position(pos.Pos, pos.Offset));
            }
            //reward variation
            if (recVaration.Value < awdVaration.Value)
            {
                //calculate reward
                float val = ((awdVaration.Value - recVaration.Value) / awdVaration.Value) * award.MaxAwardable;
                
                if (val < award.MinAwardable)
                    val = award.MinAwardable;

                recVaration.Value += val;

                //announce reward
                Position pos = (Position)v_PositionMapper.get(award.Receiver);
                if (pos != null)
                    v_UIFactory.createFloatingText("+" + val.ToString("#.0") + " [" + info.CreatureVariationGroup.ToString() + "]", "GENERAL", Color.MediumPurple, 1000, new Position(pos.Pos, pos.Offset));
            }
            //reward unique
            if (recUnique.Value < awdUnique.Value)
            {
                //calculate reward
                float val = ((awdUnique.Value - recUnique.Value) / awdUnique.Value) * award.MaxAwardable;

                if (val < award.MinAwardable)
                    val = award.MinAwardable;

                recUnique.Value += val;

                //announce reward
                Position pos = (Position)v_PositionMapper.get(award.Receiver);
                if (pos != null)
                    v_UIFactory.createFloatingText("+" + val.ToString("#.0") + " [" + info.CreatureUniqueGroup.ToString() + "]", "GENERAL", Color.MediumPurple, 1000, new Position(pos.Pos, pos.Offset));
            }


        }

        /// <summary>
        /// awards a skill-up
        /// </summary>
        /// <param name="entity"></param>
        private void awardSkillUp(Award award)
        {
            Skills skills = (Skills)v_SkillMapper.get(award.Receiver);

            skills.SkillSet[award.SkillName].Value += award.MaxAwardable;

            Position pos = (Position)v_PositionMapper.get(award.Receiver);
            if (pos != null)
                v_UIFactory.createFloatingText("+" + award.MaxAwardable + " [" + award.SkillName.ToString() + "]", "GENERAL", Color.SkyBlue, 1000, new Position(pos.Pos, pos.Offset));

        }

        /// <summary>
        /// awards an attribute-up
        /// </summary>
        /// <param name="award"></param>
        private void awardAttributeUp(Award award)
        {
            Statistics attributes = (Statistics)v_AttributeMapper.get(award.Receiver);

            attributes.StatisticSet[award.AttributeType] += award.MaxAwardable;

            Position pos = (Position)v_PositionMapper.get(award.Receiver);
            if (pos != null)
                v_UIFactory.createFloatingText("+" + award.MaxAwardable + " [" + award.AttributeType.ToString() + "]", "GENERAL", Color.Orange, 1000, new Position(pos.Pos, pos.Offset));
        }

        private void awardHealthUp(Award award)
        {
            Health health = (Health)v_HealthMapper.get(award.Receiver);
            Statistics attributes = (Statistics)v_AttributeMapper.get(award.Receiver);

            health.MaxHealth += award.MaxAwardable;
            health.RecoveryAmmount = attributes.StatisticSet[StatType.Endurance] / 5;

            Position pos = (Position)v_PositionMapper.get(award.Receiver);
            if (pos != null)
                v_UIFactory.createFloatingText("+" + award.MaxAwardable + " [Max HP]", "GENERAL", Color.Red, 1000, new Position(pos.Pos, pos.Offset));
        }
    }
}
