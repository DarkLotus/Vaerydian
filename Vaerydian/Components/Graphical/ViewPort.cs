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

namespace Vaerydian.Components.Graphical
{
    public class ViewPort : IComponent
    {

        private static int v_TypeID;
        private int v_EntityID;

        private Vector2 v_Origin;
        private Vector2 v_Dimensions;

        public ViewPort() { }

        public ViewPort(Vector2 origin, Vector2 dimensions) 
        {
            v_Origin = origin;
            v_Dimensions = dimensions;
        }

        public int getEntityId()
        {
            return v_EntityID;
        }

        public int getTypeId()
        {
            return v_TypeID;
        }

        public Vector2 getOrigin()
        {
            return v_Origin;
        }

        public Vector2 getDimensions()
        {
            return v_Dimensions;
        }

        public void setEntityId(int entityId)
        {
            v_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            v_TypeID = typeId;
        }

        public void setOrigin(Vector2 origin)
        {
            v_Origin = origin;
        }

        public void setDimensions(Vector2 dimensions)
        {
            v_Dimensions = dimensions;
        }
    }
}
