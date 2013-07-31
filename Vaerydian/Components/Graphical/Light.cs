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
    class Light : IComponent
    {
        private static int l_TypeID;
        private int l_EntityID;

        public Light() { }

        public int getEntityId()
        {
            return l_EntityID;
        }

        public int getTypeId()
        {
            return l_TypeID;
        }

		public static int TypeID {
			get {
				return l_TypeID;
			}
			set {
				l_TypeID = value;
			}
		}

        public void setEntityId(int entityId)
        {
            l_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            l_TypeID = typeId;
        }

        private bool l_IsEnabled;

        public bool IsEnabled
        {
            get { return l_IsEnabled; }
            set { l_IsEnabled = value; }
        }

        private float l_ActualPower;

        public float ActualPower
        {
            get { return l_ActualPower; }
            set { l_ActualPower = value; }
        }
        private Vector3 l_Position;

        public Vector3 Position
        {
            get { return l_Position; }
            set { l_Position = value; }
        }
        private Vector4 l_Color;

        public Vector4 Color
        {
            get { return l_Color; }
            set { l_Color = value; }
        }
        private int l_LightRadius;

        public int LightRadius
        {
            get { return l_LightRadius; }
            set { l_LightRadius = value; }
        }
    }
}
