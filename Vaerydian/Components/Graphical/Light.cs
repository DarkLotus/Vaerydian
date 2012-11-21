using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

namespace Vaerydian.Components.Graphical
{
    class Light : IComponent
    {
        private static int l_TypeID;
        private int l_EntityID;

        public Light() { }

        public int getEntityId()
        {
            return l_EntityID;
        }

        public int getTypeId()
        {
            return l_TypeID;
        }

        public void setEntityId(int entityId)
        {
            l_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            l_TypeID = typeId;
        }

        private bool l_IsEnabled;

        public bool IsEnabled
        {
            get { return l_IsEnabled; }
            set { l_IsEnabled = value; }
        }

        private float l_ActualPower;

        public float ActualPower
        {
            get { return l_ActualPower; }
            set { l_ActualPower = value; }
        }
        private Vector3 l_Position;

        public Vector3 Position
        {
            get { return l_Position; }
            set { l_Position = value; }
        }
        private Vector4 l_Color;

        public Vector4 Color
        {
            get { return l_Color; }
            set { l_Color = value; }
        }
        private int l_LightRadius;

        public int LightRadius
        {
            get { return l_LightRadius; }
            set { l_LightRadius = value; }
        }
    }
}
