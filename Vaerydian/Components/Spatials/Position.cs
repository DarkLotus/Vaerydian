using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

namespace Vaerydian.Components.Spatials
{
    public class Position : IComponent
    {
        private static int p_TypeID;

        public static int TypeID
        {
            get { return Position.p_TypeID; }
            set { Position.p_TypeID = value; }
        }

        private int p_EntityID;

        private Vector2 p_Position;

        public Vector2 Pos
        {
            get { return p_Position; }
            set { p_Position = value; }
        }

        private Vector2 p_Offset;

        public Vector2 Offset
        {
            get { return p_Offset; }
            set { p_Offset = value; }
        }

        public Position() { }

        public Position(Vector2 position, Vector2 offset)
        {
            p_Position = position;
            p_Offset = offset;
        }

        public int getEntityId()
        {
            return p_EntityID;
        }

        public int getTypeId()
        {
            return p_TypeID;
        }

        public void setEntityId(int entityId)
        {
            p_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            p_TypeID = typeId;
        }

    }
}
