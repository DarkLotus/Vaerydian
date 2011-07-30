using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Vaerydian.Windows;

namespace Vaerydian.Screens
{
    class StartScreen : Screen
    {

        private TextMenuWindow ss_TextMenu;

        private List<String> ss_MenuItems = new List<String>();


        public override void Initialize()
        {
            base.Initialize();

            ss_MenuItems.Add("New Game");
            ss_MenuItems.Add("Load Game");
            ss_MenuItems.Add("Options");
            ss_MenuItems.Add("Exit");

            ss_TextMenu = new TextMenuWindow(new Point(450, 250), new Point(100, 100), ss_MenuItems);

            this.ScreenManager.WindowManager.addWindow(ss_TextMenu);

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
                    LoadingScreen.Load(this.ScreenManager, true, new WorldScreen());
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
                    InputManager.yesExit = true;
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
        }

        
    }
}
