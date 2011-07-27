using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vaerydian.Screens
{
    class TestScreen1 : Screen
    {
        /// <summary>
        /// handles all screen related user input
        /// </summary>
        public override void handleInput(GameTime gameTime)
        {
            base.handleInput(gameTime);

            //check to see if escape was recently pressed
            if (InputManager.isKeyToggled(Keys.Escape))
                InputManager.yesExit = true;

            //create a new screen
            if (InputManager.isKeyToggled(Keys.Q))
                ScreenManager.addScreen(new TestScreen2());
         
        }

        /// <summary>
        /// load any screen related content
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// updates the start screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


        }

        /// <summary>
        /// draws the start screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch sb = ScreenManager.SpriteBatch;

            sb.Begin();

            sb.DrawString(FontManager.Instance.Fonts["General"], "TestScreen1", new Vector2(0, 0), Color.Red);

            sb.End();
        }
    }
}
