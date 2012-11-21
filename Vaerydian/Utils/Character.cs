using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Vaerydian.Utils;
using ECSFramework;

namespace Vaerydian.Components.Characters
{
    class Character : IComponent
    {

        private static int c_TypeID;

        private int c_EntityID;

        public Character() { }

        public int getEntityId()
        {
            return c_EntityID;
        }

        public int getTypeId()
        {
            return c_TypeID;
        }

        public void setEntityId(int entityId)
        {
            c_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            c_TypeID = typeId;
        }

        private String c_CurrentSkeleton;

        public String CurrentSkeleton
        {
            get { return c_CurrentSkeleton; }
            set { c_CurrentSkeleton = value; }
        }

        private Dictionary<String, Skeleton> c_Skeletons = new Dictionary<string, Skeleton>();

        public Dictionary<String, Skeleton> Skeletons
        {
            get { return c_Skeletons; }
            set { c_Skeletons = value; }
        }

        private String c_CurrentAnimtaion;

        public String CurrentAnimtaion
        {
            get { return c_CurrentAnimtaion; }
            set { c_CurrentAnimtaion = value; }
        }

    }
}
