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

using Vaerydian.Utils;

using Vaerydian.Components;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Graphical;

namespace Vaerydian.Systems.Update
{
    class MapCollisionSystem : EntityProcessingSystem
    {
        private Entity m_Map;
        private Entity m_Camera;
        private ComponentMapper m_PositionMapper;
        private ComponentMapper m_VelocityMapper;
        private ComponentMapper m_GameMapMapper;
        private ComponentMapper m_ViewPortMapper;
        private ComponentMapper m_HeadingMapper;
        private ComponentMapper m_MapCollidableMapper;
        private int m_TileSize = 32;

        private Vector2 m_Center;

        public MapCollisionSystem() { }

        public override void initialize()
        {
            m_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            m_VelocityMapper = new ComponentMapper(new Velocity(), e_ECSInstance);
            m_GameMapMapper = new ComponentMapper(new GameMap(), e_ECSInstance);
            m_ViewPortMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
            m_HeadingMapper = new ComponentMapper(new Heading(), e_ECSInstance);
            m_MapCollidableMapper = new ComponentMapper(new MapCollidable(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            m_Map = e_ECSInstance.TagManager.getEntityByTag("MAP");
            m_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
        }

        protected override void cleanUp(Bag<Entity> entities) { }

        protected override void process(Entity entity)
        {
            //get colidable entity's position
            Position pos = (Position)m_PositionMapper.get(entity);
            ViewPort camView = (ViewPort)m_ViewPortMapper.get(m_Camera);
            MapCollidable mapCollide = (MapCollidable)m_MapCollidableMapper.get(entity);
            
            //reset collision detection
            mapCollide.Collided = false;

            //get the coliding entities position
            Vector2 colPos = pos.Pos;
            
            //create a polygon based at this entity's current position, using standard tileSize for height and width
            Polygon colPoly = createSquarePolygon(colPos, m_TileSize, m_TileSize);

            //set the center of the screen (used later for creating the the tile's polygon in "createPolysFromTiles")
            m_Center = camView.getDimensions() / 2;
            
            //find colliding tile polygons by finding a list of tiles that colide with the coliding entity's polygon
            List<Polygon> polys = createPolysFromTiles(findColidingTiles(colPoly));

            //if no poly's were found return now
            if (polys == null)
                return;

            //collided
            mapCollide.Collided = true;

            Vector2 response;

            //for each tile polygon that we have collided with, find the heading correction vector
            //and update the position with it. The sum of the corrections, will resolve our position
            //to all collisions
            for (int i = 0; i < polys.Count; i++)
            {
                response = correctHeading(colPoly, polys[i]);
                mapCollide.ResponseVector += response;
                colPos += response;
            }

            //set the corrected position
            pos.Pos = colPos;
        }

        /// <summary>
        /// finds tiles that may collide with the given polygon
        /// </summary>
        /// <param name="A">Polygon to check</param>
        /// <returns>a list of colliding tile index vectors</returns>
        private List<Vector2> findColidingTiles(Polygon A)
        {
            //setup some temp variables
            int x, y;
            List<Vector2> tiles = new List<Vector2>();

            //get the tile map 
            GameMap map = (GameMap) m_GameMapMapper.get(m_Map);
            Terrain terrain;
            
            //check each test point on A for a coresponding tile
            for (int i = 0; i < A.TestPoints.Length; i++)
            {
                //map the test point back-to the tile matrix
                //x = (int)((A.TestPoints[i].X + m_Center.X) / m_TileSize);
                //y = (int)((A.TestPoints[i].Y + m_Center.Y) / m_TileSize);
                x = (int)((A.TestPoints[i].X) / m_TileSize);
                y = (int)((A.TestPoints[i].Y) / m_TileSize);

                //get a potential colliding tile
                terrain = map.getTerrain(x, y);

                //ensure we have something usefull
                if (terrain == null)
                    continue;

                //if tile isnt blocking, then there is no issue, try getting another
                if (!terrain.IsBlocking)
                    continue;

                //make sure we didnt get this one already
                if (!tiles.Contains(new Vector2(x, y)))
                    tiles.Add(new Vector2(x, y));
            }
            //return our list (may or may not contain tile index vectors)
            return tiles;
        }

        /// <summary>
        /// creates polygons form a list of tile index vectors
        /// </summary>
        /// <param name="tiles">the list of tile index vectors</param>
        /// <returns>a list of polygons representing the tiles</returns>
        private List<Polygon> createPolysFromTiles(List<Vector2> tiles)
        {
            //did we actually get any index vectors? if not, return nothing
            if (tiles.Count == 0)
                return null;

            //create temp variables
            List<Polygon> polys = new List<Polygon>();
            Vector2 temp;

            //for each tile, re-constitute its game-space location and then generate a polygon representing it
            for (int i = 0; i < tiles.Count; i++)
            {
                temp = new Vector2((tiles[i].X * m_TileSize), tiles[i].Y * m_TileSize);// -m_Center;
                polys.Add(createSquarePolygon(temp,m_TileSize,m_TileSize));
            }

            //return polygon list
            return polys;
        }

        /// <summary>
        /// corrects heading based on two colliding polygons (A against B)
        /// </summary>
        /// <param name="A">polygon colliding with B</param>
        /// <param name="B">polygon being collided with A</param>
        /// <returns>the minimal distance correction</returns>
        private Vector2 correctHeading(Polygon A, Polygon B)
        {
            //setup temporary variables
            Vector2 newHeading = Vector2.Zero;
            float min = float.PositiveInfinity;
            Vector2 minAxis = Vector2.Zero;
            float BtoAlength, halfWidthA, halfHeightA, halfWidthB, halfHeightB, length;

            //calculate the vector from B-to-A
            Vector2 BtoA = Vector2.Subtract(A.Center, B.Center);

            //for each edge normal of B, find the minimal distance
            //NOTE: since we already know A is colliding with B, we just need to test
            //against B's normals
            for (int i = 0; i < B.Normals.Length; i++)
            {
                //find the projected lengths of B-to-A and their half-widths/heights
                BtoAlength = project(BtoA, B.Normals[i]);
                halfWidthA = project(A.HalfWidth, B.Normals[i]);
                halfHeightA = project(A.HalfHeight, B.Normals[i]);
                halfWidthB = project(B.HalfWidth, B.Normals[i]);
                halfHeightB = project(B.HalfHeight, B.Normals[i]);

                //determine the overlapping length of the projections for this axis
                length = halfWidthA + halfHeightA + halfWidthB + halfHeightB - BtoAlength;

                //check to see if this is the minimal length
                if (length < min)
                {
                    //it is the min lenght, so update and set the axis as minAxis
                    min = length;
                    minAxis = B.Normals[i];
                }
            }

            //double check dot-product of B-To-A against minAxis, if they are converging, we need to
            //negate the axis to get the correct heading
            if (Vector2.Dot(BtoA, minAxis) < 0)
                minAxis *= -1;

            //define the new heading and return it
            newHeading = minAxis * min;
            return newHeading;
        }

        /// <summary>
        /// project vector a onto vector b
        /// </summary>
        /// <param name="a">vector to project onto b</param>
        /// <param name="b">axis of projection</param>
        /// <returns>projected vector</returns>
        private float project(Vector2 a, Vector2 b)
        {
            return Math.Abs(Vector2.Dot(a, b));
        }

        /// <summary>
        /// basic polygon struct
        /// </summary>
        public struct Polygon
        {
            public Vector2[] Normals;
            public Vector2[] Vertices;
            public Vector2[] TestPoints;
            public Vector2 Center;
            public Vector2 HalfWidth;
            public Vector2 HalfHeight;
        }

        /// <summary>
        /// creates a polygon based off a tile position and a width-height. defined CCW
        /// </summary>
        /// <param name="position">Upper Left position of square polygon</param>
        /// <param name="height">height of polygon</param>
        /// <param name="width">width of polygon</param>
        /// <returns></returns>
        public Polygon createSquarePolygon(Vector2 position, float height, float width)
        {
            Polygon poly = new Polygon();

            //define center
            poly.Center = new Vector2(position.X + (width / 2), position.Y + (height / 2));
            
            //setup arrays
            poly.Normals = new Vector2[4];
            poly.Vertices = new Vector2[4];
            poly.TestPoints = new Vector2[4];

            //calculate the half-width/height
            poly.HalfWidth = new Vector2(width / 2,0);
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
            poly.TestPoints[2] = new Vector2(position.X + width/2, position.Y + height);
            
            //right
            poly.TestPoints[3] = new Vector2(position.X + width, position.Y + height/2);
             
            //setup edge normals - note, these MUST be normalized or your correcting vector will be too powerful
            //left normal
            poly.Normals[0] = getRightNormal(Vector2.Subtract(poly.Vertices[1], poly.Vertices[0]));
            poly.Normals[0].Normalize();
            //bottom normal
            poly.Normals[1] = getRightNormal(Vector2.Subtract(poly.Vertices[2], poly.Vertices[1]));
            poly.Normals[1].Normalize();
            //right normal
            poly.Normals[2] = getRightNormal(Vector2.Subtract(poly.Vertices[2], poly.Vertices[3]));
            poly.Normals[2].Normalize();
            //top normal
            poly.Normals[3] = getRightNormal(Vector2.Subtract(poly.Vertices[0], poly.Vertices[3]));
            poly.Normals[3].Normalize();

            return poly;
        }

        /// <summary>
        /// get right-hand normal of a given vector
        /// </summary>
        /// <param name="vector">vector to find normal for</param>
        /// <returns>right-hand normal</returns>
        private Vector2 getRightNormal(Vector2 vector)
        {
            Vector2 returnVec;
            returnVec.X = -vector.Y;
            returnVec.Y = vector.X;
            return returnVec;
        }

        /// <summary>
        /// get left-hand normal of a given vector
        /// </summary>
        /// <param name="vector">vector to find normal for</param>
        /// <returns>left-hand normal</returns>
        private Vector2 getLeftNormal(Vector2 vector)
        {
            Vector2 returnVec;
            returnVec.X = vector.Y;
            returnVec.Y = -vector.X;
            return returnVec;
        }
    }
}
