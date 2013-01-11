using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Microsoft.Xna.Framework;

using Vaerydian.Factories;
using Vaerydian.UI;

using Glimpse.Controls;
using Glimpse.Components;
using Glimpse.Systems;
using Glimpse.Input;
using Microsoft.Xna.Framework.Input;
using WorldGeneration.Utils;

namespace Vaerydian.Screens
{
    class StartScreen : Screen
    {
        private ECSInstance s_ECSInstance;
        private GameContainer s_Container;

        private EntitySystem s_UiUpdateSystem;
        private EntitySystem s_UiDrawSystem;

        private ButtonMenu s_ButtonMenu;
        private GFrame s_Frame;

        private UIFactory s_UIFactory;

        public StartScreen() { }

        public override void Initialize()
        {
            base.Initialize();

            //setup instnace and container
            s_ECSInstance = new ECSInstance();
            s_Container = ScreenManager.GameContainer;

            //define and init systems
            s_UiUpdateSystem = s_ECSInstance.SystemManager.setSystem(new UIUpdateSystem(), new UserInterface());
            s_UiDrawSystem = s_ECSInstance.SystemManager.setSystem(new UIDrawSystem(s_Container.ContentManager, s_Container.GraphicsDevice), new UserInterface());
            s_ECSInstance.SystemManager.initializeSystems();

            //setup factory
            s_UIFactory = new UIFactory(s_ECSInstance, s_Container);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            //create backdrop
            s_UIFactory.createFrame(null, new Point(0, 0), s_Container.GraphicsDevice.Viewport.Height, s_Container.GraphicsDevice.Viewport.Width, "Title");

            
            //create menu
            Entity e = s_ECSInstance.create();

            int border = 10;
            int spacing = 5;
            int height = 50;
            int width = 100;
            Point screen = new Point(s_Container.GraphicsDevice.Viewport.Width,s_Container.GraphicsDevice.Viewport.Height);
            Point location = new Point(screen.X / 2 - (width + 2 * border) / 2, screen.Y / 2);

            s_ButtonMenu = new ButtonMenu(e, null, s_ECSInstance, 3, location, height, width, border, spacing);


            s_ButtonMenu.init();

            s_ButtonMenu.Frame.BackgroundName = "frame";
            s_ButtonMenu.Frame.BackgroundColor = Color.Black;
            s_ButtonMenu.Frame.Transparency = 0.75f;            
           
            s_ButtonMenu.Buttons[0].NormalTextureName = "test_dialog";
            s_ButtonMenu.Buttons[0].PressedTextureName = "test_dialog2";
            s_ButtonMenu.Buttons[0].MouseOverTextureName = "test_dialog2";
            s_ButtonMenu.Buttons[0].Color = Color.Gray;
            s_ButtonMenu.Buttons[0].Transparency = 1f; //0.75f;
            s_ButtonMenu.Buttons[0].Border = 10;
            s_ButtonMenu.Buttons[0].FontName = "General";
            s_ButtonMenu.Buttons[0].AutoSize = true;
            s_ButtonMenu.Buttons[0].CenterText = true;
            s_ButtonMenu.Buttons[0].Text = "New Game";
            s_ButtonMenu.Buttons[0].NormalTextColor = Color.White;
            s_ButtonMenu.Buttons[0].MouseOverTextColor = Color.Yellow;
            s_ButtonMenu.Buttons[0].PressedTextColor = Color.Red;
            s_ButtonMenu.Buttons[0].MouseClick += OnMouseClickNewGame;

            s_ButtonMenu.Buttons[1].NormalTextureName = "test_dialog";
            s_ButtonMenu.Buttons[1].PressedTextureName = "test_dialog2";
            s_ButtonMenu.Buttons[1].MouseOverTextureName = "test_dialog2";
            s_ButtonMenu.Buttons[1].Color = Color.Gray;
            s_ButtonMenu.Buttons[1].Transparency = 1f;
            s_ButtonMenu.Buttons[1].Border = 10;
            s_ButtonMenu.Buttons[1].FontName = "General";
            s_ButtonMenu.Buttons[1].AutoSize = true;
            s_ButtonMenu.Buttons[1].CenterText = true;
            s_ButtonMenu.Buttons[1].Text = "World Gen";
            s_ButtonMenu.Buttons[1].NormalTextColor = Color.White;
            s_ButtonMenu.Buttons[1].MouseOverTextColor = Color.Yellow;
            s_ButtonMenu.Buttons[1].PressedTextColor = Color.Red;
            s_ButtonMenu.Buttons[1].MouseClick += OnMouseClickWorldGen;

            s_ButtonMenu.Buttons[2].NormalTextureName = "test_dialog";
            s_ButtonMenu.Buttons[2].PressedTextureName = "test_dialog2";
            s_ButtonMenu.Buttons[2].MouseOverTextureName = "test_dialog2";
            s_ButtonMenu.Buttons[2].Color = Color.Gray;
            s_ButtonMenu.Buttons[2].Transparency = 1f;
            s_ButtonMenu.Buttons[2].Border = 10;
            s_ButtonMenu.Buttons[2].FontName = "General";
            s_ButtonMenu.Buttons[2].AutoSize = true;
            s_ButtonMenu.Buttons[2].CenterText = true;
            s_ButtonMenu.Buttons[2].Text = "Exit Game";
            s_ButtonMenu.Buttons[2].NormalTextColor = Color.White;
            s_ButtonMenu.Buttons[2].MouseOverTextColor = Color.Yellow;
            s_ButtonMenu.Buttons[2].PressedTextColor = Color.Red;
            s_ButtonMenu.Buttons[2].MouseClick += OnMouseClickExit;
            


            s_ButtonMenu.assemble();

            UserInterface ui = new UserInterface(s_ButtonMenu.Form);

            s_ECSInstance.ComponentManager.addComponent(e, ui);

            s_ECSInstance.refresh(e);

            //create mouse pointer
            s_Frame = s_UIFactory.createMousePointer(InputManager.getMousePositionPoint(), 10, 10, "pointer", OnMousePointerUpdate);

            //early entity reslove
            s_ECSInstance.resolveEntities();

            //load system content
            s_ECSInstance.SystemManager.systemsLoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            s_ECSInstance.cleanUp();

            GC.Collect();
        }

        public override void hasFocusUpdate(GameTime gameTime)
        {
            base.hasFocusUpdate(gameTime);

            //check to see if escape was recently pressed
            if (InputManager.isKeyToggled(Keys.Escape))
            {
                InputManager.YesExit = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            s_UiUpdateSystem.process();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            s_UiDrawSystem.process();
        }

        private void OnMouseClickNewGame(IControl control, InterfaceArgs args) 
        {
            //dispose of this screen
            this.ScreenManager.removeScreen(this);

            //setup new game parameters
            object[] parameters = new object[GameScreen.GAMESCREEN_PARAM_SIZE];
            parameters[GameScreen.GAMESCREEN_SEED] = 4;
            parameters[GameScreen.GAMESCREEN_SKILLLEVEL] = 10;
            parameters[GameScreen.GAMESCREEN_RETURNING] = false;
            parameters[GameScreen.GAMESCREEN_LAST_PLAYER_POSITION] = null;
            
            //load the world screen
            NewLoadingScreen.Load(this.ScreenManager, false, new GameScreen(true,MapType.CAVE,parameters));
        }

        private void OnMouseClickWorldGen(IControl control, InterfaceArgs args)
        {
            //dispose of this screen
            this.ScreenManager.removeScreen(this);


            //load the world screen
            //LoadingScreen.Load(this.ScreenManager, true, new WorldScreen());
			NewLoadingScreen.Load(this.ScreenManager,true,new WorldScreen());
        }

        private void OnMouseClickExit(IControl control, InterfaceArgs args)
        {
            //tell the input manager that the player wants to quit
            InputManager.YesExit = true;
        }

        private void OnMousePointerUpdate(IControl control, InterfaceArgs args)
        {
            control.Bounds = new Rectangle(args.InputStateContainer.CurrentMousePosition.X, args.InputStateContainer.CurrentMousePosition.Y, 10, 10);
        }

    }
}
