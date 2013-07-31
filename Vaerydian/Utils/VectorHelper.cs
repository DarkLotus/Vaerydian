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
    public static class VectorHelper
    {

        public static Vector2 normalize(Vector2 vec)
        {
            float val = (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);

            if (val == 0)
                return Vector2.Zero;

            return new Vector2(vec.X / val, vec.Y / val);
        }

        public static float getAngle(Vector2 a, Vector2 b)
        {
            Vector2 ta = a;
            Vector2 tb = b;
            ta = VectorHelper.normalize(ta);
            tb = VectorHelper.normalize(tb);
            //ta.Normalize();
            //tb.Normalize();

            float dot = Vector2.Dot(ta, tb);

            if (ta.Y > tb.Y)
                return (float)Math.Acos(dot);
            else
                return 6.283f - (float)Math.Acos(dot);
        }

        public static float getAngle2(Vector2 a, Vector2 b)
        {
            Vector2 ta = a;
            Vector2 tb = b;
            ta = VectorHelper.normalize(ta);
            tb = VectorHelper.normalize(tb);
            //ta.Normalize();
            //tb.Normalize();

            float dot = Vector2.Dot(ta, tb);

            return (float)Math.Acos(dot);

        }

        public static float getSignedAngle(Vector2 a, Vector2 b)
        {
            float perDot = a.X * b.Y - a.Y * b.X;
            return (float)Math.Atan2(perDot, Vector2.Dot(a, b));
        }

        public static Vector2 rotateVectorRadians(Vector2 vector, float angleRadians)
        {
            float x = vector.X * (float)Math.Cos(angleRadians) - vector.Y * (float)Math.Sin(angleRadians);
            float y = vector.X * (float)Math.Sin(angleRadians) + vector.Y * (float)Math.Cos(angleRadians);
            return new Vector2(x, y);
        }

        public static Vector2 rotateVectorDegrees(Vector2 vector, float angleDegrees)
        {
            float angle = ((2f * (float)Math.PI) / 360f) * angleDegrees;

            float x = vector.X * (float)Math.Cos(angle) - vector.Y * (float)Math.Sin(angle);
            float y = vector.X * (float)Math.Sin(angle) + vector.Y * (float)Math.Cos(angle);
            return new Vector2(x, y);
        }

        /// <summary>
        /// rotate a vector about an offset by a given angle
        /// </summary>
        /// <param name="vector">vector to be rotated</param>
        /// <param name="offset">the offset to be rotated around</param>
        /// <param name="angle">the angle of rotation (IN RADIANS)</param>
        /// <returns></returns>
        public static Vector2 rotateOffsetVectorRadians(Vector2 vector, Vector2 offset, float angle)
        {
            Vector2 rotVec = new Vector2();
            rotVec.X = (float)(offset.X + (vector.X - offset.X) * Math.Cos(angle) - (vector.X - offset.X) * Math.Sin(angle));
            rotVec.Y = (float)(offset.Y + (vector.Y - offset.Y) * Math.Cos(angle) + (vector.Y - offset.Y) * Math.Sin(angle));
            return rotVec;
        }

        /// <summary>
        /// rotate a vector about an offset by a given angle
        /// </summary>
        /// <param name="vector">vector to be rotated</param>
        /// <param name="offset">the offset to be rotated around</param>
        /// <param name="angle">the angle of rotation (IN DEGREES)</param>
        /// <returns></returns>
        public static Vector2 rotateOffsetVectorDegrees(Vector2 vector, Vector2 offset, float angle)
        {
            angle = (((float)Math.PI) / 180f) * angle;

            Vector2 rotVec = new Vector2();
            rotVec.X = (float)(offset.X + (vector.X - offset.X) * Math.Cos(angle) - (vector.X - offset.X) * Math.Sin(angle));
            rotVec.Y = (float)(offset.Y + (vector.Y - offset.Y) * Math.Cos(angle) + (vector.Y - offset.Y) * Math.Sin(angle));
            return rotVec;
        }

        public static Vector2 getRightNormal(Vector2 vector)
        {
            Vector2 returnVec;
            returnVec.X = -vector.Y;
            returnVec.Y = vector.X;
            return returnVec;
        }

        public static Vector2 getLeftNormal(Vector2 vector)
        {
            Vector2 returnVec;
            returnVec.X = vector.Y;
            returnVec.Y = -vector.X;
            return returnVec;
        }

        /// <summary>
        /// project vector a onto vector b
        /// </summary>
        /// <param name="vector">vector to project onto b</param>
        /// <param name="axis">axis of projection</param>
        /// <returns>projected vector</returns>
        public static float project(Vector2 vector, Vector2 axis)
        {
            //return Math.Abs(Vector2.Dot(vector, axis));
            return ((Vector2.Dot(vector, axis) / axis.LengthSquared()) * axis).Length();
        }

        

    }
}
