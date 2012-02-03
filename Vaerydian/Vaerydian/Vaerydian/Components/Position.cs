﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

namespace Vaerydian.Components
{
    public class Position : IComponent
    {
        private static int p_TypeID;
        private int p_EntityID;

        private Vector2 p_Position;

        private Vector2 p_Offset;

        public Position() { }

        public Position(Vector2 position, Vector2 offset)
        {
            p_Position = position;
        }

        public int getEntityId()
        {
            return p_EntityID;
        }

        public int getTypeId()
        {
            return p_TypeID;
        }

        public Vector2 getPosition()
        {
            return p_Position;
        }

        public Vector2 getOffset()
        {
            return p_Offset;
        }

        public void setEntityId(int entityId)
        {
            p_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            p_TypeID = typeId;
        }

        public void setPosition(Vector2 position)
        {
            p_Position = position;
        }

        public void setOffset(Vector2 offset) 
        {
            p_Offset = offset;
        }
    }
}