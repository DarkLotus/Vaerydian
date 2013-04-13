using System;

using ECSFramework;
using Vaerydian.Utils;

namespace Vaerydian.Components.Actions
{
	public struct ActionDef{
		public string Name;
		public short ID;
		public ActionType ActionType;
		public ImpactType ImpactType;
		public DamageDef DamageDef;
		public ModifyType ModifyType;
		public ModifyDuration ModifyDurtion;
		public CreateType CreateType;
		public DestoryType DestoryType;
	}

	public enum ActionType{
		DAMAGE = 0,
		MODIFY = 1,
		CREATE = 2,
		DESTROY = 3
	}

	public enum ImpactType{
		NONE = 0,
		DIRECT = 1,
		AREA = 2,
		CONE = 3,
		OVERTIME = 4	
	}

	public enum ModifyType{
		NONE = 0,
		SKILL = 1,
		ABILITY = 2,
		MECHANIC = 3,
		ATTRIBUTE = 4,
		KNOWLEDGE = 5,
	}

	public enum ModifyDuration{
		NONE = 0,
		TEMPORARY = 1,
		PERMANENT = 2,
		LOCATION = 3
	}

	public enum CreateType{
		NONE = 0,
		OBJECT = 1,
		CHARACTER = 2,
		ITEM = 3,
		FEATURE = 4
	}

	public enum DestoryType{
		NONE = 0,
		OBJECT = 1,
		CHARACTER = 2,
		ITEM = 3,
		FEATURE = 4
	}

	public delegate void PerformAction(Entity owner, Entity target);

	public class VAction : IComponent
	{

        private static int a_TypeID;
        private int a_EntityID;
		private event PerformAction action;

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

		public void doAction(){
			action(this.Owner,this.Target);
		}

		public void removeActions(){
			action = null;
		}
	}
}

