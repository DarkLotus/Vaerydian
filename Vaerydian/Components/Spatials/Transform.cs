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
    public class Transform : IComponent
    {
        private static int t_TypeID;
        private int t_EntityID;

        public Transform() { }

        public int getEntityId()
        {
            return t_EntityID;
        }

        public int getTypeId()
        {
            return t_TypeID;
        }

        public void setEntityId(int entityId)
        {
            t_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            t_TypeID = typeId;
        }

        private float t_Rotation = 0f;

        public float Rotation
        {
            get { return t_Rotation; }
            set { t_Rotation = value; }
        }

        private Vector2 t_RotationOrigin = new Vector2(0);

        public Vector2 RotationOrigin
        {
            get { return t_RotationOrigin; }
            set { t_RotationOrigin = value; }
        }

    }
}
