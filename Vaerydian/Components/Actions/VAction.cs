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

