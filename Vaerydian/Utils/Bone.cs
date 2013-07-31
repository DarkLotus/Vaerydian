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
using Microsoft.Xna.Framework.Graphics;

namespace Vaerydian.Utils
{
    class Bone
    {
        private int a_ElapsedTime;

        public int ElapsedTime
        {
            get { return a_ElapsedTime; }
            set { a_ElapsedTime = value; }
        }

        private int a_AnimationTime;

        public int AnimationTime
        {
            get { return a_AnimationTime; }
            set { a_AnimationTime = value; }
        }

        private Dictionary<String, List<KeyFrame>> a_Animations = new Dictionary<string, List<KeyFrame>>();

        public Dictionary<String, List<KeyFrame>> Animations
        {
            get { return a_Animations; }
            set { a_Animations = value; }
        }

        private String a_TextureName;

        public String TextureName
        {
            get { return a_TextureName; }
            set { a_TextureName = value; }
        }

        private Vector2 a_Origin;

        public Vector2 Origin
        {
            get { return a_Origin; }
            set { a_Origin = value; }
        }

        private Vector2 a_RotationOrigin;

        public Vector2 RotationOrigin
        {
            get { return a_RotationOrigin; }
            set { a_RotationOrigin = value; }
        }

        private float a_Rotation;

        public float Rotation
        {
            get { return a_Rotation; }
            set { a_Rotation = value; }
        }

        
    }
}
