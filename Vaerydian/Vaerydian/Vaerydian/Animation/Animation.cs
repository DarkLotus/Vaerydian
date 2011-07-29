//Animation used from XNA example

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vaerydian.Animation
{
    class Animation
    {
        /// <summary>
        /// The image representing the collection of images used for animation
        /// </summary>
        private Texture2D a_SpriteStrip;

        /// <summary>
        /// The scale used to display the sprite strip
        /// </summary>
        private float a_Scale;

        /// <summary>
        ///  The time since we last updated the frame
        /// </summary>
        private int a_ElapsedTime;

        /// <summary>
        ///  The time we display a frame until the next one
        /// </summary>
        private int a_FrameTime;

        /// <summary>
        ///  The number of frames that the animation contains
        /// </summary>
        private int a_FrameCount;

        /// <summary>
        ///  The index of the current frame we are displaying
        /// </summary>
        private int a_CurrentFrame;

        /// <summary>
        ///  The color of the frame we will be displaying
        /// </summary>
        private Color a_Color;

        /// <summary>
        ///  The area of the image strip we want to display
        /// </summary>
        private Rectangle a_SourceRect = new Rectangle();

        /// <summary>
        ///  The area where we want to display the image strip in the game
        /// </summary>
        private Rectangle a_DestinationRect = new Rectangle();

        /// <summary>
        ///  Width of a given frame
        /// </summary>
        private int a_FrameWidth;
        /// <summary>
        ///  Width of a given frame
        /// </summary>
        public int FrameWidth
        {
            get { return a_FrameWidth; }
            set { a_FrameWidth = value; }
        }

        /// <summary>
        ///  Height of a given frame
        /// </summary>
        private int a_FrameHeight;
        /// <summary>
        ///  Height of a given frame
        /// </summary>
        public int FrameHeight
        {
            get { return a_FrameHeight; }
            set { a_FrameHeight = value; }
        }

        /// <summary>
        /// The state of the Animation
        /// </summary>
        private bool a_Active;
        /// <summary>
        /// The state of the Animation
        /// </summary>
        public bool Active
        {
            get { return a_Active; }
            set { a_Active = value; }
        }

        /// <summary>
        ///  Determines if the animation will keep playing or deactivate after one run
        /// </summary>
        private bool a_Looping;
        /// <summary>
        ///  Determines if the animation will keep playing or deactivate after one run
        /// </summary>
        public bool Looping
        {
            get { return a_Looping; }
            set { a_Looping = value; }
        }

        /// <summary>
        ///  Width of a given frame
        /// </summary>
        private Vector2 a_Position;
        /// <summary>
        ///  Width of a given frame
        /// </summary>
        public Vector2 Position
        {
            get { return a_Position; }
            set { a_Position = value; }
        }

        /// <summary>
        /// initializes the animation to begin animating
        /// </summary>
        /// <param name="texture">SpriteStrip texture</param>
        /// <param name="position">Origin of the animation</param>
        /// <param name="frameWidth">Width of a frame in the SpriteStrip</param>
        /// <param name="frameHeight">Height of a fram in the SpriteStrip</param>
        /// <param name="frameCount">Number of frames in the SpriteStrip</param>
        /// <param name="frametime">Duration between frames of animation</param>
        /// <param name="color">Color shading to the Animation</param>
        /// <param name="scale">Scale of the animation</param>
        /// <param name="looping">Whether the animation should loop or only run once</param>
        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool looping)
        {
            // Keep a local copy of the values passed in
            this.a_Color = color;
            this.a_FrameWidth = frameWidth;
            this.a_FrameHeight = frameHeight;
            this.a_FrameCount = frameCount;
            this.a_FrameTime = frametime;
            this.a_Scale = scale;

            a_Looping = looping;
            a_Position = position;
            a_SpriteStrip = texture;

            // Set the time to zero
            a_ElapsedTime = 0;
            a_CurrentFrame = 0;

            // Set the Animation to active by default
            a_Active = true;
        }


        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (a_Active == false)
                return;

            // Update the elapsed time
            a_ElapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (a_ElapsedTime > a_FrameTime)
            {
                // Move to the next frame
                a_CurrentFrame++;

                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (a_CurrentFrame == a_FrameCount)
                {
                    a_CurrentFrame = 0;
                    // If we are not looping deactivate the animation
                    if (a_Looping == false)
                        a_Active = false;
                }

                // Reset the elapsed time to zero
                a_ElapsedTime = 0;
            }

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            a_SourceRect = new Rectangle(a_CurrentFrame * a_FrameWidth, 0, a_FrameWidth, a_FrameHeight);

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            a_DestinationRect = new Rectangle((int)a_Position.X - (int)(a_FrameWidth * a_Scale) / 2,
            (int)a_Position.Y - (int)(a_FrameHeight * a_Scale) / 2,
            (int)(a_FrameWidth * a_Scale),
            (int)(a_FrameHeight * a_Scale));
        }


        // Draw the Animation Strip
        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw the animation when we are active
            if (a_Active)
            {
                spriteBatch.Draw(a_SpriteStrip, a_DestinationRect, a_SourceRect, a_Color);
            }
        }

    }
}
