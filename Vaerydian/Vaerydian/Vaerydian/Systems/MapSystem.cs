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
        private Dictionary<short, Texture2D> m_TextureDict;
        private ComponentMapper m_CaveMapMapper;
        private ComponentMapper m_ViewportMapper;
        private ComponentMapper m_PositionMapper;
        private ComponentMapper m_MapDebugMapper;
        private GameContainer m_Container;
        private Entity m_Camera;
        private Entity m_Player;
        private Entity m_MapDebug;
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
            m_TextureDict = new Dictionary<short, Texture2D>();
            m_CaveMapMapper = new ComponentMapper(new GameMap(), e_ECSInstance);
            m_ViewportMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
            m_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            m_MapDebugMapper = new ComponentMapper(new MapDebug(), e_ECSInstance);
        }
        
        protected override void preLoadContent(Bag<Entity> entities)
        {
            m_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
            m_Player = e_ECSInstance.TagManager.getEntityByTag("PLAYER");
            m_MapDebug = e_ECSInstance.TagManager.getEntityByTag("MAP_DEBUG");
            m_TextureDict.Add(TerrainType.CAVE_FLOOR, m_Container.ContentManager.Load<Texture2D>("terrain\\mountains"));
            m_TextureDict.Add(TerrainType.CAVE_WALL, m_Container.ContentManager.Load<Texture2D>("terrain\\cascade"));
            m_TileSize = m_TextureDict[TerrainType.CAVE_WALL].Width;
        }

        protected override void process(Entity entity)
        {
            //get map and viewport
            GameMap map = (GameMap) m_CaveMapMapper.get(entity);
            m_ViewPort = (ViewPort) m_ViewportMapper.get(m_Camera);

            //update for current viewport location/dimensions
            updateView();
            
            //grab key viewport info
            Vector2 origin = m_ViewPort.getOrigin();
            Vector2 center = m_ViewPort.getDimensions() / 2;
            Vector2 pos;

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

                    //calculate data for debugging
                    Position playerPos = (Position)m_PositionMapper.get(m_Player);
                    int pX, pY;
                    pX = (int)((playerPos.getPosition().X + center.X) / m_TileSize);
                    pY = (int)((playerPos.getPosition().Y + center.Y) / m_TileSize);
                    Vector2 playerVec = new Vector2(pX, pY);// +playerPos.getOffset();
                    Vector2 tileVec = new Vector2(x, y);

                    //draw tile
                    /*if (c_CaveTerrain.IsBlocking)
                    {
                        m_SpriteBatch.Draw(m_TextureDict[c_CaveTerrain.TerrainType], pos, null, Color.White, 0f, origin, new Vector2(1), SpriteEffects.None, 0f);
                    }
                    else
                    {
                        m_SpriteBatch.Draw(m_TextureDict[c_CaveTerrain.TerrainType], pos, null, Color.White, 0f, origin, new Vector2(1), SpriteEffects.None, 0f);
                    }*/
                    m_SpriteBatch.Draw(m_TextureDict[c_CaveTerrain.TerrainType], pos, null, Color.White, 0f, origin, new Vector2(1), SpriteEffects.None, 0f);
                }
            }

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
                
            }
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
