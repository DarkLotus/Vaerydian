using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

using Vaerydian.Components;
using Vaerydian.Behaviors;

using WorldGeneration.Cave;
using WorldGeneration.World;


namespace Vaerydian.Factories
{
    class EntityFactory
    {
        private ECSInstance e_EcsInstance;

        public EntityFactory(ECSInstance ecsInstance)
        {
            e_EcsInstance = ecsInstance;
        }

        public void createPlayer()
        {
            Entity e = e_EcsInstance.create();
            e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(540f, 337.5f),new Vector2(12.5f)));
            e_EcsInstance.EntityManager.addComponent(e, new Velocity(5f));
            e_EcsInstance.EntityManager.addComponent(e, new Controllable());
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("characters\\player"));
            e_EcsInstance.EntityManager.addComponent(e, new CameraFocus(150));
            e_EcsInstance.EntityManager.addComponent(e, new MapCollidable());
            e_EcsInstance.EntityManager.addComponent(e, new Heading());

            e_EcsInstance.TagManager.tagEntity("PLAYER", e);

            e_EcsInstance.refresh(e);

        }

        public void createCamera()
        {
            Entity e = e_EcsInstance.create();
            e_EcsInstance.EntityManager.addComponent(e, new Velocity(5f));
            e_EcsInstance.EntityManager.addComponent(e, new ViewPort(new Vector2(540f, 337.5f), new Vector2(1080, 675)));

            e_EcsInstance.TagManager.tagEntity("CAMERA", e);

            e_EcsInstance.refresh(e);
           
        }

        public void createBackground()
        {
            Entity e = e_EcsInstance.create();
            e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(0,0),new Vector2(0)));
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("Title"));

            e_EcsInstance.refresh(e);

        }

        public void createMousePointer()
        {
            Entity e = e_EcsInstance.create();
            e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(0), new Vector2(24)));
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("reticle"));
            e_EcsInstance.EntityManager.addComponent(e, new MousePosition());

            e_EcsInstance.TagManager.tagEntity("MOUSE", e);

            e_EcsInstance.refresh(e);
        }

        public void createFollower(Vector2 position, Entity target, float distance)
        {
            Entity e = e_EcsInstance.create();

            e_EcsInstance.EntityManager.addComponent(e, new Position(position, new Vector2(12.5f)));
            e_EcsInstance.EntityManager.addComponent(e, new Velocity(4f));
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("characters\\enemy"));
            e_EcsInstance.EntityManager.addComponent(e, new AiBehavior(new SimpleFollowBehavior(e, target, distance, e_EcsInstance)));
            e_EcsInstance.EntityManager.addComponent(e, new MapCollidable());
            e_EcsInstance.EntityManager.addComponent(e, new Heading());

            e_EcsInstance.refresh(e);

        }

        public void createCave()
        {
            Entity e = e_EcsInstance.create();
            CaveEngine ce = new CaveEngine();
            ce.generateCave(100, 100, 42, 4, 0.5);
            e_EcsInstance.EntityManager.addComponent(e, new CaveMap(ce));

            e_EcsInstance.TagManager.tagEntity("MAP", e);

            e_EcsInstance.refresh(e);
        }
    }
}
