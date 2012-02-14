using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ECSFramework;
using ECSFramework.Utils;

//using WorldGeneration.Cave;
using WorldGeneration.Utils;

using Vaerydian.Components;
using Vaerydian.Components.Debug;

namespace Vaerydian.Systems
{
    class MapNormalSystem : EntityProcessingSystem
    {
        private Dictionary<short, Texture2D> m_NormalDict;
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

            m_NormalDict = new Dictionary<short, Texture2D>();
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
            m_NormalDict.Add(TerrainType.CAVE_FLOOR, m_Container.ContentManager.Load<Texture2D>("terrain\\normals\\mountains_normal2"));
            m_NormalDict.Add(TerrainType.CAVE_WALL, m_Container.ContentManager.Load<Texture2D>("terrain\\normals\\cascade_normal2"));
            m_TileSize = m_NormalDict[TerrainType.CAVE_WALL].Width;
        }

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

                    m_SpriteBatch.Draw(m_NormalDict[c_CaveTerrain.TerrainType], pos, null, Color.White, 0f, origin, new Vector2(1), SpriteEffects.None, 0f);
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
