using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

namespace Vaerydian.Components.Graphical
{
    public class ViewPort : IComponent
    {

        private static int v_TypeID;
        private int v_EntityID;

        private Vector2 v_Origin;
        private Vector2 v_Dimensions;

        public ViewPort() { }

        public ViewPort(Vector2 origin, Vector2 dimensions) 
        {
            v_Origin = origin;
            v_Dimensions = dimensions;
        }

        public int getEntityId()
        {
            return v_EntityID;
        }

        public int getTypeId()
        {
            return v_TypeID;
        }

        public Vector2 getOrigin()
        {
            return v_Origin;
        }

        public Vector2 getDimensions()
        {
            return v_Dimensions;
        }

        public void setEntityId(int entityId)
        {
            v_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            v_TypeID = typeId;
        }

        public void setOrigin(Vector2 origin)
        {
            v_Origin = origin;
        }

        public void setDimensions(Vector2 dimensions)
        {
            v_Dimensions = dimensions;
        }
    }
}
