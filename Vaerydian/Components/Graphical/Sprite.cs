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
using Vaerydian.Utils;


namespace Vaerydian.Components.Graphical
{
    public class Sprite : IComponent
    {

        private static int s_TypeID;
        private int s_EntityID;

        

        public Sprite() { }

        public Sprite(String textureName, String normalName,int height, int width, int xInd, int yInd)
        {
            s_TextureName = textureName;
            s_NormalName = normalName;
            s_Height = height;
            s_Width = width;
            s_Column = xInd;
            s_Row = yInd;
        }

        public int getEntityId()
        {
            return s_EntityID;
        }

        public int getTypeId()
        {
            return s_TypeID;
        }

        public String getTextureName()
        {
            return s_TextureName;
        }

        public void setEntityId(int entityId)
        {
            s_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            s_TypeID = typeId;
        }

        private String s_TextureName;

        public String TextureName
        {
            get { return s_TextureName; }
            set { s_TextureName = value; }
        }

        public void setTextureName(String textureName)
        {
            s_TextureName = textureName;
        }

        private String s_NormalName;

        public String NormalName
        {
            get { return s_NormalName; }
            set { s_NormalName = value; }
        }

        private int s_Height;

        public int Height
        {
            get { return s_Height; }
            set { s_Height = value; }
        }

        private int s_Width;

        public int Width
        {
            get { return s_Width; }
            set { s_Width = value; }
        }

        private int s_Column;

        public int Column
        {
            get { return s_Column; }
            set { s_Column = value; }
        }

        private int s_Row;

        public int Row
        {
            get { return s_Row; }
            set { s_Row = value; }
        }

        private Color s_Color = Color.White;

        public Color Color
        {
            get { return s_Color; }
            set { s_Color = value; }
        }

        private SpriteAnimation s_SpriteAnimation = new SpriteAnimation(0, 0);

        public SpriteAnimation SpriteAnimation
        {
            get { return s_SpriteAnimation; }
            set { s_SpriteAnimation = value; }
        }

        private bool s_ShouldSystemAnimate = false;

        public bool ShouldSystemAnimate
        {
            get { return s_ShouldSystemAnimate; }
            set { s_ShouldSystemAnimate = value; }
        }

		public bool Visible = true;

    }
}
