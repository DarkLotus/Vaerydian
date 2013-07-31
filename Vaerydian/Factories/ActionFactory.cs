/*
 Author:
      Thomas H. Jonell <@Net_Gnome>
 
 Copyright (c) 2013 Thomas H. Jonell

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Lesser General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
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

