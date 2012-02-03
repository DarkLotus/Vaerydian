using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Vaerydian
{
    static class InputManager
    {


        private static KeyboardState i_currentKeyboardState;
        private static KeyboardState i_previousKeyboardState;
        private static MouseState i_currentMouseState;
        private static MouseState i_previousMouseState;

        private static bool i_yesExit = false;

        /// <summary>
        /// checked on next "go around" if true, game will exit
        /// </summary>
        public static bool YesExit { get { return i_yesExit; } set { i_yesExit = value; } }

        private static bool im_YesScreenshot = false;

        public static bool YesScreenshot
        {
            get { return InputManager.im_YesScreenshot; }
            set { InputManager.im_YesScreenshot = value; }
        }

        public static void Update()
        {
            // update the keyboard state
            i_previousKeyboardState = i_currentKeyboardState;
            i_currentKeyboardState = Keyboard.GetState();

            //update the mouse state
            i_previousMouseState = i_currentMouseState;
            i_currentMouseState = Mouse.GetState();

        }

        /// <summary>
        /// is the current key pressed
        /// </summary>
        /// <param name="key">the key to check</param>
        /// <returns>true if pressed otherwise false</returns>
        public static bool isKeyPressed(Keys key)
        {
            return i_currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// has the key recently been toggled
        /// </summary>
        /// <param name="key">key to check</param>
        /// <returns>true if just toggled otherwise false</returns>
        public static bool isKeyToggled(Keys key)
        {
            return (i_currentKeyboardState.IsKeyDown(key) && !i_previousKeyboardState.IsKeyDown(key));
        }

        public static Vector2 getMousePosition()
        {
            return new Vector2(i_currentMouseState.X, i_currentMouseState.Y);
        }
        
    }
}
