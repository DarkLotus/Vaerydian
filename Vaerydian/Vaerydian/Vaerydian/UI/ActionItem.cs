using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using ECSFramework;

using Glimpse.Components;
using Glimpse.Input;
using Glimpse.Controls;

namespace Vaerydian.UI.implemented
{
    enum MouseButton
    {
        Left,
        Right
    }

    class ActionItem : IUserInterface
    {
        private Action<IUserInterface, InterfaceArgs> a_Action;
        private Keys a_Key;
        private MouseButton a_MouseButton;
        private bool a_KeyDriven;
        private bool a_OnKeyPress = false;
        private bool a_OnMousePress = false;
        private Rectangle a_Bounds;

        public Rectangle Bounds
        {
            get { return a_Bounds; }
            set { a_Bounds = value; }
        }
        private String a_TextureName;
        private Texture2D a_Texture;
        private GraphicsDevice a_GraphicsDevice;
        private ContentManager a_ContentManager;

        private SpriteBatch a_SpriteBatch;

        public event InterfaceHandler OnKeyPress;

        /// <summary>
        /// Key-based activation hot bar item
        /// </summary>
        /// <param name="action"></param>
        /// <param name="onKeyPress"></param>
        /// <param name="key"></param>
        /// <param name="boundingRect"></param>
        public ActionItem(Action<IUserInterface, InterfaceArgs> action, bool onKeyPress, Keys key, Rectangle boundingRect, String textureName)
        {
            a_Action = action;
            a_Key = key;
            a_KeyDriven = true;
            a_OnKeyPress = onKeyPress;
            a_Bounds = boundingRect;
            a_TextureName = textureName;

            InputManager.keybordEvent += this.keyboardListener;
            
        }

        /// <summary>
        /// mouse-based activiation hot bar item
        /// </summary>
        /// <param name="action"></param>
        /// <param name="onMousePress"></param>
        /// <param name="mouseButton"></param>
        /// <param name="boundingRect"></param>
        public ActionItem(Action<IUserInterface, InterfaceArgs> action, bool onMousePress, MouseButton mouseButton, Rectangle boundingRect, String textureName)
        {
            a_Action = action;
            a_MouseButton = mouseButton;
            a_KeyDriven = false;
            a_OnMousePress = onMousePress;
            a_Bounds = boundingRect;
            a_TextureName = textureName;
        }

        private Entity a_Owner;

        public Entity Owner
        {
            get
            {
                return a_Owner;
            }
            set
            {
                a_Owner = value;
            }
        }

        private Entity a_Caller;

        public Entity Caller
        {
            get
            {
                return a_Caller;
            }
            set
            {
                a_Caller = value;
            }
        }

        private ECSInstance a_ECSInstance;

        public ECSInstance ECSInstance
        {
            get
            {
                return a_ECSInstance;
            }
            set
            {
                a_ECSInstance = value;
            }
        }

        private GameContainer a_Container;

        public GameContainer GameContainer
        {
            get
            {
                return a_Container;
            }
            set
            {
                a_Container = value;
            }
        }

        private bool a_IsInitialized = false;

        public bool IsInitialized
        {
            get {return a_IsInitialized; }
        }

        public void initialize()
        {
            a_IsInitialized = true;
        }

        private bool a_IsLoaded = false;

        public bool IsLoaded
        {
            get { return a_IsLoaded; }
        }

        public void loadContent()
        {
            a_SpriteBatch = new SpriteBatch(a_GraphicsDevice);

            a_Texture = a_ContentManager.Load<Texture2D>(a_TextureName);

            a_IsLoaded = true;
        }

        private bool a_IsUnloaded = false;

        public bool IsUnloaded
        {
            get { return a_IsUnloaded; }
        }

        public void unloadContent()
        {


            a_IsUnloaded = true;
        }

        public void update(int elapsedTime)
        {
            /*
            //run key based action or mouse based action
            if (a_KeyDriven)
            {
                if (InputManager.isKeyToggled(a_Key) && !a_OnKeyPress)
                    a_Action.Invoke();
                else if (InputManager.isKeyPressed(a_Key) && a_OnKeyPress)
                    a_Action.Invoke();
            }
            else
            {
                //does bounds contain mouse?
                bool contains = a_BoundingRect.Contains(InputManager.getMousePositionPoint());

                //left or right button?
                if (a_MouseButton == MouseButton.Left)
                {
                    //clicked or pressed?
                    if (InputManager.isLeftButtonClicked() && !a_OnMousePress && contains)
                        a_Action.Invoke();
                    else if (InputManager.isLeftButtonDown() && a_OnMousePress && contains)
                        a_Action.Invoke();
                }
                else
                {
                    //clicked or pressed?
                    if (InputManager.isRightButtonClicked() && !a_OnMousePress && contains)
                        a_Action.Invoke();
                    else if (InputManager.isRightButtonDown() && a_OnMousePress && contains)
                        a_Action.Invoke();
                }
            }
             */

            
        }

        public void draw(int elapsedTime)
        {
            a_SpriteBatch.Begin();

            a_SpriteBatch.Draw(a_Texture, a_Bounds, Color.White);

            a_SpriteBatch.End(); 
        }


        public void keyboardListener(InputArgs args)
        {
            //if (InputManager.isKeyPressed(this.a_Key))
                //OnKeyPress(this,new InterfaceArgs(InterfaceArgType.keyboard));
        }




        

        public ContentManager ContentManager
        {
            get { return a_ContentManager; }
            set { a_ContentManager = value; }
        }



        public GraphicsDevice GraphicsDevice
        {
            get { return a_GraphicsDevice; }
            set { a_GraphicsDevice = value; }
        }
    }
}
