using System;

using ECSFramework;
using Vaerydian.Utils;

namespace Vaerydian.Components.Actions
{
	public class VAction : IComponent
	{

        private static int a_TypeID;
        private int a_EntityID;

		public VAction ()
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
		public ActionDef ActionDef{ get; set;}

		public void doAction(){
			ActionUtils.doAction (this.Owner, this.Target, this.ActionDef);
		}

	}
}

