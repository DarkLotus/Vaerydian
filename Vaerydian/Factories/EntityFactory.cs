/*
 Author:
      Thomas H. Jonell <thjonell@gmail.com>
 
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ECSFramework;

using Vaerydian.Characters;

using Vaerydian.Components;
using Vaerydian.Components.Dbg;
using Vaerydian.Behaviors;
using Vaerydian.Utils;
using Vaerydian.Factories;


using Vaerydian.Components.Characters;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Graphical;
using Vaerydian.Components.Actions;
using Vaerydian.Characters;
using Vaerydian.Sessions;


namespace Vaerydian.Factories
{
    static class EntityFactory
    {
        public static ECSInstance ECSInstance;
        public static GameContainer GameContainer;
        private static Random rand = new Random();

        public static Entity createPlayer(int skillLevel)
        {
            Entity e = ECSInstance.create();

			GameMap gameMap = ComponentMapper.get<GameMap> (ECSInstance.TagManager.getEntityByTag ("MAP"));
			Vector2 pos = MapFactory.findSafeLocation (gameMap);

            //ECSInstance.EntityManager.addComponent(e, new Position(new Vector2(576f, 360f),new Vector2(12.5f)));
			ECSInstance.EntityManager.addComponent(e, new Position(pos,new Vector2(16)));
            //ECSInstance.EntityManager.addComponent(e, new Position(new Vector2(0, 0), new Vector2(12.5f)));
			ECSInstance.EntityManager.addComponent(e, new Velocity(4f));
			ECSInstance.EntityManager.addComponent(e, new Controllable());
            //ECSInstance.EntityManager.addComponent(e, new Sprite("characters\\lord_lard_sheet", "characters\\normals\\lord_lard_sheet_normals",32,32,0,0));;
            
			ECSInstance.EntityManager.addComponent(e, AnimationFactory.createPlayerAnimation());
			ECSInstance.EntityManager.addComponent(e, new CameraFocus(75));
            ECSInstance.EntityManager.addComponent(e, new MapCollidable());
			ECSInstance.EntityManager.addComponent(e, new Heading());
			ECSInstance.EntityManager.addComponent(e, createLight(true, 8, new Vector3(new Vector2(576f, 360f), 10), 0.5f, new Vector4(1, 1, .6f, 1)));
			ECSInstance.EntityManager.addComponent(e, new Transform());

            Information info = new Information();
			info.CreatureGeneralGroup = "HUMAN";
			info.CreatureVariationGroup = "NONE";
			info.CreatureUniqueGroup = "NONE";
            info.Name = "PLAYER";
			ECSInstance.EntityManager.addComponent(e, info);

            //create life
            Life life = new Life();
            life.IsAlive = true;
            life.DeathLongevity = 1000;
			ECSInstance.EntityManager.addComponent(e, life);

            //create interactions
            Interactable interact = new Interactable();
            interact.SupportedInteractions.PROJECTILE_COLLIDABLE = true;
            interact.SupportedInteractions.ATTACKABLE = true;
            interact.SupportedInteractions.MAY_RECEIVE_VICTORY = true;
            interact.SupportedInteractions.MAY_ADVANCE = true;
            interact.SupportedInteractions.CAUSES_ADVANCEMENT = false;
            interact.SupportedInteractions.AWARDS_VICTORY = false;
			ECSInstance.EntityManager.addComponent(e, interact);

            //create test equipment
            ItemFactory iFactory = new ItemFactory(ECSInstance);
			ECSInstance.EntityManager.addComponent(e, iFactory.createTestEquipment());

            //setup experiences
            Knowledges knowledges = new Knowledges();
			knowledges.GeneralKnowledge.Add ("HUMAN", new Knowledge { Name="", Value=skillLevel, KnowledgeType=KnowledgeType.General });
			knowledges.GeneralKnowledge.Add ("BAT", new Knowledge { Name="", Value=skillLevel, KnowledgeType=KnowledgeType.General });
			knowledges.VariationKnowledge.Add ("NONE", new Knowledge { Name="", Value=0f, KnowledgeType=KnowledgeType.General });
			knowledges.UniqueKnowledge.Add ("NONE", new Knowledge { Name="", Value=0f, KnowledgeType=KnowledgeType.General });
			ECSInstance.EntityManager.addComponent(e, knowledges);

            //setup attributes
            Statistics statistics = new Statistics();
			statistics.Focus = new Statistic {Name="FOCUS",Value=skillLevel,StatType=StatType.FOCUS };
			statistics.Endurance = new Statistic {Name= "ENDURANCE",Value= skillLevel,StatType= StatType.ENDURANCE };
			statistics.Mind = new Statistic {Name= "MIND",Value= skillLevel,StatType= StatType.MIND };
			statistics.Muscle = new Statistic {Name= "MUSCLE",Value= skillLevel,StatType= StatType.MUSCLE };
			statistics.Perception = new Statistic {Name= "PERCEPTION",Value= skillLevel,StatType= StatType.PERCEPTION };
			statistics.Personality = new Statistic {Name= "PERSONALITY",Value= skillLevel,StatType= StatType.PERSONALITY };
			statistics.Quickness = new Statistic {Name= "QUICKNESS",Value= skillLevel,StatType= StatType.QUICKNESS };
			ECSInstance.EntityManager.addComponent(e, statistics);

            //create health
			Health health = new Health(statistics.Endurance.Value * 5);// new Health(5000);//
			health.RecoveryAmmount = statistics.Endurance.Value / 5;
            health.RecoveryRate = 1000;
			ECSInstance.EntityManager.addComponent(e, health);

            //setup skills
            Skills skills = new Skills();
			skills.Ranged = new Skill{Name="RANGED",Value= skillLevel,SkillType= SkillType.Offensive};
			skills.Avoidance = new Skill{Name="AVOIDANCE",Value= skillLevel,SkillType= SkillType.Defensive};
			skills.Melee = new Skill{Name="MELEE",Value= skillLevel,SkillType= SkillType.Offensive};
			ECSInstance.EntityManager.addComponent(e, skills);

            Factions factions = new Factions();
			factions.OwnerFaction = new Faction{Name="PLAYER",Value=100,FactionType= FactionType.Player};
			factions.KnownFactions.Add("WILDERNESS", new Faction{Name="WILDERNESS",Value=-10,FactionType= FactionType.Wilderness});
			factions.KnownFactions.Add("ALLY", new Faction{Name="ALLY",Value=100,FactionType=FactionType.Ally});
			ECSInstance.EntityManager.addComponent(e, factions);

            GameSession.PlayerState = new PlayerState();
            GameSession.PlayerState.Statistics = statistics;
            GameSession.PlayerState.Factions = factions;
            GameSession.PlayerState.Health = health;
            GameSession.PlayerState.Information = info;
            GameSession.PlayerState.Interactable = interact;
            GameSession.PlayerState.Knowledges = knowledges;
            GameSession.PlayerState.Life = life;
            GameSession.PlayerState.Skills = skills;

			ECSInstance.TagManager.tagEntity("PLAYER", e);

			ECSInstance.refresh(e);

			return e;

        }

        public static Entity recreatePlayer(PlayerState playerHolder, Position position)
        {
			Entity e = ECSInstance.create();

			ECSInstance.EntityManager.addComponent(e, position);
            //ECSInstance.EntityManager.addComponent(e, new Position(new Vector2(0, 0), new Vector2(12.5f)));
			ECSInstance.EntityManager.addComponent(e, new Velocity(4f));
			ECSInstance.EntityManager.addComponent(e, new Controllable());
            //ECSInstance.EntityManager.addComponent(e, new Sprite("characters\\lord_lard_sheet", "characters\\normals\\lord_lard_sheet_normals",32,32,0,0));
			ECSInstance.EntityManager.addComponent(e, AnimationFactory.createPlayerAnimation()); ECSInstance.EntityManager.addComponent(e, new CameraFocus(75));
			ECSInstance.EntityManager.addComponent(e, new MapCollidable());
			ECSInstance.EntityManager.addComponent(e, new Heading());
            ECSInstance.EntityManager.addComponent(e, createLight(true, 8, new Vector3(new Vector2(576f, 360f), 10), 0.5f, new Vector4(1, 1, .6f, 1)));
			ECSInstance.EntityManager.addComponent(e, new Transform());

            /* LIKELY NOT NEEDED
            GameSession.PlayerState.Attributes.setEntityId(e.Id);
            GameSession.PlayerState.Factions.setEntityId(e.Id);
            GameSession.PlayerState.Health.setEntityId(e.Id);
            GameSession.PlayerState.Information.setEntityId(e.Id);
            GameSession.PlayerState.Interactable.setEntityId(e.Id);
            GameSession.PlayerState.Knowledges.setEntityId(e.Id);
            GameSession.PlayerState.Life.setEntityId(e.Id);
            GameSession.PlayerState.Skills.setEntityId(e.Id);
            */

			ECSInstance.EntityManager.addComponent(e, playerHolder.Information);
			ECSInstance.EntityManager.addComponent(e, playerHolder.Life);
			ECSInstance.EntityManager.addComponent(e, playerHolder.Interactable);

            ItemFactory iFactory = new ItemFactory(ECSInstance);
			ECSInstance.EntityManager.addComponent(e, iFactory.createTestEquipment());

			ECSInstance.EntityManager.addComponent(e, playerHolder.Knowledges);
			ECSInstance.EntityManager.addComponent(e, playerHolder.Statistics);
			ECSInstance.EntityManager.addComponent(e, playerHolder.Health);
			ECSInstance.EntityManager.addComponent(e, playerHolder.Skills);
			ECSInstance.EntityManager.addComponent(e, playerHolder.Factions);

			ECSInstance.TagManager.tagEntity("PLAYER", e);

			ECSInstance.refresh(e);

            return e;
        }

        public static void createCamera()
        {
			Entity e = ECSInstance.create();
			ECSInstance.EntityManager.addComponent(e, new Velocity(5f));
            //ECSInstance.EntityManager.addComponent(e, new ViewPort(new Vector2(576, 360f), new Vector2(1152, 720)));
			ECSInstance.EntityManager.addComponent(e, new ViewPort(new Vector2(0, 0), new Vector2(EntityFactory.GameContainer.GraphicsDevice.Viewport.Width, EntityFactory.GameContainer.GraphicsDevice.Viewport.Height)));

			ECSInstance.TagManager.tagEntity("CAMERA", e);

			ECSInstance.refresh(e);
           
        }

        public static void createMousePointer()
        {
			Entity e = ECSInstance.create();
			ECSInstance.EntityManager.addComponent(e, new Position(new Vector2(0), new Vector2(5)));
            ECSInstance.EntityManager.addComponent(e, new Sprite("pointer", "pointer", 10, 10, 0, 0));
			ECSInstance.EntityManager.addComponent(e, new MousePosition());
			//ECSInstance.EntityManager.addComponent(e, createLight(true, 2, new Vector3(576, 360, 50), 0.3f, new Vector4(1f, 1f, 0.6f, 1f)));
			ECSInstance.EntityManager.addComponent(e, new Transform());

			ECSInstance.TagManager.tagEntity("MOUSE", e);

			ECSInstance.refresh(e);
        }

 		
        public static void createMapDebug()
        {
			Entity e = ECSInstance.create();

			ECSInstance.EntityManager.addComponent(e, new MapDebug());

			ECSInstance.TagManager.tagEntity("MAP_DEBUG", e);

			ECSInstance.refresh(e);

        }


        public static Entity createGeometryMap()
        {
			Entity e = ECSInstance.create();

            GeometryMap geometry = new GeometryMap();

            PresentationParameters pp = GameContainer.SpriteBatch.GraphicsDevice.PresentationParameters;
            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;
            SurfaceFormat format = pp.BackBufferFormat;

            geometry.ColorMap = new RenderTarget2D(GameContainer.SpriteBatch.GraphicsDevice, width, height);
            geometry.NormalMap = new RenderTarget2D(GameContainer.SpriteBatch.GraphicsDevice, width, height);
            geometry.DepthMap = new RenderTarget2D(GameContainer.SpriteBatch.GraphicsDevice, width, height);
            geometry.ShadingMap = new RenderTarget2D(GameContainer.SpriteBatch.GraphicsDevice, width, height, false, format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

            geometry.AmbientColor = new Vector4(.1f, .1f, .1f, .1f);

			ECSInstance.EntityManager.addComponent(e, geometry);

			ECSInstance.TagManager.tagEntity("GEOMETRY", e);

			ECSInstance.refresh(e);

            return e;
        }

        public static Light createLight(bool enabled, int radius, Vector3 position, float power, Vector4 color)
        {
            Light light = new Light();

            light.IsEnabled = enabled;
            light.LightRadius = radius;
            light.Position = position;
            light.ActualPower = power;
            light.Color = color;

            return light;
        }

        public static void createStandaloneLight(bool enabled, int radius, Vector3 position, float power, Vector4 color)
        {
			Entity e = ECSInstance.create();

			ECSInstance.EntityManager.addComponent(e, createLight(enabled, radius, position, power, color));
			ECSInstance.EntityManager.addComponent(e, new Position(new Vector2(position.X, position.Y),Vector2.Zero));

			ECSInstance.refresh(e);
        }

        public static void createRandomLight()
        {
			Entity e = ECSInstance.create();

            

            Vector3 pos = new Vector3(rand.Next(100*25), rand.Next(100*25), 50);

            //1152, 720
			ECSInstance.EntityManager.addComponent(e, createLight(true, 
                                                        rand.Next(5)+5,
                                                        pos, 
                                                        (float)rand.NextDouble()*.5f+0.5f,
                                                        new Vector4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble())));
			ECSInstance.EntityManager.addComponent(e, new Position(new Vector2(pos.X, pos.Y), Vector2.Zero));

			ECSInstance.refresh(e);

        }

        public static void createSonicProjectile(Vector2 start, Vector2 heading, float velocity, int duration, Light light, Transform transform, Entity originator)
        {
			Entity e = ECSInstance.create();

			ECSInstance.EntityManager.addComponent(e, new Position(start,new Vector2(16)));
			ECSInstance.EntityManager.addComponent(e, new Heading(heading));
            Sprite sprite = new Sprite("sonic_attack", "sonic_attack", 32, 32, 0, 0);
            sprite.SpriteAnimation = new SpriteAnimation(4, 16);
            sprite.ShouldSystemAnimate = true;
			ECSInstance.EntityManager.addComponent(e, sprite);
            ECSInstance.EntityManager.addComponent(e, new Velocity(velocity));
            
            Projectile projectile = new Projectile(duration);
            projectile.Originator = originator;
            ECSInstance.EntityManager.addComponent(e, projectile);
            
            ECSInstance.EntityManager.addComponent(e, new MapCollidable());
            ECSInstance.EntityManager.addComponent(e, transform);

            if (light != null)
                ECSInstance.EntityManager.addComponent(e, light);

            ECSInstance.refresh(e);

        }

        public static void createCollidingProjectile(Vector2 start, Vector2 heading, float velocity, int duration, Light light, Transform transform, Entity originator)
        {
            Entity e = ECSInstance.create();

            ECSInstance.EntityManager.addComponent(e, new Position(start,new Vector2(16)));
            ECSInstance.EntityManager.addComponent(e, new Heading(heading));
            ECSInstance.EntityManager.addComponent(e,  new Sprite("projectile", "projectile", 32, 32, 0, 0));
            ECSInstance.EntityManager.addComponent(e, new Velocity(velocity));
            
            Projectile projectile = new Projectile(duration);
            projectile.Originator = originator;
            ECSInstance.EntityManager.addComponent(e, projectile);
            
            ECSInstance.EntityManager.addComponent(e, new MapCollidable());
            ECSInstance.EntityManager.addComponent(e, transform);

            if (light != null)
                ECSInstance.EntityManager.addComponent(e, light);

            ECSInstance.refresh(e);

        }

        public static void createProjectile(Vector2 start, Vector2 heading, float velocity, int duration, Light light, Transform transform, Entity originator)
        {
            Entity e = ECSInstance.create();

            ECSInstance.EntityManager.addComponent(e, new Position(start, new Vector2(16)));
            ECSInstance.EntityManager.addComponent(e, new Heading(heading));
            ECSInstance.EntityManager.addComponent(e, new Sprite("projectile", "projectile", 32, 32, 0, 0));
            ECSInstance.EntityManager.addComponent(e, new Velocity(velocity));

            Projectile projectile = new Projectile(duration);
            projectile.Originator = originator;
            ECSInstance.EntityManager.addComponent(e, projectile);
            
            ECSInstance.EntityManager.addComponent(e, transform);
            //ECSInstance.EntityManager.addComponent(e, new MapCollidable());

            if (light != null)
                ECSInstance.EntityManager.addComponent(e, light);

            ECSInstance.refresh(e);

        }

        public static void createSpatialPartition(Vector2 ul, Vector2 lr, int tiers)
        {
            Entity e = ECSInstance.create();

            SpatialPartition spatial = new SpatialPartition();

            spatial.QuadTree = new QuadTree<Entity>(ul, lr);
            spatial.QuadTree.buildQuadTree(tiers);

            ECSInstance.EntityManager.addComponent(e, spatial);

            ECSInstance.TagManager.tagEntity("SPATIAL", e);

            ECSInstance.refresh(e);
        }




    }
}
