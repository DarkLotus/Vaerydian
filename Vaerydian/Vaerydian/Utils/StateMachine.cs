using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Vaerydian.Utils
{
    delegate void Delegate(Object[] parameters);

    delegate void Proxy<T>(T argument) where T : struct, IComparable, IConvertible, IFormattable;

    class EventProxy<EState> where EState : struct, IComparable, IConvertible, IFormattable
    {
        event Proxy<EState> StateChange;

        public void invoke(EState state)
        {
            if (StateChange != null)
                StateChange(state);
        }

        public void bind(Proxy<EState> _delegate)
        {
            StateChange += _delegate;
        }

        public void unbind(Proxy<EState> _delegate)
        {
            StateChange -= _delegate;
        }
    }

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

        public void changeState(TTrigger trigger)
        {
            s_States[s_State].changeState(trigger);
        }

        public void setState(TState state)
        {
            s_State = state;
        }

    }
}
