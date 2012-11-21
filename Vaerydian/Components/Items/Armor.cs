using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Utils;

namespace Vaerydian.Components.Items
{
    class Armor : IComponent
    {
        private static int a_TypeID;
        private int a_EntityID;

        public Armor() { }

        public Armor(int mitigation, int mobility)
        {
            a_Mitigation = mitigation;
            a_Mobility = mobility;
        }

        public int getEntityId()
        {
            return a_EntityID;
        }

        public int getTypeId()
        {
            return a_TypeID;
        }

        public void setEntityId(int entityId)
        {
            a_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            a_TypeID = typeId;
        }

        private int a_Mitigation;
        /// <summary>
        /// the amount of lethality the armor can mitigate
        /// </summary>
        public int Mitigation
        {
          get { return a_Mitigation; }
          set { a_Mitigation = value; }
        }

        private int a_Mobility;
        /// <summary>
        /// the mobility cost of the armor
        /// </summary>
        public int Mobility
        {
          get { return a_Mobility; }
          set { a_Mobility = value; }
        }
    }
}
