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
            e_EcsInstance.EntityManager.addComponent(e, new Position(new Vector2(50, 50)));
            e_EcsInstance.EntityManager.addComponent(e, new Velocity(1f));
            e_EcsInstance.EntityManager.addComponent(e, new Controllable());
            e_EcsInstance.EntityManager.addComponent(e, new Sprite("player"));
            e_EcsInstance.refresh(e);

            e_EcsInstance.TagManager.tagEntity("PLAYER", e);
        }

    }
}
