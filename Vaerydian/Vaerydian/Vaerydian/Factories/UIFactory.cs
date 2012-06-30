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

        //TODO: replace timed dialog window with a Glimpse structure
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

        public void createUITests()
        {
            Entity e = u_EcsInstance.create();

            //creating the form
            GForm form = new GForm();
            form.Owner = e;
            form.ECSInstance = u_EcsInstance;
            form.Bounds = new Rectangle(100, 100, 50, 25);

            //creating the canvas
            GCanvas canvas = new GCanvas();
            canvas.Owner = e;
            canvas.ECSInstance = u_EcsInstance;
            canvas.Bounds = new Rectangle(100, 100, 50, 25);

            //creating the button
            GButton button = new GButton();
            button.Owner = e;
            button.ECSInstance = u_EcsInstance;
            button.Bounds = new Rectangle(100, 100, 50, 25);
            button.NormalTextureName = "ui\\buttonNormal";
            button.PressedTextureName = "ui\\buttonPressed";
            button.MouseOverTextureName = "ui\\buttonOver";
            button.Key = Keys.D1;

            //creating the text box
            GTextBox textBox = new GTextBox();
            textBox.Owner = e;
            textBox.Bounds = new Rectangle(100, 200, 250, 50);
            textBox.FontName = "General";
            textBox.BackgroundName = "dialog_bubble";
            textBox.Border = 10;
            textBox.Text = "Hello World, How Are You? This is a GTextBox! This is alot of text, but is it enough? I don't know, do you? Either way, the GTextBox took this very long string and auto-sized all this text! Yay!";

            //reference to be used to toggle the text-box on-and-off
            TB = textBox;

            //creating the label
            GLabel label = new GLabel();
            label.Owner = e;
            label.ECSInstance = u_EcsInstance;
            label.Bounds = new Rectangle(100, 150, 100, 50);
            label.FontName = "General";
            label.BackgroundName = "dialog_bubble";
            label.Border = 10;
            label.Text = "This is a GLabel";

            //creating the frame
            GFrame frame = new GFrame();
            frame.Owner = e;
            frame.ECSInstance = u_EcsInstance;
            frame.Bounds = new Rectangle(400, 100, (int)(100 * 1.6), 100);
            frame.BackgroundName = "Title";

            //setup events
            button.KeyToggle += toggleTextBox;
            button.MouseClick += buttonMouseMoveToggle;
            button.Updating += buttonMouseMove;
            frame.MouseClick += frameMouseMoveToggle;
            //frame.Updating += frameMouseMove;
            label.MouseClick += dragMove;
            


            //adding controls to the frame, in draw order (bottom-to-top)
            canvas.Controls.Add(frame);
            canvas.Controls.Add(button);
            canvas.Controls.Add(textBox);
            canvas.Controls.Add(label);

            //assign canvas to form and add it to the UI
            form.CanvasControls.Add(canvas);

            UserInterface ui = new UserInterface(form);

            //assign component and issue refresh
            u_EcsInstance.EntityManager.addComponent(e, ui);
            u_EcsInstance.refresh(e);
        }
        
        //holder for the TB
        private GTextBox TB;

        //toggle the TB
        private void toggleTextBox(IControl sender, InterfaceArgs args)
        {
            if (TB.IsActive)
                TB.IsActive = false;
            else
                TB.IsActive = true;
        }

        //some flags for when i should allow update-moves
        private bool u_mouseMoveToggle = false;
        private bool u_frameMouseMoveToggle = false;

        //toggle button moves
        private void buttonMouseMoveToggle(IControl sender, InterfaceArgs args)
        {
            if (u_mouseMoveToggle)
                u_mouseMoveToggle = false;
            else
                u_mouseMoveToggle = true;
        }

        //toggle frame moves
        private void frameMouseMoveToggle(IControl sender, InterfaceArgs args)
        {
            if (u_frameMouseMoveToggle)
            {
                u_frameMouseMoveToggle = false;
                sender.Updating -= frameMouseMove;
            }
            else
            {
                u_frameMouseMoveToggle = true;
                sender.Updating += frameMouseMove;
            }
        }

        //move the button if you can
        private void buttonMouseMove(IControl sender, InterfaceArgs args)
        {
            if (u_mouseMoveToggle)
            {
                Point p = args.InputStateContainer.CurrentMousePosition;
                sender.Bounds = new Rectangle(p.X, p.Y, sender.Bounds.Width, sender.Bounds.Height);
            }
        }

        //move the frame if you can
        private void frameMouseMove(IControl sender, InterfaceArgs args)
        {
            Point p1 = args.InputStateContainer.PreviousMousePosition;
            Point p2 = args.InputStateContainer.CurrentMousePosition;
            Point c = sender.Bounds.Location;
            Point p;
            p.X = p2.X - p1.X;
            p.Y = p2.Y - p1.Y;
            sender.Bounds = new Rectangle(c.X + p.X, c.Y + p.Y, sender.Bounds.Width, sender.Bounds.Height);
        }

        private void dragMove(IControl sender, InterfaceArgs args)
        {
            Point p1 = args.InputStateContainer.PreviousMousePosition;
            Point p2 = args.InputStateContainer.CurrentMousePosition;
            Point c = sender.Bounds.Location;
            Point p;
            p.X = p2.X - p1.X;
            p.Y = p2.Y - p1.Y;
            sender.Bounds = new Rectangle(c.X + p.X, c.Y + p.Y, sender.Bounds.Width, sender.Bounds.Height);
        }

        

        private void toggleOnDragMove(IControl sender, InterfaceArgs args)
        {
            sender.Updating += dragMove;
        }

        private void toggleOffDragMove(IControl sender, InterfaceArgs args)
        {
            sender.Updating -= dragMove;
        }
    }
}
