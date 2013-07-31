/*
 Author:
      Thomas H. Jonell <@Net_Gnome>
 
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

using ECSFramework;


using BehaviorLibrary;

using Vaerydian.Components;
using Vaerydian.Components.Characters;


namespace Vaerydian.Systems.Update
{
    class BehaviorSystem : EntityProcessingSystem
    {
        private ComponentMapper b_BehaviorMapper;
        private ComponentMapper b_LifeMapper;

        public BehaviorSystem() {}//: base(intervals) { }

        public override void initialize()
        {
            b_BehaviorMapper = new ComponentMapper(new AiBehavior(), e_ECSInstance);
            b_LifeMapper = new ComponentMapper(new Life(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {

        }

        protected override void cleanUp(Bag<Entity> entities) { }

        protected override void process(Entity entity)
        {
            AiBehavior aiBehavior = (AiBehavior)b_BehaviorMapper.get(entity);
            Life life = (Life)b_LifeMapper.get(entity);
            if (life.IsAlive)
                aiBehavior.Behavior.Behave();
            else
            {
                if (!aiBehavior.Behavior.IsClean)
                    aiBehavior.Behavior.deathCleanup();
            }
        }

        
    }
}
