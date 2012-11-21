using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Vaerydian.Utils
{
    public class SpriteAnimation
    {
        private int a_FrameRate;

        private int a_Frames;

        private int a_ElapsedTime = 0;

        private int a_LastFrame = 0;

        public SpriteAnimation() { }

        public SpriteAnimation(int frames, int frameRate) 
        {
            a_Frames = frames;
            a_FrameRate = frameRate;
        }

        public void reset()
        {
            a_ElapsedTime = 0;
            a_LastFrame = 0;
        }

        public int updateFrame(GameTime gameTime)
        {
            a_ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (a_ElapsedTime > a_FrameRate)
            {
                //reset elapsed
                a_ElapsedTime = 0;

                //update frame
                a_LastFrame++;

                //make sure we didnt run over
                if (a_LastFrame == a_Frames)
                    a_LastFrame = 0;

                //return frame
                return a_LastFrame;
            }
            else
                return a_LastFrame;
        }

        public int updateFrame(int gameTime)
        {
            a_ElapsedTime += gameTime;

            if (a_ElapsedTime > a_FrameRate)
            {
                //reset elapsed
                a_ElapsedTime = 0;

                //update frame
                a_LastFrame++;

                //make sure we didnt run over
                if (a_LastFrame == a_Frames)
                    a_LastFrame = 0;

                //return frame
                return a_LastFrame;
            }
            else
                return a_LastFrame;
        }

        /// <summary>
        /// number of frames in this animation
        /// </summary>
        public int Frames
        {
            get { return a_Frames; }
            set { a_Frames = value; }
        }
    }
}
