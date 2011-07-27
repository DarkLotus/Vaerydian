using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaerydian.Triggers;

namespace Vaerydian.Maps
{
    public class Tile
    {

        /// <summary>
        /// base texture of the tile
        /// </summary>
        private int t_BaseTexture = 0;
        /// <summary>
        /// base texture of the tile
        /// </summary>
        public int BaseTexture { get { return t_BaseTexture; } set { t_BaseTexture = value; } }

        /// <summary>
        /// detail texture of the tile
        /// </summary>
        private int t_DetailTexture = 1;
        /// <summary>
        /// detail texture of the tile
        /// </summary>
        public int DetailTexture
        {
            get { return t_DetailTexture; }
            set { t_DetailTexture = value; }
        }

        /// <summary>
        /// object texture of the tile
        /// </summary>
        private int t_ObjectTexture = 3;
        /// <summary>
        /// object texture of the tile
        /// </summary>
        public int ObjectTexture { get { return t_ObjectTexture; } set { t_ObjectTexture = value; } }

        

        /// <summary>
        /// trigger for this tile
        /// </summary>
        private Trigger t_Trigger;
        /// <summary>
        /// trigger for this tile
        /// </summary>
        public Trigger Trigger { get { return t_Trigger; } set { t_Trigger = value; } }

        /// <summary>
        /// is the tile a blocking tile
        /// </summary>
        private Boolean t_isBlocking = false;
        /// <summary>
        /// is the tile a blocking tile
        /// </summary>
        public Boolean isBlocking { get { return t_isBlocking; } set { t_isBlocking = value; } }

    }
}
