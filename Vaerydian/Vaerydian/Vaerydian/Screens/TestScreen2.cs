using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vaerydian.Screens
{
    class TestScreen2 : Screen
    {

        SpriteFont font;

        /// <summary>
        /// handles all screen related user input
        /// </summary>
        public override void handleInput(GameTime gameTime)
        {
            base.handleInput(gameTime);

            //check to see if escape was recently pressed
            if (InputManager.isKeyToggled(Keys.F))
                InputManager.yesExit = true;

            if (InputManager.isKeyToggled(Keys.G))
                ScreenManager.removeScreen(this);

        }

        /// <summary>
        /// load any screen related content
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            ContentManager cm = ScreenManager.Game.Content;
            font = cm.Load<SpriteFont>("SpriteFont1");
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

            sb.DrawString(font, "TestScreen2", new Vector2(0, 14), Color.Red);

            sb.End();
        }

    }
}
