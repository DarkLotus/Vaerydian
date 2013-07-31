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
using System.Reflection;

namespace Vaerydian.Utils
{
    /// <summary>
    /// universal delegate with 0-many parameters contained within an object array
    /// </summary>
    /// <param name="parameters">array of object parameters</param>
    delegate void Delegate(Object[] parameters);

    /// <summary>
    /// delegate of that uses an argument of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="argument"></param>
    delegate void Proxy<T>(T argument);

    //delegate R Proxy<T,R>(T argument);

    /// <summary>
    /// dynamic event wrapper proxy class. allows creation of events on the fly. Used to trigger state changes
    /// </summary>
    /// <typeparam name="PType"></typeparam>
    class EventProxy<PType>
    {
        /// <summary>
        /// create a proxy event of PType
        /// </summary>
        private event Proxy<PType> StateChange;

        /// <summary>
        /// trigger the state change
        /// </summary>
        /// <param name="state"></param>
        public void invoke(PType state)
        {
            if (StateChange != null)
                StateChange(state);
        }

        /// <summary>
        /// bind a delegate to the state change event
        /// </summary>
        /// <param name="_delegate">proxy delegate to be bound</param>
        public void bind(Proxy<PType> _delegate)
        {
            StateChange += _delegate;
        }

        /// <summary>
        /// ubind a delegate to the state change event
        /// </summary>
        /// <param name="_delegate"></param>
        public void unbind(Proxy<PType> _delegate)
        {
            StateChange -= _delegate;
        }

        /// <summary>
        /// unbinds all state change delegates
        /// </summary>
        public void unbind()
        {
            StateChange = null;
        }
    }

    /// <summary>
    /// represents a single state in the state machine
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <typeparam name="TTrigger"></typeparam>
    class State<TState, TTrigger> where TState : struct, IComparable, IConvertible, IFormattable
    {
        private TState s_ThisState;

        private Delegate s_Delegate;

        private Dictionary<TTrigger, EventProxy<TState>> s_TransitionEvent = new Dictionary<TTrigger, EventProxy<TState>>();

        private Dictionary<TTrigger, TState> s_TransitionState = new Dictionary<TTrigger, TState>();

        public State(TState state, Delegate _delegate)
        {
            s_ThisState = state;
            s_Delegate = _delegate;
        }

        public void defineStateChange(EventProxy<TState> proxy, TTrigger trigger, TState desingationState, Proxy<TState> _delegate)
        {
            proxy.bind(_delegate);
            s_TransitionEvent.Add(trigger, proxy);
            s_TransitionState.Add(trigger, desingationState);
        }

        public void changeState(TTrigger trigger)
        {
            if (s_TransitionEvent.ContainsKey(trigger))
                s_TransitionEvent[trigger].invoke(s_TransitionState[trigger]);
#if DEBUG
            else
            {
                Console.Error.WriteLine("Trigger Not Defined For Current State - State: {0} Trigger: {1}", s_ThisState.ToString(), trigger.ToString());
            }
#endif
        }

        public void invoke(Object[] parameters)
        {
            s_Delegate.Invoke(parameters);
        }
    }

    class StateMachine<TState, TTrigger> where TState : struct, IComparable, IConvertible, IFormattable
    {
        private TState s_State;

        private Dictionary<TState, State<TState, TTrigger>> s_States = new Dictionary<TState, State<TState, TTrigger>>();

        public StateMachine(TState baseState, Delegate _delegate, TTrigger trigger)
        {
            s_State = baseState;

            State<TState, TTrigger> state = new State<TState, TTrigger>(baseState, _delegate);

            state.defineStateChange(new EventProxy<TState>(), trigger, baseState, onStateChange<TState>);

            s_States.Add(s_State, state);
        }

        private void onStateChange<EState>(TState state) where EState : struct, IComparable, IConvertible, IFormattable
        {
            s_State = state;
        }

        public void evaluate(params Object[] parameters)
        {
            if (s_States.ContainsKey(s_State))
                s_States[s_State].invoke(parameters);
        }

        public void addState(TState state, Delegate _delegate)
        {
            State<TState, TTrigger> newState = new State<TState, TTrigger>(state, _delegate);
            s_States.Add(state, newState);
        }

        public void addStateChange(TState originState, TState desinationState, TTrigger trigger)
        {
            s_States[originState].defineStateChange(new EventProxy<TState>(), trigger, desinationState, onStateChange<TState>);
        }

        public TState changeState(TTrigger trigger)
        {
            s_States[s_State].changeState(trigger); 
            return s_State;
        }

        public TState State
        {
            get { return s_State; }
            set { s_State = value; }
        }
    }
}
