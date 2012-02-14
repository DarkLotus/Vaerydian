using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

using ECSFramework;

namespace Vaerydian.Components
{
    public class Sprite : IComponent
    {

        private static int s_TypeID;
        private int s_EntityID;

        private String s_TextureName;

        public Sprite() { }

        public Sprite(String textureName, String normalName)
        {
            s_TextureName = textureName;
            s_NormalName = normalName;
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
    }
}
