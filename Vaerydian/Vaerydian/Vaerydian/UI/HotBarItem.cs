using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ECSFramework;

namespace Vaerydian.UI
{
    enum MouseButton
    {
        Left,
        Right
    }

    class HotBarItem : IUserInterface
    {
        private Action h_Action;

        private Keys h_Key;

        private MouseButton h_MouseButton;

        private bool h_KeyDriven;

        private bool h_OnKeyPress = false;

        private bool h_OnMousePress = false;

        private Rectangle h_BoundingRect;

        private String h_TextureName;
        private Texture2D h_Texture;

        /// <summary>
        /// Key-based activation hot bar item
        /// </summary>
        /// <param name="action"></param>
        /// <param name="onKeyPress"></param>
        /// <param name="key"></param>
        /// <param name="boundingRect"></param>
        public HotBarItem(Action action, bool onKeyPress, Keys key, Rectangle boundingRect, String textureName)
        {
            h_Action = action;
            h_Key = key;
            h_KeyDriven = true;
            h_OnKeyPress = onKeyPress;
            h_BoundingRect = boundingRect;
            h_TextureName = textureName;
        }

        /// <summary>
        /// mouse-based activiation hot bar item
        /// </summary>
        /// <param name="action"></param>
        /// <param name="onMousePress"></param>
        /// <param name="mouseButton"></param>
        /// <param name="boundingRect"></param>
        public HotBarItem(Action action, bool onMousePress, MouseButton mouseButton, Rectangle boundingRect, String textureName)
        {
            h_Action = action;
            h_MouseButton = mouseButton;
            h_KeyDriven = false;
            h_OnMousePress = onMousePress;
            h_BoundingRect = boundingRect;
            h_TextureName = textureName;
        }

        private Entity h_Owner;

        public Entity Owner
        {
            get
            {
                return h_Owner;
            }
            set
            {
                h_Owner = value;
            }
        }

        private Entity h_Caller;

        public Entity Caller
        {
            get
            {
                return h_Caller;
            }
            set
            {
                h_Caller = value;
            }
        }

        private ECSInstance h_ECSInstance;

        public ECSInstance ECSInstance
        {
            get
            {
                return h_ECSInstance;
            }
            set
            {
                h_ECSInstance = value;
            }
        }

        private GameContainer h_Container;

        public GameContainer GameContainer
        {
            get
            {
                return h_Container;
            }
            set
            {
                h_Container = value;
            }
        }

        private bool h_IsInitialized = false;

        public bool IsInitialized
        {
            get {return h_IsInitialized; }
        }

        public void initialize()
        {
            h_IsInitialized = true;
        }

        private bool h_IsLoaded = false;

        public bool IsLoaded
        {
            get { return h_IsLoaded; }
        }

        public void loadContent()
        {
            h_IsLoaded = true;
        }

        private bool h_IsUnloaded = false;

        public bool IsUnloaded
        {
            get { return h_IsUnloaded; }
        }

        public void unloadContent()
        {


            h_IsUnloaded = true;
        }

        public void update(int elapsedTime)
        {
            //run key based action or mouse based action
            if (h_KeyDriven)
            {
                if (InputManager.isKeyToggled(h_Key) && !h_OnKeyPress)
                    h_Action.Invoke();
                else if (InputManager.isKeyPressed(h_Key) && h_OnKeyPress)
                    h_Action.Invoke();
            }
            else
            {
                //does bounds contain mouse?
                bool contains = h_BoundingRect.Contains(InputManager.getMousePositionPoint());

                //left or right button?
                if (h_MouseButton == MouseButton.Left)
                {
                    //clicked or pressed?
                    if (InputManager.isLeftButtonClicked() && !h_OnMousePress && contains)
                        h_Action.Invoke();
                    else if (InputManager.isLeftButtonDown() && h_OnMousePress && contains)
                        h_Action.Invoke();
                }
                else
                {
                    //clicked or pressed?
                    if (InputManager.isRightButtonClicked() && !h_OnMousePress && contains)
                        h_Action.Invoke();
                    else if (InputManager.isRightButtonDown() && h_OnMousePress && contains)
                        h_Action.Invoke();
                }
            }
        }

        public void draw(int elapsedTime)
        {
            
        }
    }
}
