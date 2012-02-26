using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;


namespace Vaerydian.Utils
{
    class Polygon
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

            //setup edge normals - note, these MUST be normalized or your correcting vector will be too powerful
            //left normal
            this.Normals[0] = VectorHelper.getRightNormal(Vector2.Subtract(this.Vertices[1], this.Vertices[0]));
            this.Normals[0].Normalize();
            //bottom normal
            this.Normals[1] = VectorHelper.getRightNormal(Vector2.Subtract(this.Vertices[2], this.Vertices[1]));
            this.Normals[1].Normalize();
            //right normal
            this.Normals[2] = VectorHelper.getRightNormal(Vector2.Subtract(this.Vertices[2], this.Vertices[3]));
            this.Normals[2].Normalize();
            //top normal
            this.Normals[3] = VectorHelper.getRightNormal(Vector2.Subtract(this.Vertices[0], this.Vertices[3]));
            this.Normals[3].Normalize();
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

            return poly;
        }

    }
}
