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

using ECSFramework;

using Vaerydian.Components;
using Vaerydian.Components.Characters;
using Vaerydian.Factories;
using Vaerydian.Screens;

namespace Vaerydian.Systems.Update
{
    class LifeSystem : EntityProcessingSystem
    {
        private ComponentMapper l_LifeMapper;
        private Entity l_Player;

        private NPCFactory l_NPCFactory;

        public override void initialize()
        {
            l_LifeMapper = new ComponentMapper(new Life(), e_ECSInstance);
            l_NPCFactory = new NPCFactory(e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            l_Player = e_ECSInstance.TagManager.getEntityByTag("PLAYER");
        }

        protected override void cleanUp(Bag<Entity> entities) { }

        protected override void process(Entity entity)
        {
            Life life = (Life)l_LifeMapper.get(entity);

            //if you're still alive, dont worry about it
            if (life.IsAlive)
                return;

            //don't decay the player yet
            if (entity == l_Player)
            {
                GameScreen.PLAYER_IS_DEAD = true;
                return;
            }

            life.TimeSinceDeath += e_ECSInstance.ElapsedTime;

            if (life.TimeSinceDeath > life.DeathLongevity)
            {
                //cleanup entity
                l_NPCFactory.destroyRelatedEntities(entity);

                //get rid of entity
                e_ECSInstance.deleteEntity(entity);
            }


        }


    }
}
