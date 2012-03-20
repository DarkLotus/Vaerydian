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

            //create health
            Health health = new Health(200);
            health.RecoveryAmmount = 20;
            health.RecoveryRate = 500;
            n_EcsInstance.EntityManager.addComponent(e, health);

            //create life
            Life life = new Life();
            life.IsAlive = true;
            life.DeathLongevity = 5000;
            n_EcsInstance.EntityManager.addComponent(e, life);

            //create interactions
            Interactable interact = new Interactable();
            interact.SupportedInteractions.PROJECTILE_COLLIDABLE = true;
            interact.SupportedInteractions.ATTACKABLE = true;
            n_EcsInstance.EntityManager.addComponent(e, interact);

            //create test equipment
            ItemFactory iFactory = new ItemFactory(n_EcsInstance);
            n_EcsInstance.EntityManager.addComponent(e, iFactory.createTestEquipment());

            //setup experiences
            Experiences experiences = new Experiences();
            Experience xp = new Experience(50);
            experiences.GeneralExperience.Add(MobGroup.Test, xp);
            n_EcsInstance.EntityManager.addComponent(e, experiences);

            //setup attributes
            Attributes attributes = new Attributes();
            attributes.Endurance.Value = 50;
            attributes.Mind.Value = 50;
            attributes.Muscle.Value = 50;
            attributes.Perception.Value = 50;
            attributes.Quickness.Value = 50;
            n_EcsInstance.EntityManager.addComponent(e, attributes);

            //setup skills
            Skill skill = new Skill("Avoidance", 50, SkillType.Offensive);
            Skills skills = new Skills();
            skills.SkillSet.Add(SkillNames.Avoidance, skill);
            skill = new Skill("Ranged", 50, SkillType.Offensive);
            skills.SkillSet.Add(SkillNames.Ranged, skill);
            n_EcsInstance.EntityManager.addComponent(e, skills);

            Faction faction = new Faction(100, FactionType.Ally);
            Faction enemy = new Faction(-10, FactionType.TestMob);

            Factions factions = new Factions();
            factions.OwnerFaction = faction;
            factions.KnownFactions.Add(enemy.FactionType, enemy);
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

            //create health
            Health health = new Health(100);
            health.RecoveryAmmount = 10;
            health.RecoveryRate = 500;
            n_EcsInstance.EntityManager.addComponent(e, health);

            //create life
            Life life = new Life();
            life.IsAlive = true;
            life.DeathLongevity = 2000;
            n_EcsInstance.EntityManager.addComponent(e, life);

            //create interactions
            Interactable interact = new Interactable();
            interact.SupportedInteractions.PROJECTILE_COLLIDABLE = true;
            interact.SupportedInteractions.ATTACKABLE = true;
            n_EcsInstance.EntityManager.addComponent(e, interact);

            //create test equipment
            ItemFactory iFactory = new ItemFactory(n_EcsInstance);
            n_EcsInstance.EntityManager.addComponent(e, iFactory.createTestEquipment());

            //setup experiences
            Experiences experiences = new Experiences();
            Experience xp = new Experience(50);
            experiences.GeneralExperience.Add(MobGroup.Test, xp);
            n_EcsInstance.EntityManager.addComponent(e, experiences);

            //setup attributes
            Attributes attributes = new Attributes();
            attributes.Endurance.Value = 50;
            attributes.Mind.Value = 50;
            attributes.Muscle.Value = 50;
            attributes.Perception.Value = 50;
            attributes.Quickness.Value = 50;
            n_EcsInstance.EntityManager.addComponent(e, attributes);

            //setup skills
            Skill skill = new Skill("Avoidance", 50, SkillType.Offensive);
            Skills skills = new Skills();
            skills.SkillSet.Add(SkillNames.Avoidance, skill);
            
            skill = new Skill("Ranged", 50, SkillType.Offensive);
            skills.SkillSet.Add(SkillNames.Ranged, skill);
            n_EcsInstance.EntityManager.addComponent(e, skills);

            //setup factions
            Faction faction = new Faction(100,FactionType.TestMob);
            Factions factions = new Factions();
            factions.OwnerFaction = faction;
            n_EcsInstance.EntityManager.addComponent(e, factions);

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
