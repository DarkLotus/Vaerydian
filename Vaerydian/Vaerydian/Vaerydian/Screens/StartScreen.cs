using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Vaerydian.Windows;
using Vaerydian.Sessions;

using Glimpse.Input;
using Glimpse.Managers;

namespace Vaerydian.Screens
{
    class StartScreen : Screen
    {

        private TextMenuWindow s_TextMenu;

        private List<String> s_MenuItems = new List<String>();

        private Texture2D s_TitleTexture;

        private SpriteBatch s_SpriteBatch;

        private Rectangle s_BackgroundRect;

        private float s_yTextOffset;

        public override void Initialize()
        {
            base.Initialize();

            //create the menu items
            s_MenuItems.Add("New Game");//0
            s_MenuItems.Add("Load Game");//1
            s_MenuItems.Add("Options");//2
            s_MenuItems.Add("Exit");//3
            s_MenuItems.Add("Instructions");//4
            s_MenuItems.Add("World Test");//5
            //create the menu
            s_TextMenu = new TextMenuWindow(new Vector2(this.ScreenManager.GraphicsDevice.Viewport.Width / 2, this.ScreenManager.GraphicsDevice.Viewport.Height / 2), s_MenuItems, "StartScreen");
            //register the menu
            this.ScreenManager.WindowManager.addWindow(s_TextMenu);

            s_BackgroundRect = new Rectangle(0, 0, this.ScreenManager.GraphicsDevice.Viewport.Width, this.ScreenManager.GraphicsDevice.Viewport.Height);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            //load title texture
            s_TitleTexture = this.ScreenManager.Game.Content.Load<Texture2D>("Title");
            s_yTextOffset = FontManager.Fonts["StartScreen"].LineSpacing;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();


        }

        public override void hasFocusUpdate(GameTime gameTime)
        {
            base.hasFocusUpdate(gameTime);

            if(InputManager.isKeyToggled(Keys.Up))//move selection up
            {
                if (s_TextMenu.MenuIndex > 0)
                {
                    s_TextMenu.MenuIndex -= 1;
                }
            }
            else if(InputManager.isKeyToggled(Keys.Down))//move selection down
            {
                if (s_TextMenu.MenuIndex < s_MenuItems.Count - 1)
                {
                    s_TextMenu.MenuIndex += 1;
                }
            }
            else if (InputManager.isKeyToggled(Keys.Enter))
            {
                //perform the selected action
                if (s_TextMenu.MenuIndex == 0)//player selected to start a new game
                {
                    //dispose of this screen
                    this.ScreenManager.removeScreen(this);
                    //dispose of the menu
                    this.ScreenManager.WindowManager.removeWindow(s_TextMenu);
                    //load the world screen
                    LoadingScreen.Load(this.ScreenManager, true, new GameScreen());
                }
                else if (s_TextMenu.MenuIndex == 1)//player wants to load a game
                {
                }
                else if (s_TextMenu.MenuIndex == 2)//player wants to goto options menu
                {
                }
                else if (s_TextMenu.MenuIndex == 3)//player wants to quit
                {
                    //tell the input manager that the player wants to quit
                    InputManager.YesExit = true;
                }
                else if (s_TextMenu.MenuIndex == 4)
                {
                }
                else if (s_TextMenu.MenuIndex == 5)//test map
                {
                    //dispose of this screen
                    this.ScreenManager.removeScreen(this);
                    //dispose of the menu
                    this.ScreenManager.WindowManager.removeWindow(s_TextMenu);
                    //load the world screen
                    LoadingScreen.Load(this.ScreenManager, true, new WorldScreen());
                }

            }else if(InputManager.isKeyToggled(Keys.Escape))
            {
                InputManager.YesExit = true;
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            s_SpriteBatch = this.ScreenManager.SpriteBatch;

            s_SpriteBatch.Begin();

            //display title texture
            s_SpriteBatch.Draw(s_TitleTexture, s_BackgroundRect, Color.White);

            s_SpriteBatch.DrawString(FontManager.Fonts["General"], GameSession.GameVersion, new Vector2(0, this.ScreenManager.GraphicsDevice.Viewport.Height - s_yTextOffset), Color.White);

            s_SpriteBatch.End();
        }

        
    }
}
