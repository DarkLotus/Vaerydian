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

using Vaerydian.Characters;

namespace Vaerydian.Components.Characters
{
	public struct StatisticsDef{
		public string Name;
		public Statistic Muscle;
		public Statistic Endurance;
		public Statistic Mind;
		public Statistic Personality;
		public Statistic Quickness;
		public Statistic Perception;
		public Statistic Focus;
	}

    class Statistics : IComponent
    {
        private static int a_TypeID;
        private int a_EntityID;

        public Statistics() { }

        public int getEntityId()
        {
            return a_EntityID;
        }

        public int getTypeId()
        {
            return a_TypeID;
        }

        public void setEntityId(int entityId)
        {
            a_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            a_TypeID = typeId;
        }

        /// <summary>
        /// muscle of entity
        /// </summary>
		public Statistic Muscle;
        /// <summary>
        /// endurance of entity
        /// </summary>
		public Statistic Endurance;
        /// <summary>
        /// mind of entity
        /// </summary>
		public Statistic Mind;
        /// <summary>
        /// personality of entity
        /// </summary>
		public Statistic Personality;
        /// <summary>
        /// quickness of entity
        /// </summary>
		public Statistic Quickness;
        /// <summary>
		/// perception of entity
        /// </summary>
		public Statistic Perception;
		/// <summary>
		/// focus of the entity
		/// </summary>
		public Statistic Focus;

    }
}
