using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

using Vaerydian.Components;

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
            e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(540f, 337.5f)));
            e_EcsInstance.EntityManager.addComponent(e, new Velocity(5f));
            e_EcsInstance.EntityManager.addComponent(e, new Controllable());
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("player"));
            e_EcsInstance.EntityManager.addComponent(e, new CameraFocus(200));

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
            e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(0,0)));
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("Title"));

            e_EcsInstance.refresh(e);

        }

    }
}
