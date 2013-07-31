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

using Vaerydian;
using Vaerydian.Components;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Graphical;

namespace Vaerydian.Systems.Draw
{
    class ShadingSystem : EntityProcessingSystem
    {
        private GameContainer s_Container;
        private Effect s_ShadingEffect;
        //private SpriteBatch s_SpriteBatch;
        private GraphicsDevice s_GraphicsDevice;
        private ComponentMapper s_GeometryMapper;
        private ComponentMapper s_LightMapper;
        private ComponentMapper s_PositionMapper;
        private ComponentMapper s_ViewPortMapper;
        private Entity s_GeometryMap;
        private Entity s_Camera;
        private GeometryMap s_Geometry;
        private BlendState s_BlendState;

        private VertexPositionColorTexture[] s_Vertices;
        private VertexBuffer s_VertexBuffer;
        
        public ShadingSystem(GameContainer container)
        {
            s_Container = container;
            s_GraphicsDevice = s_Container.GraphicsDevice;
        }

        public override void initialize()
        {
            //s_ShadingEffect.CurrentTechnique = s_ShadingEffect.Techniques["PointLight"];
            s_GeometryMapper = new ComponentMapper(new GeometryMap(), e_ECSInstance);
            s_LightMapper = new ComponentMapper(new Light(), e_ECSInstance);
            s_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            s_ViewPortMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);

            //setup vertex buffer
            s_Vertices = new VertexPositionColorTexture[4];
            s_Vertices[0] = new VertexPositionColorTexture(new Vector3(-1, 1, 0), Color.White, new Vector2(0, 0));
            s_Vertices[1] = new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.White, new Vector2(1, 0));
            s_Vertices[2] = new VertexPositionColorTexture(new Vector3(-1, -1, 0), Color.White, new Vector2(0, 1));
            s_Vertices[3] = new VertexPositionColorTexture(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1));
            s_VertexBuffer = new VertexBuffer(s_GraphicsDevice, typeof(VertexPositionColorTexture), s_Vertices.Length, BufferUsage.None);
            s_VertexBuffer.SetData(s_Vertices);

            //setup blending
            s_BlendState = new BlendState();
            s_BlendState.ColorBlendFunction = BlendFunction.Add;
            s_BlendState.ColorSourceBlend = Blend.One;
            s_BlendState.ColorDestinationBlend = Blend.One;
            s_BlendState.AlphaBlendFunction = BlendFunction.Add;
            s_BlendState.AlphaSourceBlend = Blend.SourceAlpha;
            s_BlendState.AlphaDestinationBlend = Blend.One;
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            s_ShadingEffect = s_Container.ContentManager.Load<Effect>("effects\\Shading");
            s_GeometryMap = e_ECSInstance.TagManager.getEntityByTag("GEOMETRY");
            s_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
        }

        protected override void cleanUp(Bag<Entity> entities) { }

        protected override void begin()
        {
            s_Geometry = (GeometryMap) s_GeometryMapper.get(s_GeometryMap);
        }

        protected override void process(Entity entity)
        {
            Light light = (Light)s_LightMapper.get(entity);

            if (!light.IsEnabled)
                return;
  
            //setup locals
            ViewPort viewport = (ViewPort)s_ViewPortMapper.get(s_Camera);
            Position lightPos = (Position)s_PositionMapper.get(entity);
            Vector2 position = lightPos.Pos + lightPos.Offset;
            Vector2 origin = viewport.getOrigin();
            Vector2 center = viewport.getDimensions() / 2;
            int radius = light.LightRadius;

            //calculate the wide-view rectangle
            Rectangle view = new Rectangle((int)(origin.X - radius),
                                           (int)(origin.Y - radius), 
                                           (int)(viewport.getDimensions().X + 2*radius), 
                                           (int)(viewport.getDimensions().Y + 2*radius));
            /*
            Rectangle view = new Rectangle((int)(origin.X - center.X - radius),
                                           (int)(origin.Y - center.Y - radius), 
                                           (int)(viewport.getDimensions().X + 2*radius), 
                                           (int)(viewport.getDimensions().Y + 2*radius));
            */

            //if light is not on the screen, dont process it
            if (!view.Contains((int)(position.X), (int)(position.Y)))
                return;

            //setup some parameter variables
            Vector3 center3 = new Vector3(center, 0);
            Vector3 origin3 = new Vector3(origin, 0);
            Vector3 position3 = new Vector3(position, light.Position.Z);

            //set vertex buffer
            s_GraphicsDevice.SetVertexBuffer(s_VertexBuffer);

            //get light source parameters
            s_ShadingEffect.Parameters["lightStrength"].SetValue(light.ActualPower);
            s_ShadingEffect.Parameters["lightPosition"].SetValue(position3);
            s_ShadingEffect.Parameters["viewCenter"].SetValue(new Vector3(0));//center3);
            s_ShadingEffect.Parameters["viewOrigin"].SetValue(origin3);
            s_ShadingEffect.Parameters["lightColor"].SetValue(light.Color);
            s_ShadingEffect.Parameters["lightRadius"].SetValue(light.LightRadius);
            s_ShadingEffect.Parameters["specularStrength"].SetValue(.5f);
            s_ShadingEffect.Parameters["specularColor"].SetValue(light.Color);
            s_ShadingEffect.Parameters["screenWidth"].SetValue(s_GraphicsDevice.Viewport.Width);
            s_ShadingEffect.Parameters["screenHeight"].SetValue(s_GraphicsDevice.Viewport.Height);
            s_ShadingEffect.Parameters["NormalMap"].SetValue(s_Geometry.NormalMap);
            s_ShadingEffect.Parameters["ColorMap"].SetValue(s_Geometry.ColorMap);
            s_ShadingEffect.Parameters["DepthMap"].SetValue(s_Geometry.DepthMap);
            
            s_ShadingEffect.CurrentTechnique = s_ShadingEffect.Techniques["PointLight"];
            s_ShadingEffect.CurrentTechnique.Passes[0].Apply();

            s_GraphicsDevice.BlendState = s_BlendState;

            s_GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, s_Vertices, 0, 2);

            

        }


    }
}
