using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Vaerydian.Utils
{
    public class Animation
    {
        private int a_FrameRate;

        private int a_Size;

        private int a_ElapsedTime = 0;

        private int a_LastFrame = 0;

        public Animation() { }

        public Animation(int size, int frameRate) 
        {
            a_Size = size;
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
                if (a_LastFrame == a_Size)
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
                if (a_LastFrame == a_Size)
                    a_LastFrame = 0;

                //return frame
                return a_LastFrame;
            }
            else
                return a_LastFrame;
        }


    }
}
