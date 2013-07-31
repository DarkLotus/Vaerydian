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

using Vaerydian.Utils;

namespace Vaerydian.Components.Utils
{
    class BoundingPolygon : IComponent
    {

        private static int b_TypeID;
        private int b_EntityID;

        public BoundingPolygon() { }

        public int getEntityId()
        {
            return b_EntityID;
        }

        public int getTypeId()
        {
            return b_TypeID;
        }

        public void setEntityId(int entityId)
        {
            b_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            b_TypeID = typeId;
        }

        private Polygon b_Polygon;

        internal Polygon Polygon
        {
            get { return b_Polygon; }
            set { b_Polygon = value; }
        }
    }
}
