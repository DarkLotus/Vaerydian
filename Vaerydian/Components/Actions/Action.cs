using System;

using ECSFramework;

namespace Vaerydian
{
	public delegate void PerformAction(Entity owner, Entity target);

	public class Action : IComponent
	{

        private static int a_TypeID;
        private int a_EntityID;
		private event PerformAction action;

		public Action ()
		{
		}

        public int getEntityId()
        {
            return a_EntityID;
        }

        public int getTypeId()
        {
            return a_TypeID;
        }

		public static int TypeId{ 
			get { return a_TypeID; }
			set { a_TypeID = value; } 
		}

        public void setEntityId(int entityId)
        {
            a_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            a_TypeID = typeId;
        }

		public Entity Owner{ get; set; }
		public Entity Target{ get; set; }

		public void doAction(){
			action(this.Owner,this.Target);
		}

		public void removeActions(){
			action = null;
		}
	}
}

