using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Microsoft.Xna.Framework;

namespace Vaerydian.Components
{
    public class Heading : IComponent
    {
        private static int h_TypeID;
        private int h_EntityID;

        private Vector2 h_Heading;

        public Heading() { }

        public Heading(Vector2 heading)
        {
            h_Heading = heading;
        }

        public int getEntityId()
        {
            return h_EntityID;
        }

        public int getTypeId()
        {
            return h_TypeID;
        }

        public Vector2 getHeading()
        {
            return h_Heading;
        }

        public void setEntityId(int entityId)
        {
            h_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            h_TypeID = typeId;
        }

        public void setHeading(Vector2 heading)
        {
            h_Heading = heading;
        }

    }
}
