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

namespace Vaerydian.Screens
{
    class StartScreen : Screen
    {

        private TextMenuWindow ss_TextMenu;

        private List<String> ss_MenuItems = new List<String>();

        private Texture2D ss_TitleTexture;

        private SpriteBatch ss_SpriteBatch;

        public override void Initialize()
        {
            base.Initialize();

            //create the menu items
            ss_MenuItems.Add("New Game");//0
            ss_MenuItems.Add("Load Game");//1
            ss_MenuItems.Add("Options");//2
            ss_MenuItems.Add("Exit");//3
            ss_MenuItems.Add("Instructions");//4
            ss_MenuItems.Add("Combat Test");//5
            ss_MenuItems.Add("Cave Test");//6
            //create the menu
            ss_TextMenu = new TextMenuWindow(new Point(400, 300), ss_MenuItems, "StartScreen");
            //register the menu
            this.ScreenManager.WindowManager.addWindow(ss_TextMenu);

        }

        public override void LoadContent()
        {
            base.LoadContent();

            //load title texture
            ss_TitleTexture = this.ScreenManager.Game.Content.Load<Texture2D>("Title");

        }

        public override void UnloadContent()
        {
            base.UnloadContent();


        }

        public override void handleInput(GameTime gameTime)
        {
            base.handleInput(gameTime);

            if(InputManager.isKeyToggled(Keys.Up))//move selection up
            {
                if (ss_TextMenu.MenuIndex > 0)
                {
                    ss_TextMenu.MenuIndex -= 1;
                }
            }
            else if(InputManager.isKeyToggled(Keys.Down))//move selection down
            {
                if (ss_TextMenu.MenuIndex < ss_MenuItems.Count - 1)
                {
                    ss_TextMenu.MenuIndex += 1;
                }
            }
            else if (InputManager.isKeyToggled(Keys.Enter))
            {
                //perform the selected action
                if (ss_TextMenu.MenuIndex == 0)//player selected to start a new game
                {
                    //dispose of this screen
                    this.ScreenManager.removeScreen(this);
                    //dispose of the menu
                    this.ScreenManager.WindowManager.removeWindow(ss_TextMenu);
                    //load the world screen
                    LoadingScreen.Load(this.ScreenManager, true, new GameScreen());
                }
                else if (ss_TextMenu.MenuIndex == 1)//player wants to load a game
                {
                }
                else if (ss_TextMenu.MenuIndex == 2)//player wants to goto options menu
                {
                }
                else if (ss_TextMenu.MenuIndex == 3)//player wants to quit
                {
                    //tell the input manager that the player wants to quit
                    InputManager.YesExit = true;
                }
                else if (ss_TextMenu.MenuIndex == 4)
                {
                }
                else if (ss_TextMenu.MenuIndex == 5)//test combat
                {
                    //dispose of this screen
                    this.ScreenManager.removeScreen(this);
                    //dispose of the menu
                    this.ScreenManager.WindowManager.removeWindow(ss_TextMenu);
                    //load the world screen
                    LoadingScreen.Load(this.ScreenManager, true, new CombatScreen());
                }
                else if (ss_TextMenu.MenuIndex == 6)
                {
                    //dispose of this screen
                    this.ScreenManager.removeScreen(this);
                    //dispose of the menu
                    this.ScreenManager.WindowManager.removeWindow(ss_TextMenu);
                    //load the cave screen
                    LoadingScreen.Load(this.ScreenManager, true, new CaveScreen());
                }

            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ss_SpriteBatch = this.ScreenManager.SpriteBatch;

            ss_SpriteBatch.Begin();

            //display title texture
            ss_SpriteBatch.Draw(ss_TitleTexture, Vector2.Zero, Color.White);

            ss_SpriteBatch.DrawString(FontManager.Instance.Fonts["General"], GameSession.Instance.GameVersion, Vector2.Zero, Color.White);

            ss_SpriteBatch.End();
        }

        
    }
}
