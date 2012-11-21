using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECSFramework;


using Vaerydian.Components;
using Vaerydian.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Glimpse.Components;
using Glimpse.Controls;
using Glimpse.Input;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Graphical;

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


        /// <summary>
        /// creates a timed dialog
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="dialog"></param>
        /// <param name="origin"></param>
        /// <param name="name"></param>
        /// <param name="duration"></param>
        public void createTimedDialogWindow(Entity caller, String dialog, Vector2 origin, String name, int duration)
        {
            Entity e = u_EcsInstance.create();

            DialogTimer timer = new DialogTimer(duration,u_EcsInstance);

            GForm form = new GForm();
            form.Caller = caller;
            form.Owner = e;
            form.ECSInstance = u_EcsInstance;
            form.Bounds = new Rectangle((int)origin.X, (int)origin.Y, 100, 50);

            GCanvas canvas = new GCanvas();
            canvas.Caller = caller;
            canvas.Owner = e;
            canvas.ECSInstance = u_EcsInstance;
            canvas.Bounds = new Rectangle((int)origin.X, (int)origin.Y, 200, 200);

            GTextBox textBox = new GTextBox();
            textBox.Caller = caller;
            textBox.Owner = e;
            textBox.ECSInstance = u_EcsInstance;
            textBox.Bounds = new Rectangle((int)origin.X, (int)origin.Y, 100, 10);
            textBox.FontName = "StartScreen";
            textBox.BackgroundName = "dialog_bubble";
            textBox.Border = 10;
            textBox.Text = dialog;
            textBox.TextColor = Color.White;
            textBox.CenterText = true;
            textBox.BackgroundColor = Color.Black;
            textBox.BackgroundTransparency = 0.75f;

            textBox.Updating += timer.updateHandler;

            canvas.Controls.Add(textBox);

            form.CanvasControls.Add(canvas);

            UserInterface ui = new UserInterface(form);

            //assign component and issue refresh
            u_EcsInstance.EntityManager.addComponent(e, ui);
            u_EcsInstance.refresh(e);
        }

        public void createFrame(Entity caller, Point Position, int height, int width, string textureName)
        {
            Entity e = u_EcsInstance.create();

            GForm form = new GForm();
            form.Caller = caller;
            form.Owner = e;
            form.ECSInstance = u_EcsInstance;
            form.Bounds = new Rectangle(Position.X, Position.Y, width, height);

            GCanvas canvas = new GCanvas();
            canvas.Caller = caller;
            canvas.Owner = e;
            canvas.ECSInstance = u_EcsInstance;
            canvas.Bounds = new Rectangle(Position.X, Position.Y, width, height);

            GFrame frame = new GFrame();
            frame.Caller = caller;
            frame.Owner = e;
            frame.ECSInstance = u_EcsInstance;
            frame.Bounds = new Rectangle(Position.X, Position.Y, width, height);
            frame.BackgroundName = textureName;

            canvas.Controls.Add(frame);

            form.CanvasControls.Add(canvas);

            UserInterface ui = new UserInterface(form);

            u_EcsInstance.ComponentManager.addComponent(e, ui);

            u_EcsInstance.refresh(e);
        }

        public GFrame createMousePointer(Point position, int width, int height, string textureName, InterfaceHandler handler)
        {
            Entity e = u_EcsInstance.create();

            GForm form = new GForm();
            form.Owner = e;
            form.ECSInstance = u_EcsInstance;
            form.Bounds = new Rectangle(position.X, position.Y, width, height);

            GCanvas canvas = new GCanvas();
            canvas.Owner = e;
            canvas.ECSInstance = u_EcsInstance;
            canvas.Bounds = new Rectangle(position.X, position.Y, width, height);

            GFrame frame = new GFrame();
            frame.Owner = e;
            frame.ECSInstance = u_EcsInstance;
            frame.Bounds = new Rectangle(position.X, position.Y, width, height);
            frame.BackgroundName = textureName;

            frame.Updating += handler;

            canvas.Controls.Add(frame);

            form.CanvasControls.Add(canvas);

            UserInterface ui = new UserInterface(form);

            u_EcsInstance.ComponentManager.addComponent(e, ui);

            u_EcsInstance.refresh(e);

            return frame;
        }

        public void createFloatingText(string text, string font, Color color, int timeToLive, Position position)
        {
            Entity e = u_EcsInstance.create();

            FloatingText fText = new FloatingText();
            fText.Text = text;
            fText.FontName = font;
            fText.Color = color;
            fText.Lifetime = timeToLive;

            u_EcsInstance.EntityManager.addComponent(e, fText);
            u_EcsInstance.EntityManager.addComponent(e, position);

            u_EcsInstance.refresh(e);
        }

		public void createHitPointLabel(Entity entity, int width, int height, Point position)
		{
			Entity e = u_EcsInstance.create();

			HpLabelUpdater updater = new HpLabelUpdater(u_EcsInstance);

			GForm form = new GForm();
            form.Owner = e;
            form.ECSInstance = u_EcsInstance;
            form.Bounds = new Rectangle(position.X, position.Y, width, height);

            GCanvas canvas = new GCanvas();
            canvas.Owner = e;
            canvas.ECSInstance = u_EcsInstance;
            canvas.Bounds = new Rectangle(position.X, position.Y, width, height);

			GLabel label = new GLabel();
			label.Owner = e;
			label.Caller = entity;
			label.ECSInstance = u_EcsInstance;
			label.Bounds = new Rectangle(position.X, position.Y, width, height);
			label.FontName = "StartScreen";
			label.Border = 10;
			label.BackgroundName = "dialog_bubble";
			label.BackgroundColor = Color.Black;
			label.BackgroundTransparency = 0.5f;
			label.CenterText = true;
			label.Text = "XXX / XXX";
			label.TextColor = Color.Red;
			label.Updating += updater.updateHandler;


			canvas.Controls.Add(label);
			form.CanvasControls.Add(canvas);

			UserInterface ui = new UserInterface(form);

            u_EcsInstance.ComponentManager.addComponent(e, ui);

            u_EcsInstance.refresh(e);
		}
    }
}
