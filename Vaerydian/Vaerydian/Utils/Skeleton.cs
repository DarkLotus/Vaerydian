using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vaerydian.Utils
{
    class Skeleton
    {

        private List<Bone> s_Bones = new List<Bone>();

        internal List<Bone> Bones
        {
            get { return s_Bones; }
            set { s_Bones = value; }
        }

    }
}
