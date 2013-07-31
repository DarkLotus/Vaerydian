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

using ECSFramework;

using Microsoft.Xna.Framework;

namespace Vaerydian.Components.Graphical
{
    class FloatingText : IComponent
    {
        private static int f_TypeID;
        private int f_EntityID;

        public FloatingText() { }

        public int getEntityId()
        {
            return f_EntityID;
        }

        public int getTypeId()
        {
            return f_TypeID;
        }

        public void setEntityId(int entityId)
        {
            f_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            f_TypeID = typeId;
        }

        private String f_Text;
        /// <summary>
        /// text to be displayed
        /// </summary>
        public String Text
        {
            get { return f_Text; }
            set { f_Text = value; }
        }

        private String f_FontName;
        /// <summary>
        /// name of the font inthe font manager
        /// </summary>
        public String FontName
        {
            get { return f_FontName; }
            set { f_FontName = value; }
        }
        
        private Color f_Color;
        /// <summary>
        /// color of the text
        /// </summary>
        public Color Color
        {
            get { return f_Color; }
            set { f_Color = value; }
        }

        private int f_Lifetime;
        /// <summary>
        /// total lifetime allowed for floating text
        /// </summary>
        public int Lifetime
        {
            get { return f_Lifetime; }
            set { f_Lifetime = value; }
        }

        private int f_ElapsedTime;
        /// <summary>
        /// total time component has been alive
        /// </summary>
        public int ElapsedTime
        {
            get { return f_ElapsedTime; }
            set { f_ElapsedTime = value; }
        }


    }
}
