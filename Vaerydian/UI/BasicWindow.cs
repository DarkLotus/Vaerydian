using System;

using ECSFramework;

using Glimpse.Controls;
using Microsoft.Xna.Framework;

namespace Vaerydian
{
	public class BasicWindow
	{
		private GForm b_Form;
		private GCanvas b_Canvas;
		private GFrame b_Frame;
		private GButton b_Button;
		private Entity b_Owner;
		private Entity b_Caller;
		private ECSInstance b_ECSInstance;
		private Point b_Position;
		private Point b_Dimensions;
		private int b_ButtonHeight = 10;
		
		public BasicWindow (Entity owner, Entity caller, ECSInstance ecsInstance, Point position, Point dimensions, int buttonWidth	)
		{
			b_Owner = owner;
			b_Caller = caller;
			b_ECSInstance = ecsInstance;
			b_Position = position;
			b_Dimensions = dimensions;
			b_ButtonHeight = buttonWidth;
		}
		
		
		
		public void init()
		{
			b_Form = new GForm();
			b_Form.Owner = b_Owner;
			b_Form.Caller = b_Caller;
			b_Form.ECSInstance = b_ECSInstance;
			b_Form.Bounds = new Rectangle(b_Position.X,b_Position.Y,b_Dimensions.X,b_Dimensions.Y);			
			
			b_Canvas = new GCanvas();
			b_Canvas.Owner = b_Owner;
			b_Canvas.Caller = b_Caller;
			b_Canvas.ECSInstance = b_ECSInstance;
			b_Canvas.Bounds = new Rectangle(b_Position.X,b_Position.Y,b_Dimensions.X,b_Dimensions.Y);
			
			b_Button = new GButton();
			b_Button.Owner = b_Owner;
			b_Button.Caller = b_Caller;
			b_Button.ECSInstance = b_ECSInstance;
			b_Button.Bounds = new Rectangle(b_Position.X,b_Position.Y,b_Dimensions.X,b_ButtonHeight);
		}
		
		public void assemble()
		{
		}
	}
}

