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

namespace Vaerydian.Components.Characters
{
	public struct LifeDef{
		public int DeathLongevity;
	}

    class Life : IComponent
    {

        private static int l_TypeID;
        private int l_EntityID;

        public Life() { }

        public int getEntityId()
        {
            return l_EntityID;
        }

        public int getTypeId()
        {
            return l_TypeID;
        }

        public void setEntityId(int entityId)
        {
            l_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            l_TypeID = typeId;
        }

        private bool d_IsAlive = true;
        /// <summary>
        /// is the entity currently alive
        /// </summary>
        public bool IsAlive
        {
            get { return d_IsAlive; }
            set { d_IsAlive = value; }
        }

        private int d_TimeSinceDeath = 0;

        /// <summary>
        /// total time spent "dead"
        /// </summary>
        public int TimeSinceDeath
        {
            get { return d_TimeSinceDeath; }
            set { d_TimeSinceDeath = value; }
        }

        private int d_DeathLongevity;
        /// <summary>
        /// total time allowed to be "dead"
        /// </summary>
        public int DeathLongevity
        {
            get { return d_DeathLongevity; }
            set { d_DeathLongevity = value; }
        }


    }
}
