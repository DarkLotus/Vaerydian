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
using Microsoft.Xna.Framework.Graphics;


namespace Vaerydian.Components.Graphical
{
    public class GeometryMap : IComponent
    {
        private static int d_TypeID;
        private int d_EntityID;

        public GeometryMap() { }

        public int getEntityId()
        {
            return d_EntityID;
        }

        public int getTypeId()
        {
            return d_TypeID;
        }

        public void setEntityId(int entityId)
        {
            d_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            d_TypeID = typeId;
        }

        private RenderTarget2D d_ColorMap;

        public RenderTarget2D ColorMap
        {
            get { return d_ColorMap; }
            set { d_ColorMap = value; }
        }

        private RenderTarget2D d_DepthMap;

        public RenderTarget2D DepthMap
        {
            get { return d_DepthMap; }
            set { d_DepthMap = value; }
        }

        private RenderTarget2D d_NormalMap;

        public RenderTarget2D NormalMap
        {
            get { return d_NormalMap; }
            set { d_NormalMap = value; }
        }

        private RenderTarget2D d_ShadingMap;

        public RenderTarget2D ShadingMap
        {
            get { return d_ShadingMap; }
            set { d_ShadingMap = value; }
        }

        private Texture2D d_ColorMapTexture;

        public Texture2D ColorMapTexture
        {
            get { return d_ColorMapTexture; }
            set { d_ColorMapTexture = value; }
        }
        private Texture2D d_DepthMapTexture;

        public Texture2D DepthMapTexture
        {
            get { return d_DepthMapTexture; }
            set { d_DepthMapTexture = value; }
        }
        private Texture2D d_NormalMapTexture;

        public Texture2D NormalMapTexture
        {
            get { return d_NormalMapTexture; }
            set { d_NormalMapTexture = value; }
        }
        private Texture2D d_ShadowMapTexture;

        public Texture2D ShadowMapTexture
        {
            get { return d_ShadowMapTexture; }
            set { d_ShadowMapTexture = value; }
        }

        private Vector4 d_AmbientColor;

        public Vector4 AmbientColor
        {
            get { return d_AmbientColor; }
            set { d_AmbientColor = value; }
        }
    }
}
