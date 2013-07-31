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
using System.Threading.Tasks;

using ECSFramework;

using Vaerydian.Utils;

namespace Vaerydian.Components.Utils
{
    class StateContainer<TState, TTrigger> : IComponent where TState : struct, IComparable, IConvertible, IFormattable
    {
        private static int s_TypeID;

        public static int TypeID
        {
          get { return StateContainer<TState, TTrigger>.s_TypeID; }
          set { StateContainer<TState, TTrigger>.s_TypeID = value; }
        }

        private int s_EntityID;

        public StateContainer() { }

        public int getEntityId()
        {
            return s_EntityID;
        }

        public int getTypeId()
        {
            return s_TypeID;
        }

        public void setEntityId(int entityId)
        {
            s_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            s_TypeID = typeId;
        }

        private StateMachine<TState, TTrigger> s_StateMachine;

        public StateMachine<TState, TTrigger> StateMachine
        {
            get { return s_StateMachine; }
            set { s_StateMachine = value; }
        }
    }
}
