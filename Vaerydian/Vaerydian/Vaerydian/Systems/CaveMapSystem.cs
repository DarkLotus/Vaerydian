using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ECSFramework;
using ECSFramework.Utils;

using WorldGeneration.Cave;

using Vaerydian.Components;

namespace Vaerydian.Systems
{
    class CaveMapSystem : EntityProcessingSystem
    {
        private Dictionary<CaveType, Texture2D> c_TextureDict;
        private ComponentMapper c_CaveMapMapper;
        private ComponentMapper c_ViewportMapper;
        private ComponentMapper c_PositionMapper;
        private GameContainer c_Container;
        private Entity camera;
        private Entity player;
        private ViewPort viewport;

        private int c_xStart, c_yStart, c_xFinish, c_yFinish, c_TileSize;

        private CaveTerrain c_CaveTerrain;

        public CaveMapSystem(GameContainer container)
        {
            c_Container = container;
        }

        public override void initialize()
        {
            c_TextureDict = new Dictionary<CaveType, Texture2D>();
            c_CaveMapMapper = new ComponentMapper(new CaveMap(), e_ECSInstance);
            c_ViewportMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
            c_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
        }
        
        protected override void preLoadContent(Bag<Entity> entities)
        {
            camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
            player = e_ECSInstance.TagManager.getEntityByTag("PLAYER");
            c_TextureDict.Add(CaveType.Floor, c_Container.ContentManager.Load<Texture2D>("terrain\\mountains"));
            c_TextureDict.Add(CaveType.Wall, c_Container.ContentManager.Load<Texture2D>("terrain\\cascade"));
            c_TileSize = c_TextureDict[CaveType.Wall].Width;
        }

        protected override void process(Entity entity)
        {
            //get map and viewport
            CaveMap map = (CaveMap) c_CaveMapMapper.get(entity);
            viewport = (ViewPort) c_ViewportMapper.get(camera);

            //update for current viewport location/dimensions
            updateView();
            
            //grab key viewport info
            Vector2 origin = viewport.getOrigin();
            Vector2 center = viewport.getDimensions() / 2;
            Vector2 pos;

            //iterate through current viewable tiles
            for (int x = c_xStart; x <= c_xFinish; x++)
            {
                for (int y = c_yStart; y <= c_yFinish; y++)
                {
                    //grab current tile terrain
                    c_CaveTerrain = map.Cave.getTerrain(x, y);

                    //ensure its useable
                    if (c_CaveTerrain == null)
                        continue;

                    //calculate position to place tile
                    pos = new Vector2(x*c_TileSize,y*c_TileSize);

                    //calculate data for debugging
                    Position playerPos = (Position)c_PositionMapper.get(player);
                    int pX, pY;
                    pX = (int)((playerPos.getPosition().X + center.X) / c_TileSize);
                    pY = (int)((playerPos.getPosition().Y + center.Y) / c_TileSize);
                    Vector2 playerVec = new Vector2(pX, pY);// +playerPos.getOffset();
                    Vector2 tileVec = new Vector2(x, y);

                    //draw tile
                    if (playerVec == tileVec)//if(c_CaveTerrain.IsBlocking)
                    {
                        c_Container.SpriteBatch.Draw(c_TextureDict[c_CaveTerrain.BaseCaveType], pos, null, Color.Green, 0f, origin, new Vector2(1), SpriteEffects.None, 0f);
                    }
                    else
                    {
                        c_Container.SpriteBatch.Draw(c_TextureDict[c_CaveTerrain.BaseCaveType], pos, null, Color.White, 0f, origin, new Vector2(1), SpriteEffects.None, 0f);
                    }
                }
            }
            

        }

        /// <summary>
        /// updates the tile indexes based on current viewport for the draw loop
        /// </summary>
        private void updateView()
        {
            c_xStart = (int)viewport.getOrigin().X / c_TileSize;
            if (c_xStart <= 0)
                c_xStart = 0;

            c_xFinish = (int)(viewport.getOrigin().X + viewport.getDimensions().X) / c_TileSize;
            if (c_xFinish >= viewport.getDimensions().X - 1)
                c_xFinish = (int) viewport.getDimensions().X - 1;

            c_yStart = (int)viewport.getOrigin().Y / c_TileSize;
            if (c_yStart <= 0)
                c_yStart = 0;

            c_yFinish = (int)(viewport.getOrigin().Y + viewport.getDimensions().Y) / c_TileSize;
            if (c_yFinish >= viewport.getDimensions().X - 1)
                c_yFinish = (int)viewport.getDimensions().X - 1;
        }

    }
}
