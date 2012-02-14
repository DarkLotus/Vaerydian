using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Vaerydian
{
    class GameContainer
    {

        private ContentManager g_ContentManager;

        public ContentManager ContentManager
        {
            get { return g_ContentManager; }
            set { g_ContentManager = value; }
        }

        private SpriteBatch g_SpriteBatch;

        public SpriteBatch SpriteBatch
        {
            get { return g_SpriteBatch; }
            set { g_SpriteBatch = value; }
        }

        private GraphicsDevice g_GraphicsDevice;

        public GraphicsDevice GraphicsDevice
        {
            get { return g_GraphicsDevice; }
            set { g_GraphicsDevice = value; }
        }


    }
}
