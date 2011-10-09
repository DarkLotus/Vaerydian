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


        private static KeyboardState im_currentKeyboardState;
        private static KeyboardState im_previousKeyboardState;

        private static bool im_yesExit = false;

        /// <summary>
        /// checked on next "go around" if true, game will exit
        /// </summary>
        public static bool yesExit { get { return im_yesExit; } set { im_yesExit = value; } }

        private static bool im_YesScreenshot = false;

        public static bool YesScreenshot
        {
            get { return InputManager.im_YesScreenshot; }
            set { InputManager.im_YesScreenshot = value; }
        }

        public static void Update()
        {
            // update the keyboard state
            im_previousKeyboardState = im_currentKeyboardState;
            im_currentKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// is the current key pressed
        /// </summary>
        /// <param name="key">the key to check</param>
        /// <returns>true if pressed otherwise false</returns>
        public static bool isKeyPressed(Keys key)
        {
            return im_currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// has the key recently been toggled
        /// </summary>
        /// <param name="key">key to check</param>
        /// <returns>true if just toggled otherwise false</returns>
        public static bool isKeyToggled(Keys key)
        {
            return (im_currentKeyboardState.IsKeyDown(key) && !im_previousKeyboardState.IsKeyDown(key));
        }

        
    }
}
