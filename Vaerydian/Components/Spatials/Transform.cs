using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Microsoft.Xna.Framework;


namespace Vaerydian.Components.Spatials
{
    public class Transform : IComponent
    {
        private static int t_TypeID;
        private int t_EntityID;

        public Transform() { }

        public int getEntityId()
        {
            return t_EntityID;
        }

        public int getTypeId()
        {
            return t_TypeID;
        }

        public void setEntityId(int entityId)
        {
            t_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            t_TypeID = typeId;
        }

        private float t_Rotation = 0f;

        public float Rotation
        {
            get { return t_Rotation; }
            set { t_Rotation = value; }
        }

        private Vector2 t_RotationOrigin = new Vector2(0);

        public Vector2 RotationOrigin
        {
            get { return t_RotationOrigin; }
            set { t_RotationOrigin = value; }
        }

    }
}
