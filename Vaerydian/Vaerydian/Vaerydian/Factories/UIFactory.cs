using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECSFramework;


using Vaerydian.Components;
using Vaerydian.UI;
using Vaerydian.UI.implemented;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Glimpse.Components;
using Glimpse.Controls;
using Glimpse.Input;

namespace Vaerydian.Factories
{
    public class UIFactory
    {
        private ECSInstance u_EcsInstance;
        private static GameContainer u_Container;
        private Random rand = new Random();

        public UIFactory(ECSInstance ecsInstance, GameContainer container)
        {
            u_EcsInstance = ecsInstance;
            u_Container = container;
        }

        public UIFactory(ECSInstance ecsInstance) 
        {
            u_EcsInstance = ecsInstance;
        }

        public void createTimedDialogWindow(Entity caller, String dialog, Vector2 origin, String name, int duration)
        {
            Entity e = u_EcsInstance.create();

            TimedDialog window = new TimedDialog(caller, dialog, origin, name, duration);

            window.ECSInstance = u_EcsInstance;
            window.Owner = e;

            UserInterface ui = new UserInterface(window);

            u_EcsInstance.EntityManager.addComponent(e, ui);

            u_EcsInstance.refresh(e);
        }

        public void createButton()
        {
            Entity e = u_EcsInstance.create();

            GForm form = new GForm();
            form.Owner = e;
            form.ECSInstance = u_EcsInstance;
            form.Bounds = new Rectangle(100, 100, 50, 25);

            GCanvas canvas = new GCanvas();
            canvas.Owner = e;
            canvas.ECSInstance = u_EcsInstance;
            canvas.Bounds = new Rectangle(100, 100, 50, 25);

            GButton button = new GButton();
            button.Owner = e;
            button.ECSInstance = u_EcsInstance;
            button.Bounds = new Rectangle(100, 100, 50, 25);
            button.NormalTextureName = "ui\\buttonNormal";
            button.PressedTextureName = "ui\\buttonPressed";
            button.MouseOverTextureName = "ui\\buttonOver";

            GTextBox textBox = new GTextBox();
            textBox.Owner = e;
            textBox.Bounds = new Rectangle(100, 200, 200, 50);
            textBox.FontName = "General";
            textBox.BackgroundName = "dialog_bubble";
            textBox.Border = 10;
            textBox.Text = "Hello World, How Are You? This is a GTextBox! This is alot of text, but is it enough? I don't know, do you?";

            TB = textBox;

            GLabel label = new GLabel();
            label.Owner = e;
            label.ECSInstance = u_EcsInstance;
            label.Bounds = new Rectangle(100, 150, 100, 50);
            label.FontName = "General";
            label.BackgroundName = "dialog_bubble";
            label.Border = 10;
            label.Text = "This is a GLabel";

            GFrame frame = new GFrame();
            frame.Owner = e;
            frame.ECSInstance = u_EcsInstance;
            frame.Bounds = new Rectangle(400, 100, (int)(100 * 1.6), 100);
            frame.BackgroundName = "Title";

            UtilFactory uf = new UtilFactory(u_EcsInstance);

            button.Key = Keys.D1;

            button.KeyToggle += toggleTextBox;
            //button.MouseClick += uf.createFireSound;
            button.MouseClick += mouseMoveToggle;
            button.Updating += mouseMove;

            canvas.Controls.Add(frame);
            canvas.Controls.Add(button);
            canvas.Controls.Add(textBox);
            canvas.Controls.Add(label);

            form.CanvasControls.Add(canvas);

            UserInterface ui = new UserInterface(form);

            u_EcsInstance.EntityManager.addComponent(e, ui);

            u_EcsInstance.refresh(e);
        }

        private GTextBox TB;

        private void toggleTextBox(IControl sender, InterfaceArgs args)
        {
            if (TB.IsActive)
                TB.IsActive = false;
            else
                TB.IsActive = true;
        }

        private bool u_mouseMoveToggle = false;

        private void mouseMoveToggle(IControl sender, InterfaceArgs args)
        {
            if (u_mouseMoveToggle)
                u_mouseMoveToggle = false;
            else
                u_mouseMoveToggle = true;
        }

        private void mouseMove(IControl sender, InterfaceArgs args)
        {
            if (u_mouseMoveToggle)
            {
                Point p = args.InputStateContainer.CurrentMousePosition;

                GButton button = (GButton)sender;

                button.Bounds = new Rectangle(p.X,p.Y, button.Bounds.Width, button.Bounds.Height);
            }
        }




    }
}
