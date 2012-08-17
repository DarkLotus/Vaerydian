using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ECSFramework;

using Vaerydian;
using Vaerydian.Components;
using Vaerydian.Utils;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Graphical;

namespace Vaerydian.Systems.Draw
{
    class QuadTreeDebugRenderSystem : EntityProcessingSystem
    {
        private GameContainer q_Contaner;
        private SpriteBatch q_SpriteBatch;

        private ComponentMapper q_SpatialMapper;
        private ComponentMapper q_PositionMapper;
        private ComponentMapper q_ViewPortMapper;

        private Entity q_Player;
        private Entity q_Camera;
        private Entity q_Spatial;

        private Texture2D q_Texture;
        
        public QuadTreeDebugRenderSystem(GameContainer container) 
        {
            q_Contaner = container;
            q_SpriteBatch = container.SpriteBatch;
        }

        public override void initialize()
        {
            q_SpatialMapper = new ComponentMapper(new SpatialPartition(), e_ECSInstance);
            q_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            q_ViewPortMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            q_Player = e_ECSInstance.TagManager.getEntityByTag("PLAYER");
            q_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
            q_Spatial = e_ECSInstance.TagManager.getEntityByTag("SPATIAL");

            q_Texture = q_Contaner.ContentManager.Load<Texture2D>("export");
        }

        protected override void process(Entity entity)
        {
            Position position = (Position)q_PositionMapper.get(entity);
            ViewPort camera = (ViewPort)q_ViewPortMapper.get(q_Camera);
            SpatialPartition spatial = (SpatialPartition)q_SpatialMapper.get(q_Spatial);

            Vector2 pos = position.Pos + position.Offset;
            Vector2 origin = camera.getOrigin();
            QuadNode<Entity> node = spatial.QuadTree.locateNode(pos);

            int width = (int)(node.LRCorner.X - node.ULCorner.X);
            int height = (int)(node.LRCorner.Y - node.ULCorner.Y);

            Rectangle rec = new Rectangle((int)(node.ULCorner.X - origin.X), (int)(node.ULCorner.Y - origin.Y), width, height);

            q_SpriteBatch.Begin();

            q_SpriteBatch.Draw(q_Texture, rec, new Color(1f,0f,0f,0f));

            q_SpriteBatch.End();
        }

    }
}
