﻿/*
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
