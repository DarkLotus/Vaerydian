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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using BehaviorLibrary;

using Vaerydian.Behaviors;

namespace Vaerydian.Components.Characters
{
    class AiBehavior : IComponent
    {
        private static int a_TypeID;

        public static int TypeID
        {
            get { return AiBehavior.a_TypeID; }
            set { AiBehavior.a_TypeID = value; }
        }
        private int a_EntityID;

        private CharacterBehavior a_Behavior;

        /// <summary>
        /// current behavior of this AI
        /// </summary>
        public CharacterBehavior Behavior
        {
            get { return a_Behavior; }
            set { a_Behavior = value; }
        }

        public AiBehavior() { }

        public AiBehavior(CharacterBehavior behavior) 
        {
            a_Behavior = behavior;
        }

        public int getEntityId()
        {
            return a_EntityID;
        }

        public int getTypeId()
        {
            return a_TypeID;
        }

        public void setEntityId(int entityId)
        {
            a_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            a_TypeID = typeId;
        }
    }
}
