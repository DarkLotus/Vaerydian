using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

using AgentComponentBus.Core;

using Vaerydian.Components.Utils;
using Vaerydian.Utils;

namespace Vaerydian.ACB
{
    class PathFinder : BusComponent
    {
        private AStarPathing pathing;
        
        protected override Bag<IComponent> requestRetrievePacakge(Agent agent)
        {
            Bag<IComponent> components = new Bag<IComponent>();

            Path path = new Path();
            components.Set(path.getTypeId(), path);

            return components;
        }

        protected override Bag<IComponent> runProcess(Agent agent, Bag<IComponent> components)
        {
            Path path = new Path();
            path = (Path)components.Get(path.getTypeId());

            //dont run if there is no path
            if (path == null)
                return components;

            //dont run if you've already run...
            if (path.HasRun)
                return components;

            pathing = new AStarPathing(path.Start, path.Finish, path.Map);

            while(!pathing.Failed && !pathing.IsFound)
            {
                pathing.findPath();
            }

            if (pathing.Failed)
            {
                path.PathState = PathState.PathFailed;
                return components;
            }

            if (pathing.IsFound)
            {
                path.FoundPath = pathing.getPath();
                path.PathState = PathState.PathFound;
            }

            return components;
        }
    }
}
