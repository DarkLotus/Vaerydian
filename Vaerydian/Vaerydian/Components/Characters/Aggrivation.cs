using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

namespace Vaerydian.Components.Characters
{
    class Aggrivation : IComponent
    {
        private static int a_TypeID;

        public static int TypeID
        {
            get { return Aggrivation.a_TypeID; }
            set { Aggrivation.a_TypeID = value; }
        }
        private int a_EntityID;

        public Aggrivation() { }

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

        private List<Entity> a_HateList = new List<Entity>();

        public List<Entity> HateList
        {
            get { return a_HateList; }
            set { a_HateList = value; }
        }

        private Entity a_Target;

        public Entity Target
        {
            get { return a_Target; }
            set { a_Target = value; }
        }
    }
}
