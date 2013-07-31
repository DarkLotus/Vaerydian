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

namespace Vaerydian.Utils
{
    class KeyFrame
    {
        public KeyFrame(float keyTime, Vector2 keyPosition, float keyRotation)
        {
            k_KeyPercent = keyTime;
            k_KeyPosition = keyPosition;
            k_KeyRotation = keyRotation;
        }

        private Vector2 k_KeyPosition = Vector2.Zero;

        public Vector2 KeyPosition
        {
            get { return k_KeyPosition; }
            set { k_KeyPosition = value; }
        }

        private float k_KeyRotation = 0f;

        public float KeyRotation
        {
            get { return k_KeyRotation; }
            set { k_KeyRotation = value; }
        }

        private float k_KeyPercent = 0;

        public float KeyPercent
        {
            get { return k_KeyPercent; }
            set { k_KeyPercent = value; }
        }

    }
}
