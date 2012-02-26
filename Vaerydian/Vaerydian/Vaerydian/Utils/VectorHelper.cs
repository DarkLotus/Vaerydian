using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Vaerydian.Utils
{
    public static class VectorHelper
    {

        public static float getAngle(Vector2 a, Vector2 b)
        {
            Vector2 ta = a;
            Vector2 tb = b;
            ta.Normalize();
            tb.Normalize();

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
            ta.Normalize();
            tb.Normalize();

            float dot = Vector2.Dot(ta, tb);

            return (float)Math.Acos(dot);

        }

        public static Vector2 rotateVector(Vector2 vector, float angle)
        {
            float x = vector.X * (float)Math.Cos(angle) - vector.Y * (float)Math.Sin(angle);
            float y = vector.X * (float)Math.Sin(angle) + vector.Y * (float)Math.Cos(angle);
            return new Vector2(x, y);
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
        /// <param name="a">vector to project onto b</param>
        /// <param name="b">axis of projection</param>
        /// <returns>projected vector</returns>
        public static float project(Vector2 a, Vector2 b)
        {
            return Math.Abs(Vector2.Dot(a, b));
        }

    }
}
