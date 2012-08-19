using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAKeyFrameTest
{
    class Animation
    {
        public int a_ElapsedTime;

        public int a_AnimationTime;

        public List<KeyFrame> a_KeyFrames = new List<KeyFrame>();

        public Texture2D a_Texture;

        public Vector2 a_Origin;

        public Vector2 a_RotationOrigin;

        public float a_Rotation;

        public void updateTime(GameTime gameTime)
        {
            a_ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (a_ElapsedTime >= a_AnimationTime)
            {
                a_ElapsedTime = 0;
            }
        }

        public Vector2 getKeyPosition()
        {
            for (int i = 0; i < a_KeyFrames.Count; i++)
            {
                if (i > 0)
                {
                    if (a_ElapsedTime <= a_KeyFrames[i].k_KeyTime && a_ElapsedTime > a_KeyFrames[i - 1].k_KeyTime)
                        return a_Origin + tweenKeyFramesPosition(a_KeyFrames[i - 1], a_KeyFrames[i], a_ElapsedTime);
                }
            }


            return a_Origin + Vector2.Zero;
        }


        private Vector2 tweenKeyFramesPosition(KeyFrame a, KeyFrame b, int time)
        {
            int timeBetweenFrames = b.k_KeyTime - a.k_KeyTime;
            int timeAfterA = time - a.k_KeyTime;
            float percentTween = (float)timeAfterA / (float)timeBetweenFrames;

            Vector2 aToB = b.k_KeyPosition - a.k_KeyPosition;

            return a.k_KeyPosition + (aToB * percentTween);
        }

        public float getKeyRotation()
        {
            for (int i = 0; i < a_KeyFrames.Count; i++)
            {
                if (i > 0)
                {
                    if (a_ElapsedTime <= a_KeyFrames[i].k_KeyTime && a_ElapsedTime > a_KeyFrames[i - 1].k_KeyTime)
                        return tweenKeyFramesRotation(a_KeyFrames[i - 1], a_KeyFrames[i], a_ElapsedTime);
                }
            }
            return a_Rotation;
        }


        private float tweenKeyFramesRotation(KeyFrame a, KeyFrame b, int time)
        {
            int timeBetweenFrames = b.k_KeyTime - a.k_KeyTime;
            int timeAfterA = time - a.k_KeyTime;
            float percentTween = (float)timeAfterA / (float)timeBetweenFrames;

            float aToB = b.k_KeyRotation - a.k_KeyRotation;

            return a.k_KeyRotation + aToB * percentTween;
        }
    }
}
