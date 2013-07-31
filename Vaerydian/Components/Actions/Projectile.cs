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

namespace Vaerydian.Components.Actions
{
    class Projectile : IComponent
    {
        private static int p_TypeID;
        private int p_EntityID;

        public Projectile() { }

        public Projectile(int lifetime)
        {
            p_LifeTime = lifetime;
        }

        public int getEntityId()
        {
            return p_EntityID;
        }

        public int getTypeId()
        {
            return p_TypeID;
        }

        public void setEntityId(int entityId)
        {
            p_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            p_TypeID = typeId;
        }

        private int p_LifeTime = 0;

        public int LifeTime
        {
            get { return p_LifeTime; }
            set { p_LifeTime = value; }
        }

        private int p_ElapsedTime = 0;

        public int ElapsedTime
        {
            get { return p_ElapsedTime; }
            set { p_ElapsedTime = value; }
        }

        private Entity p_Originator;

        public Entity Originator
        {
            get { return p_Originator; }
            set { p_Originator = value; }
        }
    }
}
