using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vaerydian.Utils
{
    class KeyFrame
    {
        public KeyFrame(int keyTime, Vector2 keyPosition, float keyRotation)
        {
            k_KeyTime = keyTime;
            k_KeyPosition = keyPosition;
            k_KeyRotation = keyRotation;
        }

        private Vector2 k_KeyPosition = Vector2.Zero;

        public Vector2 KeyPosition
        {
            get { return k_KeyPosition; }
            set { k_KeyPosition = value; }
        }

        private float k_KeyRotation = 0f;

        public float KeyRotation
        {
            get { return k_KeyRotation; }
            set { k_KeyRotation = value; }
        }

        private int k_KeyTime = 0;

        public int KeyTime
        {
            get { return k_KeyTime; }
            set { k_KeyTime = value; }
        }

    }
}
