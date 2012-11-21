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
