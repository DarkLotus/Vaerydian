using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

namespace Vaerydian.Components.Characters
{
    class Life : IComponent
    {

        private static int l_TypeID;
        private int l_EntityID;

        public Life() { }

        public int getEntityId()
        {
            return l_EntityID;
        }

        public int getTypeId()
        {
            return l_TypeID;
        }

        public void setEntityId(int entityId)
        {
            l_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            l_TypeID = typeId;
        }

        private bool d_IsAlive = true;
        /// <summary>
        /// is the entity currently alive
        /// </summary>
        public bool IsAlive
        {
            get { return d_IsAlive; }
            set { d_IsAlive = value; }
        }

        private int d_TimeSinceDeath = 0;

        /// <summary>
        /// total time spent "dead"
        /// </summary>
        public int TimeSinceDeath
        {
            get { return d_TimeSinceDeath; }
            set { d_TimeSinceDeath = value; }
        }

        private int d_DeathLongevity;
        /// <summary>
        /// total time allowed to be "dead"
        /// </summary>
        public int DeathLongevity
        {
            get { return d_DeathLongevity; }
            set { d_DeathLongevity = value; }
        }


    }
}
