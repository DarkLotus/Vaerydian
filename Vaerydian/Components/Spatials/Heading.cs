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

using Microsoft.Xna.Framework;

namespace Vaerydian.Components.Spatials
{
    public class Heading : IComponent
    {
        private static int h_TypeID;
        private int h_EntityID;

        private Vector2 h_Heading;

        public Heading() { }

        public Heading(Vector2 heading)
        {
            h_Heading = heading;
        }

        public int getEntityId()
        {
            return h_EntityID;
        }

        public int getTypeId()
        {
            return h_TypeID;
        }

        public Vector2 getHeading()
        {
            return h_Heading;
        }

        public void setEntityId(int entityId)
        {
            h_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            h_TypeID = typeId;
        }

        public void setHeading(Vector2 heading)
        {
            h_Heading = heading;
        }

    }
}
