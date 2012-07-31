using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;
using Vaerydian.Characters.Experience;
using Vaerydian.Components;
using Vaerydian.Utils;
using Vaerydian.Characters.Skills;
using Vaerydian.Behaviors;
using Vaerydian.Characters.Factions;
using Vaerydian.Components.Characters;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Actions;
using Vaerydian.Components.Graphical;

namespace Vaerydian.Factories
{
    class NPCFactory
    {
        private ECSInstance n_EcsInstance;
        private Random n_Rand = new Random();

        public NPCFactory(ECSInstance ecsInstance) 
        {
            n_EcsInstance = ecsInstance;
        }

        public void destroyRelatedEntities(Entity entity)
        {
            ItemFactory itFact = new ItemFactory(n_EcsInstance);

            itFact.destoryEquipment(entity);
        }


        public void createFollower(Vector2 position, Entity target, float distance)
        {
            Entity e = n_EcsInstance.create();

            n_EcsInstance.EntityManager.addComponent(e, new Position(position, new Vector2(16)));
            n_EcsInstance.EntityManager.addComponent(e, new Velocity(4f));
            n_EcsInstance.EntityManager.addComponent(e, new Sprite("characters\\herr_von_speck_sheet", "characters\\normals\\herr_von_speck_sheet_normals", 32, 32, 0, 0));
            n_EcsInstance.EntityManager.addComponent(e, new AiBehavior(new FollowerBehavior(e, target, distance, n_EcsInstance)));
            n_EcsInstance.EntityManager.addComponent(e, new MapCollidable());
            n_EcsInstance.EntityManager.addComponent(e, new Heading());
            n_EcsInstance.EntityManager.addComponent(e, new Transform());
            n_EcsInstance.EntityManager.addComponent(e, new Aggrivation());

            //create info
            Information info = new Information();
            info.Name = "TEST FOLLOWER";
            info.CreatureGeneralGroup = CreatureGeneralGroup.Human;
            info.CreatureVariationGroup = CreatureVariationGroup.None;
            info.CreatureUniqueGroup = CreatureUniqueGroup.None;
            n_EcsInstance.EntityManager.addComponent(e, info);

            //create life
            Life life = new Life();
            life.IsAlive = true;
            life.DeathLongevity = 5000;
            n_EcsInstance.EntityManager.addComponent(e, life);

            //create interactions
            Interactable interact = new Interactable();
            interact.SupportedInteractions.PROJECTILE_COLLIDABLE = true;
            interact.SupportedInteractions.ATTACKABLE = true;
            interact.SupportedInteractions.MELEE_ACTIONABLE = true;
            interact.SupportedInteractions.AWARDS_VICTORY = true;
            interact.SupportedInteractions.CAUSES_ADVANCEMENT = true;
            interact.SupportedInteractions.MAY_ADVANCE = false;
            n_EcsInstance.EntityManager.addComponent(e, interact);

            //create test equipment
            ItemFactory iFactory = new ItemFactory(n_EcsInstance);
            n_EcsInstance.EntityManager.addComponent(e, iFactory.createTestEquipment());

            int val = 25;

            //setup experiences
            Knowledges knowledges = new Knowledges();
            knowledges.GeneralKnowledge.Add(CreatureGeneralGroup.Human, new Knowledge(val));
            knowledges.VariationKnowledge.Add(CreatureVariationGroup.None, new Knowledge(0));
            knowledges.UniqueKnowledge.Add(CreatureUniqueGroup.None, new Knowledge(0));
            n_EcsInstance.EntityManager.addComponent(e, knowledges);

            //setup attributes
            Attributes attributes = new Attributes();

            attributes.AttributeSet.Add(AttributeType.Focus, val);
            attributes.AttributeSet.Add(AttributeType.Endurance, val);
            attributes.AttributeSet.Add(AttributeType.Mind, val);
            attributes.AttributeSet.Add(AttributeType.Muscle, val);
            attributes.AttributeSet.Add(AttributeType.Perception, val);
            attributes.AttributeSet.Add(AttributeType.Personality, val);
            attributes.AttributeSet.Add(AttributeType.Quickness, val);
            n_EcsInstance.EntityManager.addComponent(e, attributes);

            //create health
            Health health = new Health(attributes.AttributeSet[AttributeType.Endurance] * 5);
            health.RecoveryAmmount = attributes.AttributeSet[AttributeType.Endurance] / 5;
            health.RecoveryRate = 1000;
            n_EcsInstance.EntityManager.addComponent(e, health);

            //setup skills
            Skill skill = new Skill("Avoidance", val, SkillType.Offensive);
            Skills skills = new Skills();
            skills.SkillSet.Add(SkillName.Avoidance, skill);

            skill = new Skill("Ranged", val, SkillType.Offensive);
            skills.SkillSet.Add(SkillName.Ranged, skill);
            n_EcsInstance.EntityManager.addComponent(e, skills);

            Faction faction = new Faction(100, FactionType.Ally);
            Faction enemy = new Faction(-10, FactionType.TestMob);
            Faction player = new Faction(100, FactionType.Player);

            Factions factions = new Factions();
            factions.OwnerFaction = faction;
            factions.KnownFactions.Add(enemy.FactionType, enemy);
            factions.KnownFactions.Add(player.FactionType, player);
            n_EcsInstance.EntityManager.addComponent(e, factions);


            EntityFactory ef = new EntityFactory(n_EcsInstance);
            n_EcsInstance.EntityManager.addComponent(e, ef.createLight(true, 100, new Vector3(position, 10), 0.5f, new Vector4(1, 1, 1, 1)));


            n_EcsInstance.refresh(e);

        }


        public void createWanderingEnemy(Vector2 position)
        {
            Entity e = n_EcsInstance.create();

            n_EcsInstance.EntityManager.addComponent(e, new Position(position, new Vector2(16)));
            n_EcsInstance.EntityManager.addComponent(e, new Velocity(3f));
            n_EcsInstance.EntityManager.addComponent(e, new Sprite("characters\\herr_von_speck_sheet", "characters\\normals\\herr_von_speck_sheet_normals", 32, 32, 0, 0));
            n_EcsInstance.EntityManager.addComponent(e, new AiBehavior(new WanderingEnemyBehavior(e, n_EcsInstance)));
            n_EcsInstance.EntityManager.addComponent(e, new MapCollidable());
            n_EcsInstance.EntityManager.addComponent(e, new Heading());
            n_EcsInstance.EntityManager.addComponent(e, new Transform());
            n_EcsInstance.EntityManager.addComponent(e, new Aggrivation());

            //create info
            Information info = new Information();
            info.Name = "TEST WANDERER";
            info.CreatureGeneralGroup = CreatureGeneralGroup.Human;
            info.CreatureVariationGroup = CreatureVariationGroup.None; 
            info.CreatureUniqueGroup = CreatureUniqueGroup.None;
            n_EcsInstance.EntityManager.addComponent(e, info);

            //create life
            Life life = new Life();
            life.IsAlive = true;
            life.DeathLongevity = 2000;
            n_EcsInstance.EntityManager.addComponent(e, life);

            //create interactions
            Interactable interact = new Interactable();
            interact.SupportedInteractions.PROJECTILE_COLLIDABLE = true;
            interact.SupportedInteractions.ATTACKABLE = true;
            interact.SupportedInteractions.MELEE_ACTIONABLE = true;
            interact.SupportedInteractions.AWARDS_VICTORY = true;
            interact.SupportedInteractions.CAUSES_ADVANCEMENT = true;
            interact.SupportedInteractions.MAY_ADVANCE = false;
            n_EcsInstance.EntityManager.addComponent(e, interact);

            //create test equipment
            ItemFactory iFactory = new ItemFactory(n_EcsInstance);
            n_EcsInstance.EntityManager.addComponent(e, iFactory.createTestEquipment());

            int val = 25;

            //setup experiences
            Knowledges knowledges = new Knowledges();
            knowledges.GeneralKnowledge.Add(CreatureGeneralGroup.Human, new Knowledge(val));
            knowledges.VariationKnowledge.Add(CreatureVariationGroup.None, new Knowledge(0));
            knowledges.UniqueKnowledge.Add(CreatureUniqueGroup.None, new Knowledge(0));
            n_EcsInstance.EntityManager.addComponent(e, knowledges);

            //setup attributes
            Attributes attributes = new Attributes();

            attributes.AttributeSet.Add(AttributeType.Focus, val);
            attributes.AttributeSet.Add(AttributeType.Endurance, val);
            attributes.AttributeSet.Add(AttributeType.Mind, val);
            attributes.AttributeSet.Add(AttributeType.Muscle, val);
            attributes.AttributeSet.Add(AttributeType.Perception, val);
            attributes.AttributeSet.Add(AttributeType.Personality, val);
            attributes.AttributeSet.Add(AttributeType.Quickness, val);
            n_EcsInstance.EntityManager.addComponent(e, attributes);

            //create health
            Health health = new Health(attributes.AttributeSet[AttributeType.Endurance] * 5);
            health.RecoveryAmmount = attributes.AttributeSet[AttributeType.Endurance] / 5;
            health.RecoveryRate = 1000;
            n_EcsInstance.EntityManager.addComponent(e, health);

            //setup skills
            Skill skill = new Skill("Avoidance", val, SkillType.Offensive);
            Skills skills = new Skills();
            skills.SkillSet.Add(SkillName.Avoidance, skill);

            skill = new Skill("Ranged", val, SkillType.Offensive);
            skills.SkillSet.Add(SkillName.Ranged, skill);
            n_EcsInstance.EntityManager.addComponent(e, skills);

            //setup factions
            Faction faction = new Faction(100,FactionType.TestMob);
            Faction player = new Faction(-10, FactionType.Player);
            Faction ally = new Faction(-10, FactionType.Ally);
            Factions factions = new Factions();
            
            factions.OwnerFaction = faction;
            factions.KnownFactions.Add(player.FactionType, player);
            factions.KnownFactions.Add(ally.FactionType, ally);
            n_EcsInstance.EntityManager.addComponent(e, factions);

            Aggrivation aggro = new Aggrivation();
            n_EcsInstance.EntityManager.addComponent(e, aggro);

            EntityFactory ef = new EntityFactory(n_EcsInstance);
            n_EcsInstance.EntityManager.addComponent(e, ef.createLight(true, 100, new Vector3(position, 10), 0.5f, new Vector4(1,1,.6f, 1)));

            n_EcsInstance.refresh(e);
        }

        public void createWanders(int count, GameMap map)
        {
            int xSize = map.Map.XSize;
            int ySize = map.Map.YSize;

            int x, y;

            bool placed = false;

            for (int i = 0; i < count; i++)
            {

                while (!placed)
                {
                    x = n_Rand.Next(1, xSize - 1);
                    y = n_Rand.Next(1, ySize - 1);

                    if (!map.Map.Terrain[x, y].IsBlocking)
                    {
                        createWanderingEnemy(new Vector2(x*32, y*32));
                        placed = true;
                    }

                }

                placed = false;
            }
        }


    }
}
