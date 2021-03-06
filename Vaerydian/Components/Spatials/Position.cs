﻿/*
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

namespace Vaerydian.Components.Spatials
{
    public class Position : IComponent
    {
        private static int p_TypeID;

        public static int TypeID
        {
            get { return Position.p_TypeID; }
            set { Position.p_TypeID = value; }
        }

        private int p_EntityID;

        private Vector2 p_Position;

        public Vector2 Pos
        {
            get { return p_Position; }
            set { p_Position = value; }
        }

        private Vector2 p_Offset;

        public Vector2 Offset
        {
            get { return p_Offset; }
            set { p_Offset = value; }
        }

        public Position() { }

        public Position(Vector2 position, Vector2 offset)
        {
            p_Position = position;
            p_Offset = offset;
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

    }
}
