/*
 Author:
      Thomas H. Jonell <@Net_Gnome>
 
 Copyright (c) 2013 Thomas H. Jonell

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Lesser General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
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
