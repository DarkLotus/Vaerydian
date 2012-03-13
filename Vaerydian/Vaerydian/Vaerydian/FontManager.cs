using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vaerydian
{
    /// <summary>
    /// this singleton manages and provides easy access to all game fonts
    /// </summary>
    class FontManager
    {
        /// <summary>
        /// private dictionary of fonts
        /// </summary>
        Dictionary<String, SpriteFont> fm_fonts = new Dictionary<string, SpriteFont>();
        /// <summary>
        /// dictionary of fonts
        /// </summary>
        public Dictionary<String, SpriteFont> Fonts { get { return fm_fonts; } set { fm_fonts = value; } }

        /// <summary>
        /// private content manager copy
        /// </summary>
        private ContentManager fm_contentManager;
        /// <summary>
        /// content manager copy
        /// </summary>
        public ContentManager ContentManager { get { return fm_contentManager; } set { fm_contentManager = value; } }

        /// <summary>
        /// private singleton
        /// </summary>
        private FontManager() { }

        /// <summary>
        /// private singleton instance
        /// </summary>
        private static readonly FontManager fm_Instance = new FontManager();

        /// <summary>
        /// singleton access variable
        /// </summary>
        public static FontManager Instance
        {
            get{return fm_Instance;}
        }

        /// <summary>
        /// load all FontManager content
        /// </summary>
        public void LoadContent()
        {
            fm_fonts.Add("General", fm_contentManager.Load<SpriteFont>("General"));
            fm_fonts.Add("Loading", fm_contentManager.Load<SpriteFont>("Loading"));
            fm_fonts.Add("StartScreen", fm_contentManager.Load<SpriteFont>("StartScreen"));
            fm_fonts.Add("Damage", fm_contentManager.Load<SpriteFont>("Damage"));
            fm_fonts.Add("DamageBold", fm_contentManager.Load<SpriteFont>("DamageBold"));
        }

        
    }
}
