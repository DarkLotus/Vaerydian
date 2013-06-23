using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using AgentComponentBus.Core;
using AgentComponentBus.Components;


namespace Vaerydian.Factories
{
	static class AgentFactory
    {
		public static ECSInstance ECSInstance;

		public static BusAgent createBatAgent(Entity entity)
        {
            BusAgent busAgent = new BusAgent();

			Agent agent = ResourcePool.createAgent ();


            busAgent.Agent.Entity = entity;


            return busAgent;
        }

    }
}
