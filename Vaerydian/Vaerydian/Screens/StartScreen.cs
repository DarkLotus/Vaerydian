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

            s_ButtonMenu.Buttons[0].NormalTextureName = "ui\\buttonNormal";
            s_ButtonMenu.Buttons[0].PressedTextureName = "ui\\buttonPressed";
            s_ButtonMenu.Buttons[0].MouseOverTextureName = "ui\\buttonOver";
            s_ButtonMenu.Buttons[0].MouseClick += OnMouseClickNewGame;

            s_ButtonMenu.Buttons[1].NormalTextureName = "ui\\buttonNormal";
            s_ButtonMenu.Buttons[1].PressedTextureName = "ui\\buttonPressed";
            s_ButtonMenu.Buttons[1].MouseOverTextureName = "ui\\buttonOver";
            s_ButtonMenu.Buttons[1].MouseClick += OnMouseClickWorldGen;

            s_ButtonMenu.Buttons[2].NormalTextureName = "ui\\buttonNormal";
            s_ButtonMenu.Buttons[2].PressedTextureName = "ui\\buttonPressed";
            s_ButtonMenu.Buttons[2].MouseOverTextureName = "ui\\buttonOver";
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
            
            //cleanup button events
            s_ButtonMenu.Buttons[0].ResetEvents();
            s_ButtonMenu.Buttons[1].ResetEvents();
            s_ButtonMenu.Buttons[2].ResetEvents();

            //cleanup frame event
            s_Frame.ResetEvents();

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

            //load the world screen
            NewLoadingScreen.Load(this.ScreenManager, true, new GameScreen());
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
