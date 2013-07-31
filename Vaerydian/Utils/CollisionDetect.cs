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
