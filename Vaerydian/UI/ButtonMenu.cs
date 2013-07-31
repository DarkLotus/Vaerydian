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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Glimpse.Controls;

using ECSFramework;

using Microsoft.Xna.Framework;

namespace Vaerydian.UI
{
    class ButtonMenu
    {
        private Entity b_Owner;
        private Entity b_Caller;
        private ECSInstance b_ECSInstance;
        private List<GButton> b_Buttons = new List<GButton>();
        private int b_ButtonCount;
        private int b_border;
        private int b_Height;
        private int b_Width;
        private int b_Spacing;
        private Point b_Position;
        private GForm b_Form = new GForm();
        private GCanvas b_Canvas = new GCanvas();
        private GFrame b_Frame = new GFrame();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="caller"></param>
        /// <param name="ecsInstance"></param>
        /// <param name="buttonCount"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="border"></param>
        public ButtonMenu(Entity owner, Entity caller, ECSInstance ecsInstance, int buttonCount, Point position, int height, int width, int border, int spacing)
        {
            b_Owner = owner;
            b_Caller = caller;
            b_ECSInstance = ecsInstance;
            b_ButtonCount = buttonCount;
            b_Position = position;
            b_Height = height;
            b_Width = width;
            b_border = border;
            b_Spacing = spacing;
        }

        /// <summary>
        /// Buttons in the menu
        /// </summary>
        public List<GButton> Buttons
        {
            get { return b_Buttons; }
            set { b_Buttons = value; }
        }

        /// <summary>
        /// form containing controlls
        /// </summary>
        public GForm Form
        {
            get { return b_Form; }
            set { b_Form = value; }
        }

        /// <summary>
        /// frame background
        /// </summary>
        public GFrame Frame
        {
            get { return b_Frame; }
            set { b_Frame = value; }
        }
        
        /// <summary>
        /// initialize menu
        /// </summary>
        public void init()
        {
            //setup form
            b_Form = new GForm();
            b_Form.Owner = b_Owner;
            b_Form.Caller = b_Caller;
            b_Form.ECSInstance = b_ECSInstance;
            b_Form.Bounds = new Rectangle(b_Position.X, b_Position.Y, b_Width + 2 * b_border, (b_Height + b_Spacing) * b_ButtonCount + 2 * b_border);

            //setup canvas
            b_Canvas = new GCanvas();
            b_Canvas.Owner = b_Owner;
            b_Canvas.Caller = b_Caller;
            b_Canvas.ECSInstance = b_ECSInstance;
            b_Canvas.Bounds = new Rectangle(b_Position.X, b_Position.Y, b_Width + 2 * b_border, (b_Height + b_Spacing) * b_ButtonCount + 2 * b_border);

            //setup frame
            b_Frame = new GFrame();
            b_Frame.Owner = b_Owner;
            b_Frame.Caller = b_Caller;
            b_Frame.ECSInstance = b_ECSInstance;
            b_Frame.Bounds = new Rectangle(b_Position.X, b_Position.Y, b_Width + 2 * b_border, (b_Height + b_Spacing) * b_ButtonCount + 2 * b_border);

            //b_Canvas.Controls.Add(b_Frame);

            for (int i = 0; i < b_ButtonCount; i++)
            {
                GButton button = new GButton();
                button.Owner = b_Owner;
                button.Caller = b_Caller;
                button.ECSInstance = b_ECSInstance;
                button.Bounds = new Rectangle(b_Position.X + b_border, b_Position.Y + b_border + i * (b_Height + b_Spacing), b_Width, b_Height);
				
                //b_Canvas.Controls.Add(button);
                b_Buttons.Add(button);
            }
            
            //b_Form.CanvasControls.Add(b_Canvas);
        }

        public void assemble()
        {
            b_Canvas.Controls.Add(b_Frame);

            for (int i = 0; i < b_Buttons.Count; i++)
            {
                b_Canvas.Controls.Add(b_Buttons[i]);
            }

            b_Form.CanvasControls.Add(b_Canvas);
        }


    }
}
