using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Vaerydian.Windows
{
    class DialogWindow : Window
    {

        private String dw_Dialog;

        private List<String> dw_DialogList = new List<String>();

        private Texture2D dw_FrameBackground;

        private Texture2D dw_FrameHorizontal;

        private Texture2D dw_FrameVertical;

        private Texture2D dw_FrameCorner;

        private Texture2D dw_FrameCorner2;

        private Point dw_Origin;

        private Point dw_Size;

        private int dw_Offset = 10;

        private int dw_LineLength;

        /// <summary>
        /// create a dialog window with the following attributes
        /// </summary>
        /// <param name="dialog">dialog to be displayed</param>
        /// <param name="origin">origin of the window</param>
        /// <param name="size">size of the window</param>
        /// <param name="startTime">time window is called</param>
        /// <param name="duration">time window is to be alive</param>
        public DialogWindow(String dialog, int lineLengh, Point origin, Point size)
        {
            dw_Dialog = dialog;
            dw_Origin = origin;
            dw_Size = size;
            dw_LineLength = lineLengh;
        }

        /// <summary>
        /// initialize the windo
        /// </summary>
        public override void Initialize()
        {
            //figure out the offset
            dw_Offset = (int) FontManager.Instance.Fonts["General"].LineSpacing;
            
            //create some placeholders
            String temp = "";
            String[] tempArray;
            
            //split the diaglog into tokens
            tempArray = dw_Dialog.Split(' ');

            //sum tokens together until they are greater than the designated line length,
            //then add them to the dialog list
            for (int i = 0; i < tempArray.Length; i++)
            {   //check the length of the temp string and the next token
                if((temp.Length + tempArray[i].Length) > dw_LineLength)
                {
                    //its too long, so add the existing temp to the list
                    dw_DialogList.Add(temp.Trim());
                    //make temp equal to the next token
                    temp = tempArray[i];
                }
                else
                {   //add to temp the next token and place a space between them
                    temp += " " + tempArray[i] ;
                }
            }
            //some tokens may have been forgotten, so check for them and add them
            if (temp != "")
            {
                if (dw_DialogList.Count == 0)
                    dw_DialogList.Add(temp);
                if (temp != dw_DialogList[dw_DialogList.Count - 1])
                    dw_DialogList.Add(temp.Trim());
            }
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
            for (int i = 0; i < dw_DialogList.Count; i++)
            {
                spritebatch.DrawString(FontManager.Instance.Fonts["General"], dw_DialogList[i], new Vector2(dw_Origin.X + dw_Offset, dw_Origin.Y + dw_Offset * (i + 1)), Color.White);
            }

            spritebatch.End();

        }

        public void killWindow()
        {
            this.WindowManager.removeWindow(this);
        }

    }
}
