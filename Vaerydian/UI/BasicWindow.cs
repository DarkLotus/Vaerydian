/*
 Author:
      Thomas H. Jonell <@Net_Gnome>
 
 Copyright (c) 2013 Thomas H. Jonell

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Lesser General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
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

		/// <summary>
		/// creates a base window UI control
		/// </summary>
		/// <param name='owner'>
		/// Owning entity
		/// </param>
		/// <param name='caller'>
		/// Calling entity
		/// </param>
		/// <param name='ecsInstance'>
		/// ECSInstance entities are from
		/// </param>
		/// <param name='position'>
		/// Position to display the window
		/// </param>
		/// <param name='dimensions'>
		/// Dimensions of the window
		/// </param>
		/// <param name='buttonHeight'>
		/// Height of the top button
		/// </param>
		public BasicWindow (Entity owner, Entity caller, ECSInstance ecsInstance, Point position, Point dimensions, int buttonHeight)
		{
			b_Owner = owner;
			b_Caller = caller;
			b_ECSInstance = ecsInstance;
			b_Position = position;
			b_Dimensions = dimensions;
			b_ButtonHeight = buttonHeight;
		}

		/// <summary>
		/// underlying form of base window
		/// </summary>
		/// <value>
		/// GForm value
		/// </value>
		public GForm Form {
			get {
				return b_Form;
			}
			set {
				b_Form = value;
			}
		}

		/// <summary>
		/// underlying canvas of base window
		/// </summary>
		/// <value>
		/// GCanvas value
		/// </value>
		public GCanvas Canvas {
			get {
				return b_Canvas;
			}
			set {
				b_Canvas = value;
			}
		}

		/// <summary>
		/// underlying frame of base window
		/// </summary>
		/// <value>
		/// GFrame value
		/// </value>
		public GFrame Frame {
			get {
				return b_Frame;
			}
			set {
				b_Frame = value;
			}
		}

		/// <summary>
		/// underlying button of base window
		/// </summary>
		/// <value>
		/// GButton value
		/// </value>
		public GButton Button {
			get {
				return b_Button;
			}
			set {
				b_Button = value;
			}
		}

		/// <summary>
		/// Initialize the base controls
		/// </summary>
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

			b_Frame = new GFrame();
			b_Frame.Owner = b_Owner;
			b_Frame.Caller = b_Caller;
			b_Frame.ECSInstance = b_ECSInstance;
			b_Frame.Bounds = new Rectangle(b_Position.X,b_Position.Y,b_Dimensions.X,b_Dimensions.Y);

			b_Button = new GButton();
			b_Button.Owner = b_Owner;
			b_Button.Caller = b_Caller;
			b_Button.ECSInstance = b_ECSInstance;
			b_Button.Bounds = new Rectangle(b_Position.X,b_Position.Y,b_Dimensions.X,b_ButtonHeight);
		}

		/// <summary>
		/// Pre-assemble the controls (base frame and button)
		/// </summary>
		public void preAssemble ()
		{
			b_Canvas.Controls.Add(b_Frame);
			b_Canvas.Controls.Add(b_Button);
		}

		/// <summary>
		/// Assemble into the form.
		/// </summary>
		public void assemble()
		{
			b_Form.CanvasControls.Add(b_Canvas);
		}


	}
}

