using System;
using ECSFramework;
using Vaerydian.Components.Actions;
using Vaerydian.Utils;
using System.Collections.Generic;

namespace Vaerydian
{
	public static class ActionFactory
	{
		public static ECSInstance ECSInstance;
		

		public static Entity createAction(ActionDef aDef){

			Entity e = ActionFactory.ECSInstance.create ();

			VAction action = new VAction ();
			action.ActionDef = aDef;

			ActionFactory.ECSInstance.EntityManager.addComponent (e, action);

			ActionFactory.ECSInstance.refresh (e);

			return e;
		}
	}
}

