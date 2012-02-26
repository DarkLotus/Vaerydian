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
    class MapSystem : EntityProcessingSystem
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

        public MapSystem(GameContainer container)
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

            m_RectDict.Add(TerrainType.CAVE_FLOOR, new Rectangle(19 * 32, 10 * 32, 32, 32));
            m_RectDict.Add(TerrainType.CAVE_WALL, new Rectangle(18 * 32, 13 * 32, 32, 32));

            m_Texture = m_Container.ContentManager.Load<Texture2D>("terrain\\various");

            m_TileSize = m_RectDict[TerrainType.CAVE_WALL].Width;
        }

        protected override void process(Entity entity)
        {
            //get map and viewport
            GameMap map = (GameMap) m_CaveMapMapper.get(entity);
            m_ViewPort = (ViewPort) m_ViewportMapper.get(m_Camera);
            GeometryMap geometry = (GeometryMap)m_GeometryMapper.get(m_Geometry);

            //update for current viewport location/dimensions
            
            //grab key viewport info
            Vector2 origin = m_ViewPort.getOrigin();
            Vector2 dimensions = m_ViewPort.getDimensions();// / 2;
            Vector2 pos;

            //updateView(origin, center, m_ViewPort.getDimensions());
            updateView(map.Map,origin,dimensions);

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
                    pos = new Vector2(x*m_TileSize,y*m_TileSize);

                    m_SpriteBatch.Draw(m_Texture, pos-origin, m_RectDict[c_CaveTerrain.TerrainType], Color.White, 0f, new Vector2(0), new Vector2(1), SpriteEffects.None, 0f);
                }
            }

            
            
            /*
            try
            {
                MapDebug debug = (MapDebug)m_MapDebugMapper.get(m_MapDebug);


                if (debug.OpenSet != null)
                {
                    for (int i = 0; i < debug.OpenSet.Count; i++)
                    {
                        if (debug.OpenSet[i] == null)
                            continue;
                        pos = debug.OpenSet[i].Position * m_TileSize;

                        m_SpriteBatch.Draw(m_TextureDict[TerrainType.CAVE_FLOOR], pos, null, Color.Orange, 0f, origin, new Vector2(1), SpriteEffects.None, 0f);
                    }
                }

                if (debug.Blocking != null)
                {
                    for (int i = 0; i < debug.Blocking.Count; i++)
                    {
                        if (debug.Blocking[i] == null)
                            continue;
                        pos = debug.Blocking[i].Position * m_TileSize;

                        m_SpriteBatch.Draw(m_TextureDict[TerrainType.CAVE_WALL], pos, null, Color.Red, 0f, origin, new Vector2(1), SpriteEffects.None, 0f);
                    }
                }

                if (debug.ClosedSet != null)
                {
                    for (int i = 0; i < debug.ClosedSet.Count; i++)
                    {
                        if (debug.ClosedSet[i] == null)
                            continue;
                        pos = debug.ClosedSet[i].Position * m_TileSize;

                        m_SpriteBatch.Draw(m_TextureDict[TerrainType.CAVE_FLOOR], pos, null, Color.Yellow, 0f, origin, new Vector2(1), SpriteEffects.None, 0f);
                    }
                }

                if (debug.Path != null)
                {
                    for (int i = 0; i < debug.Path.Count; i++)
                    {
                        if (debug.Path[i] == null)
                            continue;
                        pos = debug.Path[i].Position * m_TileSize;

                        m_SpriteBatch.Draw(m_TextureDict[TerrainType.CAVE_FLOOR], pos, null, Color.YellowGreen, 0f, origin, new Vector2(1), SpriteEffects.None, 0f);
                    }
                }

            }
            catch (Exception e)
            {
                
            }*/

            m_SpriteBatch.End();
        }

        /// <summary>
        /// updates the tile indexes based on current viewport for the draw loop
        /// </summary>
        private void updateView(Map map, Vector2 origin, Vector2 dimensions)
        {

            m_xStart = (int)origin.X / m_TileSize;
            if (m_xStart <= 0)
                m_xStart = 0;

            m_xFinish = (int)(origin.X + dimensions.X) / m_TileSize;
            if (m_xFinish >= map.XSize-1)//m_ViewPort.getDimensions().X - 1)
                m_xFinish = map.XSize-1;// (int)m_ViewPort.getDimensions().X - 1;

            m_yStart = (int)origin.Y / m_TileSize;
            if (m_yStart <= 0)
                m_yStart = 0;

            m_yFinish = (int)(origin.Y + dimensions.Y) / m_TileSize;
            if (m_yFinish >= map.YSize-1)//m_ViewPort.getDimensions().X - 1)
                m_yFinish = map.YSize-1;// (int)m_ViewPort.getDimensions().X - 1;
        }

        /// <summary>
        /// updates the tile indexes based on current viewport for the draw loop
        /// </summary>
        private void updateView(Vector2 origin, Vector2 center, Vector2 dimensions)
        {
            m_xStart = (int)(origin.X - center.X) / m_TileSize;
            if (m_xStart <= 0)
                m_xStart = 0;

            m_xFinish = (int)(origin.X - center.X + dimensions.X) / m_TileSize;
            if (m_xFinish >= dimensions.X - 1)
                m_xFinish = (int)dimensions.X - 1;

            m_yStart = (int)(origin.Y - center.Y) / m_TileSize;
            if (m_yStart <= 0)
                m_yStart = 0;

            m_yFinish = (int)(origin.Y - center.Y + dimensions.Y) / m_TileSize;
            if (m_yFinish >= dimensions.X - 1)
                m_yFinish = (int)dimensions.X - 1;
        }

    }
}
