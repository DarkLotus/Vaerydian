using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ECSFramework;
using Vaerydian.Components;

namespace Vaerydian.UI
{
    class TimedDialogWindow : IUserInterface
    {

        private ECSInstance t_ECSInstance;

        private GameContainer t_Container;

        private SpriteBatch t_Spritebatch;

        private String t_Dialog;

        private String t_Name = "Char Name";

        private Texture2D t_FrameBackground;

        private int t_Duration = 3000;

        private int t_ElapsedTime = 0;

        private Vector2 t_Origin;

        private Vector2 t_NameDimensions;

        private Vector2 t_DialogDimensions;

        private int t_Offset = 10;

        private ComponentMapper t_PositionMapper;
        private ComponentMapper t_ViewPortMapper;

        private Entity t_Camera;

        /// <summary>
        /// create a dialog window with the following attributes
        /// </summary>
        /// <param name="dialog">dialog to be displayed</param>
        /// <param name="origin">origin of the window</param>
        /// <param name="size">size of the window</param>
        /// <param name="startTime">time window is called</param>
        /// <param name="duration">time window is to be alive</param>
        public TimedDialogWindow(Entity caller, String dialog, Vector2 origin, String name, int duration)
        {
            t_Caller = caller;
            t_Dialog = dialog;
            t_Origin = origin;
            t_Name = name;
            t_Duration = duration;
        }
        
        private Entity t_Owner;

        public Entity Owner 
        {
            get { return t_Owner; }
            set { t_Owner = value; }
        }

        private Entity t_Caller;

        public Entity Caller {
            get { return t_Caller;}
            set { t_Caller = value;}
        }

        public ECSInstance ECSInstance
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
            t_Offset = FontManager.Instance.Fonts["StartScreen"].LineSpacing;
            
            t_NameDimensions = FontManager.Instance.Fonts["StartScreen"].MeasureString(t_Name);

            t_DialogDimensions = FontManager.Instance.Fonts["General"].MeasureString(t_Dialog);

            t_PositionMapper = new ComponentMapper(new Position(), t_ECSInstance);
            t_ViewPortMapper = new ComponentMapper(new ViewPort(), t_ECSInstance);

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
            
            t_FrameBackground = t_Container.ContentManager.Load<Texture2D>("dialog_bubble");

            t_Camera = t_ECSInstance.TagManager.getEntityByTag("CAMERA");

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

            Position pos = (Position)t_PositionMapper.get(t_Caller);
            ViewPort camera = (ViewPort)t_ViewPortMapper.get(t_Camera);

            if(pos != null)
                t_Origin = pos.getPosition() - camera.getOrigin() - new Vector2(0,t_Offset*2);
        }

        public void draw(int elapsedTime)
        {
            t_Spritebatch.Begin();

            //draw the frame background
            t_Spritebatch.Draw(t_FrameBackground, new Rectangle((int)t_Origin.X - t_Offset/2 - 4, (int)t_Origin.Y - 4, (int)t_DialogDimensions.X + 8 + t_Offset, (int)(t_DialogDimensions.Y*2 + 8)), Color.Orange);
            t_Spritebatch.Draw(t_FrameBackground, new Rectangle((int)t_Origin.X - t_Offset/2 - 2, (int)t_Origin.Y - 2, (int)t_DialogDimensions.X + 4 + t_Offset, (int)(t_DialogDimensions.Y*2 + 4)), Color.White);

            //draw name-plate
            t_Spritebatch.Draw(t_FrameBackground, new Rectangle((int)(t_Origin.X - t_Offset*1.5 - 4), (int)(t_Origin.Y - t_Offset - 4), (int)(t_NameDimensions.X + 8 + t_Offset), (int)(t_NameDimensions.Y + 8)), Color.Orange);
            t_Spritebatch.Draw(t_FrameBackground, new Rectangle((int)(t_Origin.X - t_Offset*1.5 - 2), (int)(t_Origin.Y - t_Offset - 2), (int)(t_NameDimensions.X + 4 + t_Offset), (int)(t_NameDimensions.Y + 4)), new Color(0, 116, 196));
            t_Spritebatch.DrawString(FontManager.Instance.Fonts["StartScreen"], t_Name, new Vector2(t_Origin.X - t_Offset, t_Origin.Y - t_Offset), Color.White);

            //draw the text
            t_Spritebatch.DrawString(FontManager.Instance.Fonts["General"], t_Dialog, new Vector2(t_Origin.X, t_Origin.Y + t_Offset/2 ), Color.Black);


            t_Spritebatch.End();
        }
    }
}
