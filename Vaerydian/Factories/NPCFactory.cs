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
using AgentComponentBus.Components.ACB;
using AgentComponentBus.Components.ECS;
using AgentComponentBus.Core;
using Vaerydian.ACB;

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
            n_EcsInstance.EntityManager.addComponent(e, new AiBehavior(new FollowerBehavior(e,target,distance,n_EcsInstance)));//new FollowPath(e, target, distance, n_EcsInstance)));
            n_EcsInstance.EntityManager.addComponent(e, new MapCollidable());
            n_EcsInstance.EntityManager.addComponent(e, new Heading());
            n_EcsInstance.EntityManager.addComponent(e, new Transform());
            n_EcsInstance.EntityManager.addComponent(e, new Aggrivation());
            n_EcsInstance.EntityManager.addComponent(e, new Path());

            /*
            //setup pathing agent
            BusAgent busAgent = new BusAgent();
            busAgent.Agent = new Agent();
            busAgent.Agent.Entity = e;

            Activity activity = new Activity();
            activity.ActivityName = "activity1";
            activity.ComponentName = "PATH_FINDER";
            activity.InitialActivity = true;
            activity.NextActivity = "activity1";

            AgentProcess process = new AgentProcess();
            process.ProcessName = "path process";
            process.Activities.Add(activity.ActivityName, activity);

            busAgent.Agent.AgentProcess = process;

            n_EcsInstance.EntityManager.addComponent(e, busAgent);
            */

            //create info
            Information info = new Information();
            info.Name = "TEST FOLLOWER";
            info.CreatureGeneralGroup = CreatureGeneralGroup.Bat;
            info.CreatureVariationGroup = CreatureVariationGroup.None;
            info.CreatureUniqueGroup = CreatureUniqueGroup.None;
            n_EcsInstance.EntityManager.addComponent(e, info);

            //create life
            Life life = new Life();
            life.IsAlive = true;
            life.DeathLongevity = 500;
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
            Faction enemy = new Faction(-10, FactionType.Wilderness);
            Faction player = new Faction(100, FactionType.Player);

            Factions factions = new Factions();
            factions.OwnerFaction = faction;
            factions.KnownFactions.Add(enemy.FactionType, enemy);
            factions.KnownFactions.Add(player.FactionType, player);
            n_EcsInstance.EntityManager.addComponent(e, factions);


			n_EcsInstance.EntityManager.addComponent(e, EntityFactory.createLight(true, 100, new Vector3(position, 10), 0.5f, new Vector4(1, 1, 1, 1)));


            n_EcsInstance.refresh(e);

        }


        public void createBatEnemy(Vector2 position, int skillLevel)
        {
            Entity e = n_EcsInstance.create();

            n_EcsInstance.EntityManager.addComponent(e, new Position(position, new Vector2(16)));
            n_EcsInstance.EntityManager.addComponent(e, new Velocity(3f));
            n_EcsInstance.EntityManager.addComponent(e, new AiBehavior(new WanderingEnemyBehavior(e, n_EcsInstance)));
            n_EcsInstance.EntityManager.addComponent(e, new MapCollidable());
            n_EcsInstance.EntityManager.addComponent(e, new Heading());
            n_EcsInstance.EntityManager.addComponent(e, new Transform());
            n_EcsInstance.EntityManager.addComponent(e, new Aggrivation());

            //create state machine
            StateMachine<EnemyState,EnemyState> stateMachine = new StateMachine<EnemyState, EnemyState>(EnemyState.Idle, BatHSM.whenIdle, EnemyState.Idle);

            //define states
            stateMachine.addState(EnemyState.Wandering, BatHSM.whenWandering);
            stateMachine.addState(EnemyState.Following, BatHSM.whenFollowing);
            
            //define transitions
            stateMachine.addStateChange(EnemyState.Idle, EnemyState.Wandering, EnemyState.Wandering);
            stateMachine.addStateChange(EnemyState.Wandering, EnemyState.Following, EnemyState.Following);
            stateMachine.addStateChange(EnemyState.Following, EnemyState.Wandering, EnemyState.Wandering);

            StateContainer<EnemyState, EnemyState> stateContainer = new StateContainer<EnemyState, EnemyState>();
            stateContainer.StateMachine = stateMachine;

            n_EcsInstance.EntityManager.addComponent(e, stateContainer);
            

            //create ACB component
            BusAgent busAgent = new BusAgent();
            busAgent.Agent = new Agent();
            busAgent.Agent.Entity = e;

            Activity activity = new Activity();
            activity.ActivityName = "activity1";
            activity.ComponentName = "BAT_HSBM";
            activity.InitialActivity = true;
            activity.NextActivity = "activity1";

            AgentProcess process = new AgentProcess();
            process.ProcessName = "bat hsbm";
            process.Activities.Add(activity.ActivityName, activity);

            busAgent.Agent.AgentProcess = process;

            n_EcsInstance.EntityManager.addComponent(e, busAgent);

			n_EcsInstance.EntityManager.addComponent(e, AnimationFactory.createCharacter ("BAT"));

            //create info
            Information info = new Information();
            info.Name = "TEST WANDERER";
            info.CreatureGeneralGroup = CreatureGeneralGroup.Bat;
            info.CreatureVariationGroup = CreatureVariationGroup.None; 
            info.CreatureUniqueGroup = CreatureUniqueGroup.None;
            n_EcsInstance.EntityManager.addComponent(e, info);

            //create life
            Life life = new Life();
            life.IsAlive = true;
            life.DeathLongevity = 500;
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

            //setup experiences
            Knowledges knowledges = new Knowledges();
            knowledges.GeneralKnowledge.Add(CreatureGeneralGroup.Human, new Knowledge(skillLevel));
			knowledges.GeneralKnowledge.Add(CreatureGeneralGroup.Bat, new Knowledge(skillLevel));
            knowledges.VariationKnowledge.Add(CreatureVariationGroup.None, new Knowledge(0));
            knowledges.UniqueKnowledge.Add(CreatureUniqueGroup.None, new Knowledge(0));
            n_EcsInstance.EntityManager.addComponent(e, knowledges);

            //setup attributes
            Attributes attributes = new Attributes();

            attributes.AttributeSet.Add(AttributeType.Focus, skillLevel);
            attributes.AttributeSet.Add(AttributeType.Endurance, skillLevel);
            attributes.AttributeSet.Add(AttributeType.Mind, skillLevel);
            attributes.AttributeSet.Add(AttributeType.Muscle, skillLevel);
            attributes.AttributeSet.Add(AttributeType.Perception, skillLevel);
            attributes.AttributeSet.Add(AttributeType.Personality, skillLevel);
            attributes.AttributeSet.Add(AttributeType.Quickness, skillLevel);
            n_EcsInstance.EntityManager.addComponent(e, attributes);

            //create health
            Health health = new Health(attributes.AttributeSet[AttributeType.Endurance] * 5);
            health.RecoveryAmmount = attributes.AttributeSet[AttributeType.Endurance] / 5;
            health.RecoveryRate = 1000;
            n_EcsInstance.EntityManager.addComponent(e, health);

            //setup skills
            Skill skill = new Skill("Avoidance", skillLevel, SkillType.Offensive);
            Skills skills = new Skills();
            skills.SkillSet.Add(SkillName.Avoidance, skill);

            skill = new Skill("Ranged", skillLevel, SkillType.Offensive);
            skills.SkillSet.Add(SkillName.Ranged, skill);
            n_EcsInstance.EntityManager.addComponent(e, skills);

            //setup factions
            Faction faction = new Faction(100,FactionType.Wilderness);
            Faction player = new Faction(-10, FactionType.Player);
            Faction ally = new Faction(-10, FactionType.Ally);
            Factions factions = new Factions();
            
            factions.OwnerFaction = faction;
            factions.KnownFactions.Add(player.FactionType, player);
            factions.KnownFactions.Add(ally.FactionType, ally);
            n_EcsInstance.EntityManager.addComponent(e, factions);

            Aggrivation aggro = new Aggrivation();
            n_EcsInstance.EntityManager.addComponent(e, aggro);

			n_EcsInstance.EntityManager.addComponent(e, EntityFactory.createLight(true, 100, new Vector3(position, 10), 0.5f, new Vector4(1,1,.6f, 1)));

            n_EcsInstance.GroupManager.addEntityToGroup("WANDERERS", e);

            n_EcsInstance.refresh(e);
        }

        public void createWanders(int count, GameMap map, int skillLevel)
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
                        createBatEnemy(new Vector2(x * 32, y * 32), skillLevel);
                        placed = true;
                    }

                }

                placed = false;
            }
        }

        /// <summary>
        /// creates a trigger that spawns wanderers
        /// </summary>
        /// <param name="count">max number of wanderers to spawn</param>
        /// <param name="map">map they are spawned in</param>
        public void createWandererTrigger(int count, GameMap map, int skillLevel)
        {
            Entity e = n_EcsInstance.create();

            Trigger trigger = new Trigger(count, map, skillLevel);

            trigger.TriggerAction += OnTriggerActionCreateWanders;
            trigger.TimeDelay = 500;
            trigger.RecurrancePeriod = 10000;
            trigger.IsRecurring = true;

            n_EcsInstance.EntityManager.addComponent(e, trigger);

            n_EcsInstance.refresh(e);
        }

        /// <summary>
        /// handles the wanderer trigger actions
        /// </summary>
        /// <param name="ecsInstance"></param>
        /// <param name="parameters"></param>
        private void OnTriggerActionCreateWanders(ECSInstance ecsInstance, Object[] parameters)
        {
            int count = (int)parameters[0];
            GameMap map = (GameMap)parameters[1];
            int skillLevel = (int)parameters[2];

            Bag<Entity> wanderers = n_EcsInstance.GroupManager.getGroup("WANDERERS");

            int size = 0;

            if(wanderers != null)
                size = wanderers.Size();

            int create = count - size;

            if (create != 0)
                createWanders(create, map, skillLevel);
        }


    }
}
