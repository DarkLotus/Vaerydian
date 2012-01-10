using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ECSFramework
{
    public sealed class Entity
    {
        private int e_Id;

        public int Id
        {
            get { return e_Id; }
            set { e_Id = value; }
        }

        public Entity() { }

        public Entity(int id)
        {
            this.e_Id = id;
        }
    }
}
