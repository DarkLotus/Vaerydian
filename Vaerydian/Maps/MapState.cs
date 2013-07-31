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
using Vaerydian.Components.Spatials;
using Vaerydian.Utils;

namespace Vaerydian.Maps
{
    class MapState
    {

		private MapType m_MapType;

        public MapType MapType
        {
            get { return m_MapType; }
            set { m_MapType = value; }
        }

        private int m_SkillLevel;

        public int SkillLevel
        {
            get { return m_SkillLevel; }
            set { m_SkillLevel = value; }
        }

        private string m_LocationName;

        public string LocationName
        {
            get { return m_LocationName; }
            set { m_LocationName = value; }
        }

        private int m_Seed;

        public int Seed
        {
            get { return m_Seed; }
            set { m_Seed = value; }
        }

        private Position m_LastPlayerPosition;

        public Position LastPlayerPosition
        {
            get { return m_LastPlayerPosition; }
            set { m_LastPlayerPosition = value; }
        }


    }
}
