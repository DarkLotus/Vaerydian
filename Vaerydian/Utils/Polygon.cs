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
    public class Polygon
    {
        private Vector2[] p_Normals;

        public Vector2[] Normals
        {
            get { return p_Normals; }
            set { p_Normals = value; }
        }
        private Vector2[] p_Vertices;

        public Vector2[] Vertices
        {
            get { return p_Vertices; }
            set { p_Vertices = value; }
        }
        private Vector2[] p_TestPoints;

        public Vector2[] TestPoints
        {
            get { return p_TestPoints; }
            set { p_TestPoints = value; }
        }
        private Vector2 p_Center;

        public Vector2 Center
        {
            get { return p_Center; }
            set { p_Center = value; }
        }
        private Vector2 p_HalfWidth;

        public Vector2 HalfWidth
        {
            get { return p_HalfWidth; }
            set { p_HalfWidth = value; }
        }
        private Vector2 p_HalfHeight;

        public Vector2 HalfHeight
        {
            get { return p_HalfHeight; }
            set { p_HalfHeight = value; }
        }

        public Polygon() { }

        public Polygon(Vector2 position, float height, float width)
        {
            this.createPoly(position, height, width);
        }

        private void createPoly(Vector2 position, float height, float width)
        {

            //define center
            this.Center = new Vector2(position.X + (width / 2), position.Y + (height / 2));

            //setup arrays
            this.Normals = new Vector2[4];
            this.Vertices = new Vector2[4];
            this.TestPoints = new Vector2[4];

            //calculate the half-width/height
            this.HalfWidth = new Vector2(width / 2, 0);
            this.HalfHeight = new Vector2(0, height / 2);

            //set vertices
            //UL point
            this.Vertices[0] = position;

            //LL point
            this.Vertices[1] = new Vector2(position.X, position.Y + height);

            //LR point
            this.Vertices[2] = new Vector2(position.X + width, position.Y + height);

            //UR point
            this.Vertices[3] = new Vector2(position.X + width, position.Y);

            //set testing points
            //Top
            this.TestPoints[0] = new Vector2(position.X + width / 2, position.Y);

            //left
            this.TestPoints[1] = new Vector2(position.X, position.Y + height / 2);

            //bottom
            this.TestPoints[2] = new Vector2(position.X + width / 2, position.Y + height);

            //right
            this.TestPoints[3] = new Vector2(position.X + width, position.Y + height / 2);

            this.Normals = createNormals(this);
        }


        /// <summary>
        /// creates a polygon based off a tile position and a width-height. defined CCW
        /// </summary>
        /// <param name="position">Upper Left position of square polygon</param>
        /// <param name="height">height of polygon</param>
        /// <param name="width">width of polygon</param>
        /// <returns></returns>
        public static Polygon createSquarePolygon(Vector2 position, float height, float width)
        {
            Polygon poly = new Polygon();

            //define center
            poly.Center = new Vector2(position.X + (width / 2), position.Y + (height / 2));

            //setup arrays
            poly.Normals = new Vector2[4];
            poly.Vertices = new Vector2[4];
            poly.TestPoints = new Vector2[4];

            //calculate the half-width/height
            poly.HalfWidth = new Vector2(width / 2, 0);
            poly.HalfHeight = new Vector2(0, height / 2);

            //set vertices
            //UL point
            poly.Vertices[0] = position;

            //LL point
            poly.Vertices[1] = new Vector2(position.X, position.Y + height);

            //LR point
            poly.Vertices[2] = new Vector2(position.X + width, position.Y + height);

            //UR point
            poly.Vertices[3] = new Vector2(position.X + width, position.Y);

            //set testing points
            //Top
            poly.TestPoints[0] = new Vector2(position.X + width / 2, position.Y);

            //left
            poly.TestPoints[1] = new Vector2(position.X, position.Y + height / 2);

            //bottom
            poly.TestPoints[2] = new Vector2(position.X + width / 2, position.Y + height);

            //right
            poly.TestPoints[3] = new Vector2(position.X + width, position.Y + height / 2);

            poly.Normals = createNormals(poly);

            return poly;
        }

        private static Vector2[] createNormals(Polygon poly)
        {
            //setup edge normals - note, these MUST be normalized or your correcting vector will be too powerful
            //left normal
            poly.Normals[0] = VectorHelper.getRightNormal(Vector2.Subtract(poly.Vertices[1], poly.Vertices[0]));
            poly.Normals[0].Normalize();
            //bottom normal
            poly.Normals[1] = VectorHelper.getRightNormal(Vector2.Subtract(poly.Vertices[2], poly.Vertices[1]));
            poly.Normals[1].Normalize();
            //right normal
            poly.Normals[2] = VectorHelper.getRightNormal(Vector2.Subtract(poly.Vertices[2], poly.Vertices[3]));
            poly.Normals[2].Normalize();
            //top normal
            poly.Normals[3] = VectorHelper.getRightNormal(Vector2.Subtract(poly.Vertices[0], poly.Vertices[3]));
            poly.Normals[3].Normalize();

            return poly.Normals;
        }

        /// <summary>
        /// rotates a polygon about an origin by a given angle
        /// </summary>
        /// <param name="polygon">polygon to be rotated</param>
        /// <param name="origin">origin of rotation</param>
        /// <param name="angle">angle of rotation</param>
        /// <returns></returns>
        public static Polygon rotatePolygon(Polygon polygon, Vector2 origin, float angle)
        {
            Polygon rotPoly = new Polygon();
            rotPoly.Vertices = new Vector2[polygon.Vertices.Length];
            rotPoly.Normals = new Vector2[polygon.Normals.Length];
            rotPoly.TestPoints = new Vector2[polygon.TestPoints.Length];
            rotPoly.HalfHeight = polygon.HalfHeight;
            rotPoly.HalfWidth = polygon.HalfWidth;
            
            for(int i = 0; i < polygon.Vertices.Length;i++)
            {
                rotPoly.Vertices[i] = VectorHelper.rotateOffsetVectorRadians(polygon.Vertices[i], origin, angle);
                rotPoly.TestPoints[i] = VectorHelper.rotateOffsetVectorRadians(polygon.TestPoints[i], origin, angle);
            }

            rotPoly.Center = VectorHelper.rotateOffsetVectorRadians(polygon.Center, origin, angle);
            rotPoly.Normals = createNormals(rotPoly);

            return rotPoly;
        }

        public static bool isColliding(Polygon A, Polygon B)
        {
            //setup temp variables
            //float min = float.PositiveInfinity;
            //Vector2 minAxis = Vector2.Zero;
            //float BtoAlength, halfWidthA, halfHeightA, halfWidthB, halfHeightB, length;

            Vector2 BtoA = A.Center - B.Center;

            List<float> As = new List<float>();
            List<float> Bs = new List<float>();

            //for (int i = 0; i < A.Normals.Length; i++)
            //{
                for (int i = 0; i < B.Normals.Length; i++)
                {
                    //find the projected lengths of B-to-A and their half-widths/heights
                    //BtoAlength = VectorHelper.project(BtoA, B.Normals[i]);
                    //halfWidthA = VectorHelper.project(A.HalfWidth, B.Normals[i]);
                    //halfHeightA = VectorHelper.project(A.HalfHeight, B.Normals[i]);
                    //halfWidthB = VectorHelper.project(B.HalfWidth, B.Normals[i]);
                    //halfHeightB = VectorHelper.project(B.HalfHeight, B.Normals[i]);
                    As.Add(VectorHelper.project(A.HalfWidth, B.Normals[i]));
                    As.Add(VectorHelper.project(A.HalfHeight, B.Normals[i]));
                    Bs.Add(VectorHelper.project(B.HalfWidth, B.Normals[i]));
                    Bs.Add(VectorHelper.project(B.HalfHeight, B.Normals[i]));

                    //determine the overlapping length of the projections for this axis
                    //length = halfWidthA + halfHeightA + halfWidthB + halfHeightB - BtoAlength;

                    //if (length >= 0)
                        //continue;
                    //else
                      //  return false;
                }
            //}

            float aMin = As.Min();
            float aMax = As.Max();
            float bMin = Bs.Min();
            float bMax = Bs.Max();

            if (bMin <= aMax && bMax >= aMax)
            {
                return true;
            }
            else if (aMin <= bMax && aMax >= bMax)
            {
                return true;
            }

            return false;
        }

    }
}
