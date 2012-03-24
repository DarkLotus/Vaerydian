using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ECSFramework;

namespace Vaerydian.UI
{
    class TimedDialogWindow : IUserInterface
    {

        private ECSInstance t_ECSInstance;

        private GameContainer t_Container;

        private SpriteBatch t_Spritebatch;

        private String t_Dialog;

        private Texture2D t_FrameBackground;

        private Texture2D t_FrameHorizontal;

        private Texture2D t_FrameVertical;

        private Texture2D t_FrameCorner;

        private Texture2D t_FrameCorner2;

        private int t_Duration = 3000;

        private int t_ElapsedTime = 0;

        private Vector2 t_Origin;

        private Vector2 t_Size;

        private int t_Offset = 10;

        /// <summary>
        /// create a dialog window with the following attributes
        /// </summary>
        /// <param name="dialog">dialog to be displayed</param>
        /// <param name="origin">origin of the window</param>
        /// <param name="size">size of the window</param>
        /// <param name="startTime">time window is called</param>
        /// <param name="duration">time window is to be alive</param>
        public TimedDialogWindow(String dialog, Vector2 origin, Vector2 size, int duration)
        {
            t_Dialog = dialog;
            t_Origin = origin;
            t_Size = size;
            t_Duration = duration;
        }
        
        private Entity t_Owner;

        public Entity Owner 
        {
            get { return t_Owner; }
            set { t_Owner = value; }
        }

        public ECSFramework.ECSInstance ECSInstance
        {
            get
            {
                return t_ECSInstance;
            }
            set
            {
                t_ECSInstance = value;
            }
        }

        public GameContainer GameContainer
        {
            get
            {
                return t_Container;
            }
            set
            {
                t_Container = value;
            }
        }
        
        private bool t_IsInitialized = false;
        
        public bool IsInitialized
        {
            get { return t_IsInitialized; }
        }

        public void initialize()
        {


            t_IsInitialized = true;
        }

        private bool t_IsLoaded = false;

        public bool IsLoaded
        {
            get { return t_IsLoaded; }
        }

        public void loadContent()
        {
            t_Spritebatch = t_Container.SpriteBatch;
            
            t_FrameBackground = t_Container.ContentManager.Load<Texture2D>("frame");
            t_FrameHorizontal = t_Container.ContentManager.Load<Texture2D>("frame_h");
            t_FrameVertical = t_Container.ContentManager.Load<Texture2D>("frame_v");
            t_FrameCorner = t_Container.ContentManager.Load<Texture2D>("frame_c");
            t_FrameCorner2 = t_Container.ContentManager.Load<Texture2D>("frame_c2");

            t_IsLoaded = true;
        }

        private bool t_isUnloaded = false;

        public bool IsUnloaded
        {
            get { return t_isUnloaded; }
        }

        public void unloadContent()
        {
            t_isUnloaded = true;
        }

        public void update(int elapsedTime)
        {
            t_ElapsedTime += elapsedTime;

            if (t_ElapsedTime > t_Duration)
                t_ECSInstance.deleteEntity(t_Owner);
        }

        public void draw(int elapsedTime)
        {
            t_Spritebatch.Begin();

            //draw the frame background
            t_Spritebatch.Draw(t_FrameBackground, new Vector2(t_Origin.X, t_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                             new Vector2(t_Size.X, t_Size.Y), SpriteEffects.None, 0);

            //draw the frame vertical borders
            //draw right border
            t_Spritebatch.Draw(t_FrameVertical, new Vector2(t_Origin.X, t_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, t_Size.Y), SpriteEffects.None, 0);

            //draw left border
            t_Spritebatch.Draw(t_FrameVertical, new Vector2(t_Origin.X + t_Size.X, t_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, t_Size.Y), SpriteEffects.FlipHorizontally, 0);

            //draw frame horizontal borders
            //draw top border
            t_Spritebatch.Draw(t_FrameHorizontal, new Vector2(t_Origin.X, t_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(t_Size.X, 1), SpriteEffects.None, 0);
            //draw bottom border
            t_Spritebatch.Draw(t_FrameHorizontal, new Vector2(t_Origin.X, t_Origin.Y + t_Size.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(t_Size.X, 1), SpriteEffects.FlipVertically, 0);

            //draw corners
            //UL
            t_Spritebatch.Draw(t_FrameCorner, new Vector2(t_Origin.X, t_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, 1), SpriteEffects.None, 0);

            //UR
            t_Spritebatch.Draw(t_FrameCorner, new Vector2(t_Origin.X + t_Size.X, t_Origin.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, 1), SpriteEffects.FlipHorizontally, 0);

            //LL
            t_Spritebatch.Draw(t_FrameCorner, new Vector2(t_Origin.X, t_Origin.Y + t_Size.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, 1), SpriteEffects.FlipVertically, 0);
            //LR
            t_Spritebatch.Draw(t_FrameCorner2, new Vector2(t_Origin.X + t_Size.X, t_Origin.Y + t_Size.Y), null, Color.White, 0, new Vector2(0, 0),
                                         new Vector2(1, 1), SpriteEffects.None, 0);

            //draw the text
            t_Spritebatch.DrawString(FontManager.Instance.Fonts["General"], t_Dialog, new Vector2(t_Origin.X + t_Offset, t_Origin.Y + t_Offset), Color.White);


            t_Spritebatch.End();
        }
    }
}
