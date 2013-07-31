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
//
//  Target.cs
//
//  Author:
//       tom <>
//
//  Copyright (c) 2013 tom
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using ECSFramework;

namespace Vaerydian.Components.Utils
{
	public class Target : IComponent
	{
		private static int t_TypeID;
		private int t_EntityID;

		public Target() { }

		public int getEntityId()
		{
			return t_EntityID;
		}

		public int getTypeId()
		{
			return t_TypeID;
		}

		public static int TypeId{ get { return t_TypeID; } set { t_TypeID = value; } }

		public void setEntityId(int entityId)
		{
			t_EntityID = entityId;
		}

		public void setTypeId(int typeId)
		{
			t_TypeID = typeId;
		}

		public Entity TargetEntity;

		public bool Active = false;
	}
}

