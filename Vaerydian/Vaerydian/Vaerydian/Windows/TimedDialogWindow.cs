﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vaerydian.Windows
{
    class TimedDialogWindow : Window
    {

        private String dw_Dialog;

        private Texture2D dw_FrameBackground;

        private Texture2D dw_FrameHorizontal;

        private Texture2D dw_FrameVertical;

        private Texture2D dw_FrameCorner;

        private Texture2D dw_FrameCorner2;

        private int dw_Duration = 3;

        private int dw_StartTime;

        private Point dw_Origin;

        private Point dw_Size;

        private int dw_Offset = 10;

        /// <summary>
        /// create a dialog window with the following attributes
        /// </summary>
        /// <param name="dialog">dialog to be displayed</param>
        /// <param name="origin">origin of the window</param>
        /// <param name="size">size of the window</param>
        /// <param name="startTime">time window is called</param>
        /// <param name="duration">time window is to be alive</param>
        public TimedDialogWindow(String dialog, Point origin, Point size, int startTime, int duration)
        {
            dw_Dialog = dialog;
            dw_Origin = origin;
            dw_Size = size;
            dw_StartTime = startTime;
            dw_Duration = duration;
        }

        /// <summary>
        /// initialize the windo
        /// </summary>
        public override void Initialize()
        {
        }


        /// <summary>
        /// any window unique content loading
        /// </summary>
        public override void LoadContent()
        {
            dw_FrameBackground = this.WindowManager.Game.Content.Load<Texture2D>("frame");
            dw_FrameHorizontal = this.WindowManager.Game.Content.Load<Texture2D>("frame_h");
            dw_FrameVertical = this.WindowManager.Game.Content.Load<Texture2D>("frame_v");
            dw_FrameCorner = this.WindowManager.Game.Content.Load<Texture2D>("frame_c");
            dw_FrameCorner2 = this.WindowManager.Game.Content.Load<Texture2D>("frame_c2");
        }

        /// <summary>
        /// any window unique content unloading
        /// </summary>
        public override void UnloadContent()
        {
        }

        /// <summary>
        /// update the window
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //kill the window if it is too old
            if (dw_Duration < (gameTime.TotalGameTime.Seconds - dw_StartTime))
                WindowManager.removeWindow(this);

        }

        /// <summary>
        /// draw the window
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch spritebatch = this.WindowManager.SpriteBatch;

            spritebatch.Begin();

            //draw the frame background
            spritebatch.Draw(dw_FrameBackground, new Vector2(dw_Origin.X, dw_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                             new Vector2(dw_Size.X, dw_Size.Y), SpriteEffects.None, 0);

            //draw the frame vertical borders
            //draw right border
            spritebatch.Draw(dw_FrameVertical, new Vector2(dw_Origin.X, dw_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, dw_Size.Y), SpriteEffects.None, 0);

            //draw left border
            spritebatch.Draw(dw_FrameVertical, new Vector2(dw_Origin.X + dw_Size.X, dw_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, dw_Size.Y), SpriteEffects.FlipHorizontally, 0);

            //draw frame horizontal borders
            //draw top border
            spritebatch.Draw(dw_FrameHorizontal, new Vector2(dw_Origin.X, dw_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(dw_Size.X, 1), SpriteEffects.None, 0);
            //draw bottom border
            spritebatch.Draw(dw_FrameHorizontal, new Vector2(dw_Origin.X, dw_Origin.Y + dw_Size.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(dw_Size.X, 1), SpriteEffects.FlipVertically, 0);

            //draw corners
            //UL
            spritebatch.Draw(dw_FrameCorner, new Vector2(dw_Origin.X, dw_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, 1), SpriteEffects.None, 0);

            //UR
            spritebatch.Draw(dw_FrameCorner, new Vector2(dw_Origin.X + dw_Size.X, dw_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, 1), SpriteEffects.FlipHorizontally, 0);

            //LL
            spritebatch.Draw(dw_FrameCorner, new Vector2(dw_Origin.X, dw_Origin.Y + dw_Size.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, 1), SpriteEffects.FlipVertically, 0);
            //LR
            spritebatch.Draw(dw_FrameCorner2, new Vector2(dw_Origin.X + dw_Size.X, dw_Origin.Y + dw_Size.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, 1), SpriteEffects.None, 0);

            //draw the text
            spritebatch.DrawString(FontManager.Instance.Fonts["General"], dw_Dialog, new Vector2(dw_Origin.X + dw_Offset, dw_Origin.Y + dw_Offset), Color.White);
            

            spritebatch.End();

        }


    }
}