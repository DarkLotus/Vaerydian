using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Vaerydian.Combat;


namespace Vaerydian.Screens
{
    /// <summary>
    /// handles combat sessions
    /// </summary>
    class CombatScreen : Screen
    {
        /// <summary>
        /// combat engine reference
        /// </summary>
        private CombatEngine cs_CombatEngine = CombatEngine.Instance;

        public override void Initialize()
        {
            base.Initialize();
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
