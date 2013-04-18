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
		

		public static Entity createAction(ActionDef aDef, Entity owner, Entity Target){

			Entity e = ActionFactory.ECSInstance.create ();

			VAction action = new VAction ();
			action.ActionDef = aDef;
			action.Owner = owner;
			action.Target = Target;

			ActionFactory.ECSInstance.EntityManager.addComponent (e, action);

			ActionFactory.ECSInstance.refresh (e);

			return e;
		}
	}
}

