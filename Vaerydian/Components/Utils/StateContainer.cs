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
