using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vaerydian.Windows
{
    class TextMenuWindow : Window
    {


        private Texture2D tmw_FrameBackground;

        private Texture2D tmw_FrameHorizontal;

        private Texture2D tmw_FrameVertical;

        private Texture2D tmw_FrameCorner;

        private Texture2D tmw_FrameCorner2;

        private Vector2 tmw_Origin;

        private Vector2 tmw_Size;

        private int tmw_Offset = 10;

        private String tmw_FontName;

        /// <summary>
        /// current Menu Items
        /// </summary>
        private List<String> tmw_MenuItems = new List<String>();

        /// <summary>
        /// index of the currently highlighted menu item. Default is 0
        /// </summary>
        private int tmw_MenuIndex = 0;

        /// <summary>
        /// index of the currently highlighted menu item. Default is 0
        /// </summary>
        public int MenuIndex
        {
            get { return tmw_MenuIndex; }
            set { tmw_MenuIndex = value; }
        }



        /// <summary>
        /// create a dialog window with the following attributes
        /// </summary>
        /// <param name="dialog">dialog to be displayed</param>
        /// <param name="origin">origin of the window</param>
        /// <param name="size">size of the window</param>
        /// <param name="startTime">time window is called</param>
        /// <param name="duration">time window is to be alive</param>
        public TextMenuWindow(Vector2 origin, List<String> menuItems, String fontName)
        {
            tmw_Origin = origin;
            tmw_MenuItems = menuItems;
            tmw_FontName = fontName;
        }

        /// <summary>
        /// initialize the windo
        /// </summary>
        public override void Initialize()
        {
            tmw_Offset = FontManager.Fonts[tmw_FontName].LineSpacing;
            
            int max = 0;
            int stest;
            
            //figure out the max length of any of the menu strings
            foreach (string s in tmw_MenuItems)
            {
                stest = (int)FontManager.Fonts[tmw_FontName].MeasureString(s).X;
                if (stest > max)
                    max = stest;
            }

            //adjust the size of the menu
            tmw_Size = new Vector2(max + (2 * tmw_Offset), (tmw_MenuItems.Count+2) * tmw_Offset);

            tmw_Origin.X -= tmw_Size.X / 2;
        }


        /// <summary>
        /// any window unique content loading
        /// </summary>
        public override void LoadContent()
        {
            tmw_FrameBackground = this.WindowManager.Game.Content.Load<Texture2D>("frame");
            tmw_FrameHorizontal = this.WindowManager.Game.Content.Load<Texture2D>("frame_h");
            tmw_FrameVertical = this.WindowManager.Game.Content.Load<Texture2D>("frame_v");
            tmw_FrameCorner = this.WindowManager.Game.Content.Load<Texture2D>("frame_c");
            tmw_FrameCorner2 = this.WindowManager.Game.Content.Load<Texture2D>("frame_c2");
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
            //spritebatch.Draw(tmw_FrameBackground, new Vector2(tmw_Origin.X, tmw_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                             //new Vector2(tmw_Size.X, tmw_Size.Y), SpriteEffects.None, 0);

            spritebatch.Draw(tmw_FrameBackground, new Rectangle((int)tmw_Origin.X, (int)tmw_Origin.Y, (int)tmw_Size.X, (int)tmw_Size.Y), Color.White *0.7f);

            //draw the frame vertical borders
            //draw right border
            //spritebatch.Draw(tmw_FrameVertical, new Vector2(tmw_Origin.X, tmw_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         //new Vector2(1, tmw_Size.Y), SpriteEffects.None, 0);

            spritebatch.Draw(tmw_FrameVertical, new Rectangle((int)tmw_Origin.X, (int)tmw_Origin.Y, 5, (int)tmw_Size.Y), Color.White);

            //draw left border
            spritebatch.Draw(tmw_FrameVertical, new Vector2(tmw_Origin.X + tmw_Size.X, tmw_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, tmw_Size.Y), SpriteEffects.FlipHorizontally, 0);

            //draw frame horizontal borders
            //draw top border
            spritebatch.Draw(tmw_FrameHorizontal, new Vector2(tmw_Origin.X, tmw_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(tmw_Size.X, 1), SpriteEffects.None, 0);
            //draw bottom border
            spritebatch.Draw(tmw_FrameHorizontal, new Vector2(tmw_Origin.X, tmw_Origin.Y + tmw_Size.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(tmw_Size.X, 1), SpriteEffects.FlipVertically, 0);

            //draw corners
            //UL
            spritebatch.Draw(tmw_FrameCorner, new Vector2(tmw_Origin.X, tmw_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, 1), SpriteEffects.None, 0);

            //UR
            spritebatch.Draw(tmw_FrameCorner, new Vector2(tmw_Origin.X + tmw_Size.X, tmw_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, 1), SpriteEffects.FlipHorizontally, 0);

            //LL
            spritebatch.Draw(tmw_FrameCorner, new Vector2(tmw_Origin.X, tmw_Origin.Y + tmw_Size.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, 1), SpriteEffects.FlipVertically, 0);
            //LR
            spritebatch.Draw(tmw_FrameCorner2, new Vector2(tmw_Origin.X + tmw_Size.X, tmw_Origin.Y + tmw_Size.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, 1), SpriteEffects.None, 0);

            //draw the menu items
            for(int i = 0; i < tmw_MenuItems.Count; i++)
            {

                //highlight the indexed menu item
                if (tmw_MenuIndex == i)
                {
                    spritebatch.DrawString(FontManager.Fonts[tmw_FontName], tmw_MenuItems[i], new Vector2(tmw_Origin.X + tmw_Offset, tmw_Origin.Y + tmw_Offset * (i + 1)), Color.Yellow);
                }
                else
                {
                    spritebatch.DrawString(FontManager.Fonts[tmw_FontName], tmw_MenuItems[i], new Vector2(tmw_Origin.X + tmw_Offset, tmw_Origin.Y + tmw_Offset * (i + 1)), Color.White);
                }
            }

            spritebatch.End();

        }

        public void killWindow()
        {
            this.WindowManager.removeWindow(this);
        }


    }
}
