using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAKeyFrameTest
{
    class KeyFrame
    {
        public KeyFrame(int keyTime, Vector2 keyPosition, float keyRotation)
        {
            k_KeyTime = keyTime;
            k_KeyPosition = keyPosition;
            k_KeyRotation = keyRotation;
        }


        public Vector2 k_KeyPosition = Vector2.Zero;

        public float k_KeyRotation = 0f;

        public int k_KeyTime = 0;

    }
}
