using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Components;

using WorldGeneration.Cave;
using WorldGeneration.Utils;
using WorldGeneration.World;
using Vaerydian.Components.Utils;


namespace Vaerydian.Factories
{
    class MapFactory
    {

        private ECSInstance m_EcsInstance;
        private static GameContainer m_Container;
        private Random rand = new Random();

        public MapFactory(ECSInstance ecsInstance, GameContainer container)
        {
            m_EcsInstance = ecsInstance;
            m_Container = container;
        }

        public MapFactory(ECSInstance ecsInstance) 
        {
            m_EcsInstance = ecsInstance;
        }

        public void createWorld()
        {
            WorldGenerator wg = new WorldGenerator();
            //wg.generateNewWorld(0, 0, 512, 512, 4, 100, rand.Next(100));

            Entity e = m_EcsInstance.create();

            m_EcsInstance.EntityManager.addComponent(e, new GameMap(wg.getMap()));

            m_EcsInstance.TagManager.tagEntity("MAP", e);

            m_EcsInstance.refresh(e);

        }

    }
}
