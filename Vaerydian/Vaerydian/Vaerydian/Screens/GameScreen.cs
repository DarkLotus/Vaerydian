using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

namespace Vaerydian.Screens
{
    class GameScreen : Screen
    {
        private ECSInstance g_ECSInstance = new ECSInstance();
        

        public override void Initialize()
        {
            base.Initialize();

            //g_ECSInstance.SystemManager.setSystem(


        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void handleInput(GameTime gameTime)
        {
            base.handleInput(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

    }
}
