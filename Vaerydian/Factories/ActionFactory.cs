using System;
using ECSFramework;
using Vaerydian.Components.Actions;
using Vaerydian.Utils;
using System.Collections.Generic;

namespace Vaerydian
{
	public class ActionFactory
	{
		private ECSInstance a_ECSInstance;
		
		public ActionFactory(ECSInstance ecsInstance)
		{
			a_ECSInstance = ecsInstance;
		}

		public Entity createAction(ActionDef aDef){

			Entity e = a_ECSInstance.create ();

			VAction action = new VAction ();

			a_ECSInstance.EntityManager.addComponent (e, action);


			a_ECSInstance.refresh (e);

			return e;
		}

		public void doSomething(Dictionary<string, TerrainDef> tDefs){
			DynamicSwitch<string> ds = new DynamicSwitch<string>();

			foreach (string tDefName in tDefs.Keys) {
				ds.addCase(tDefName, (string s) =>{



					return;
				});
			}

		}

		public void blah(){}

		public void assignAction(VAction action, ActionDef aDef){

			switch (aDef.ActionType) {
			case ActionType.DAMAGE:
				break;
			case ActionType.MODIFY:
				break;
			case ActionType.CREATE:
				break;
			case ActionType.DESTROY:
				break;
			default:
				break;
			}

		}
	}
}

