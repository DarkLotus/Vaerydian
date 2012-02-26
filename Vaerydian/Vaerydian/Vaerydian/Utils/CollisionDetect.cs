using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;


namespace Vaerydian.Utils
{
    static class CollisionDetect
    {

        public static bool isColliding(Polygon A, Polygon B)
        {
            //setup temp variables
            float min = float.PositiveInfinity;
            Vector2 minAxis = Vector2.Zero;
            float BtoAlength, halfWidthA, halfHeightA, halfWidthB, halfHeightB, length;

            Vector2 BtoA = A.Center - B.Center;

            for(int i = 0; i < A.Normals.Length; i++)
            {
                for (int j = 0; j < B.Normals.Length; j++)
                {
                    //find the projected lengths of B-to-A and their half-widths/heights
                    BtoAlength = VectorHelper.project(BtoA, B.Normals[i]);
                    halfWidthA = VectorHelper.project(A.HalfWidth, B.Normals[i]);
                    halfHeightA = VectorHelper.project(A.HalfHeight, B.Normals[i]);
                    halfWidthB = VectorHelper.project(B.HalfWidth, B.Normals[i]);
                    halfHeightB = VectorHelper.project(B.HalfHeight, B.Normals[i]);

                    //determine the overlapping length of the projections for this axis
                    length = halfWidthA + halfHeightA + halfWidthB + halfHeightB - BtoAlength;

                    if (length < 0)
                        return false;

                }

            }

            return true;
        }



    }
}
