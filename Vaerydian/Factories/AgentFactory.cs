using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using AgentComponentBus.Core;
using AgentComponentBus.Components.ECS;
using AgentComponentBus.Components.ACB;

namespace Vaerydian.Factories
{
	static class AgentFactory
    {
		public static ECSInstance ECSInstance;

		public static BusAgent createBatAgent(Entity entity)
        {
            BusAgent agent = new BusAgent();

            AgentProcess process = new AgentProcess();

            Activity followPath = new Activity();
            followPath.ActivityName = "followPath";
            followPath.ComponentName = "BAT_AGENT";
            followPath.NextActivity = "findPath";
            followPath.InitialActivity = true;

            Activity findPath = new Activity();
            findPath.ActivityName = "findPath";
            findPath.ComponentName = "PATH_FINDER";
            findPath.NextActivity = "followPath";
            followPath.InitialActivity = false;

            process.Activities.Add(followPath.ActivityName, followPath);
            process.Activities.Add(findPath.ActivityName, findPath);

            agent.Agent = new Agent();
            agent.Agent.Entity = entity;
            agent.Agent.AgentProcess = process;

            return agent;
        }

    }
}
