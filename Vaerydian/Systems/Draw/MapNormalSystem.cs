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

using ECSFramework;


//using WorldGeneration.Cave;
using Vaerydian.Utils;

using Vaerydian.Components;
using Vaerydian.Components.Dbg;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Graphical;

namespace Vaerydian.Systems.Draw
{
    class MapNormalSystem : EntityProcessingSystem
    {
        private Dictionary<short, Rectangle> m_RectDict;
        private Texture2D m_Texture;
        private ComponentMapper m_CaveMapMapper;
        private ComponentMapper m_ViewportMapper;
        private ComponentMapper m_PositionMapper;
        private ComponentMapper m_MapDebugMapper;
        private ComponentMapper m_GeometryMapper;
        private GameContainer m_Container;
        private Entity m_Camera;
        private Entity m_Player;
        private Entity m_MapDebug;
        private Entity m_Geometry;
        private ViewPort m_ViewPort;
        private SpriteBatch m_SpriteBatch;

        private int m_xStart, m_yStart, m_xFinish, m_yFinish, m_TileSize;

        private Terrain c_CaveTerrain;

        public MapNormalSystem(GameContainer container)
        {
            m_Container = container;
            m_SpriteBatch = m_Container.SpriteBatch;
        }

        public override void initialize()
        {

            m_RectDict = new Dictionary<short, Rectangle>();
            m_CaveMapMapper = new ComponentMapper(new GameMap(), e_ECSInstance);
            m_ViewportMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
            m_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            m_MapDebugMapper = new ComponentMapper(new MapDebug(), e_ECSInstance);
            m_GeometryMapper = new ComponentMapper(new GeometryMap(), e_ECSInstance);
        }
        
        protected override void preLoadContent(Bag<Entity> entities)
        {
            m_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
            m_Player = e_ECSInstance.TagManager.getEntityByTag("PLAYER");
            m_MapDebug = e_ECSInstance.TagManager.getEntityByTag("MAP_DEBUG");
            m_Geometry = e_ECSInstance.TagManager.getEntityByTag("GEOMETRY");

            m_RectDict.Add(TerrainType_Old.CAVE_FLOOR, new Rectangle(19 * 32, 10 * 32, 32, 32));
            m_RectDict.Add(TerrainType_Old.CAVE_WALL, new Rectangle(18 * 32, 13 * 32, 32, 32));

            m_Texture = m_Container.ContentManager.Load<Texture2D>("terrain\\various_normals");

            m_TileSize = m_RectDict[TerrainType_Old.CAVE_WALL].Width;
        }

        protected override void cleanUp(Bag<Entity> entities) { }

        protected override void process(Entity entity)
        {
            //get map and viewport
            GameMap map = (GameMap) m_CaveMapMapper.get(entity);
            m_ViewPort = (ViewPort) m_ViewportMapper.get(m_Camera);
            GeometryMap geometry = (GeometryMap)m_GeometryMapper.get(m_Geometry);

            //update for current viewport location/dimensions
            updateView();
 
            //grab key viewport info
            Vector2 origin = m_ViewPort.getOrigin();
            Vector2 center = m_ViewPort.getDimensions() / 2;
            Vector2 pos;

            m_SpriteBatch.Begin();

            //iterate through current viewable tiles
            for (int x = m_xStart; x <= m_xFinish; x++)
            {
                for (int y = m_yStart; y <= m_yFinish; y++)
                {
                    //grab current tile terrain
                    c_CaveTerrain = map.getTerrain(x, y);

                    //ensure its useable
                    if (c_CaveTerrain == null)
                        continue;

                    //calculate position to place tile
                    pos = new Vector2(x * m_TileSize, y * m_TileSize);

                    m_SpriteBatch.Draw(m_Texture, pos-origin, m_RectDict[c_CaveTerrain.TerrainType], Color.White, 0f, new Vector2(0), new Vector2(1), SpriteEffects.None, 0f);
                }
            }

            m_SpriteBatch.End();

        }

        /// <summary>
        /// updates the tile indexes based on current viewport for the draw loop
        /// </summary>
        private void updateView()
        {
            m_xStart = (int)m_ViewPort.getOrigin().X / m_TileSize;
            if (m_xStart <= 0)
                m_xStart = 0;

            m_xFinish = (int)(m_ViewPort.getOrigin().X + m_ViewPort.getDimensions().X) / m_TileSize;
            if (m_xFinish >= m_ViewPort.getDimensions().X - 1)
                m_xFinish = (int) m_ViewPort.getDimensions().X - 1;

            m_yStart = (int)m_ViewPort.getOrigin().Y / m_TileSize;
            if (m_yStart <= 0)
                m_yStart = 0;

            m_yFinish = (int)(m_ViewPort.getOrigin().Y + m_ViewPort.getDimensions().Y) / m_TileSize;
            if (m_yFinish >= m_ViewPort.getDimensions().X - 1)
                m_yFinish = (int)m_ViewPort.getDimensions().X - 1;
        }
    }
}
