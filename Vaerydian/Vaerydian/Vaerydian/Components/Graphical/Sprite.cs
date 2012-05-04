using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

using ECSFramework;

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

    }
}
