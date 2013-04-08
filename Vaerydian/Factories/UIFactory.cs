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

		public Entity createStatWindow(Entity caller, Point position, Point dimensions, int buttonHeight)
		{
			Entity e = u_EcsInstance.create();

			BasicWindow window = new BasicWindow(e, caller, u_EcsInstance, position, dimensions, buttonHeight);

			//initialize the window
			window.init();

			//setup background frame
			window.Frame.BackgroundColor = Color.Black;
			window.Frame.BackgroundName = "frame";
			window.Frame.Transparency = 0.5f;

			//setup close button
			window.Button.NormalTextureName = "test_dialog";
            window.Button.PressedTextureName = "test_dialog2";
            window.Button.MouseOverTextureName = "test_dialog2";
            window.Button.Color = Color.Gray;
            window.Button.Transparency = 1f;
            window.Button.Border = 0;
            window.Button.FontName = "General";
            window.Button.AutoSize = false;
            window.Button.CenterText = true;
            window.Button.Text = "Close Window";
            window.Button.NormalTextColor = Color.White;
            window.Button.MouseOverTextColor = Color.Yellow;
            window.Button.PressedTextColor = Color.Red;

			//setup window delete
			window.Button.MouseClick += destroyUI;

			//pre-assemble window
			window.preAssemble();

			//add any custom controls here to window.Canvas
			GLabel label = new GLabel();
			label.Owner = e;
			label.Caller = caller;
			label.ECSInstance = u_EcsInstance;
			label.Bounds = new Rectangle(window.Form.Bounds.Left + 20,window.Form.Bounds.Top + 40, dimensions.X - 40,dimensions.Y-60);
			label.AutoSize = false;
			label.Text = "stuffs";
			label.FontName = "General";
			label.Border = 0;
			label.TextColor = Color.White;
			label.BackgroundName = "frame";
			label.BackgroundColor = Color.Black;
			label.BackgroundTransparency = 0.5f;
			label.Updating += labelUpdate;

			//add controls to canvas
			window.Canvas.Controls.Add(label);

			//final assemble
			window.assemble();

			//create the UI component and assign it to the entity
			UserInterface ui = new UserInterface(window.Form);

            u_EcsInstance.ComponentManager.addComponent(e, ui);

            u_EcsInstance.refresh(e);

			return e;
		}

		private void destroyUI(IControl sender, InterfaceArgs args)
		{
			sender.ECSInstance.deleteEntity(sender.Owner);
		}

		private void labelUpdate(IControl sender, InterfaceArgs args)
		{
			Vaerydian.Components.Characters.Skills skills = ComponentMapper.get<Vaerydian.Components.Characters.Skills>(sender.Caller);
			Vaerydian.Components.Characters.Attributes attributes = ComponentMapper.get<Vaerydian.Components.Characters.Attributes>(sender.Caller);

			GLabel label = (GLabel) sender;
			label.Text = "Skills" + "\n" +
					     "  Range: " + skills.SkillSet[Vaerydian.Utils.SkillName.Ranged].Value + "\n" +
						 "  Melee: " + skills.SkillSet[Vaerydian.Utils.SkillName.Melee].Value + "\n" +
					     "  Avoidance: " + skills.SkillSet[Vaerydian.Utils.SkillName.Avoidance].Value + "\n"+
						 "\n" +
						 "Attributes" + "\n" +
						 "  Endurance: " + attributes.AttributeSet[Vaerydian.Components.Characters.AttributeType.Endurance] + "\n" +
						 "  Mind: " + attributes.AttributeSet[Vaerydian.Components.Characters.AttributeType.Mind] + "\n" +
						 "  Muscle: " + attributes.AttributeSet[Vaerydian.Components.Characters.AttributeType.Muscle] + "\n" +
						 "  Perception: " + attributes.AttributeSet[Vaerydian.Components.Characters.AttributeType.Perception] + "\n" +
						 "  Personality: " + attributes.AttributeSet[Vaerydian.Components.Characters.AttributeType.Personality] + "\n" +
						 "  Quickness: " + attributes.AttributeSet[Vaerydian.Components.Characters.AttributeType.Quickness];
		}
    }
}
