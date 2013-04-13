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
			action.ActionDef = aDef;

			a_ECSInstance.EntityManager.addComponent (e, action);

			a_ECSInstance.refresh (e);

			return e;
		}
	}
}

