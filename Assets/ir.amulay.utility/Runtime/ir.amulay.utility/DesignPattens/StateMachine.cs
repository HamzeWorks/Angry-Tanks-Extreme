using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Amulay.Utility
{
    public class StateMachine<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// currentState ,previousState
        /// </summary>
        public event Action<IState, IState> onStateChanged;
        public IState currentState { get; private set; } = null;
        public IState previousState { get; private set; } = null;

        public void ChangeState(IState newState, Action<IState> onCompelete = null)
        {
           // Debug.Log($"<color=cyan>[ {typeof(T).Name}: {currentState} -> {newState} ]</color>");
            currentState?.OnExit(newState);
            previousState = currentState;

            currentState = newState;
            currentState?.OnEnter(previousState, onCompelete);
            onStateChanged?.Invoke(currentState, previousState);
        }
    }

    public interface IState
    {
        void OnEnter(IState previousState, Action<IState> onCompelete = null);
        void OnExit(IState nextState);
    }

    public class _StateMachine<T> : MonoBehaviour where T : MonoBehaviour
    {
        public State currentState { get; private set; } = null;
        public State previousState { get; private set; } = null;

        public void ChangeState(State newState)
        {
            currentState?.OnExit(newState);
            previousState = currentState;

            currentState = newState;
            currentState.OnEnter(previousState);
        }
    }

    public class State : MonoBehaviour
    {
        public virtual void OnEnter(State previousState)
        {

        }

        public virtual void OnExit(State nextState)
        {

        }
    }

    public class EnumStateMachine<T,Tenum> : MonoBehaviour where T : MonoBehaviour where Tenum : Enum
    {
        /// <summary>
        /// NextState ,previousState
        /// </summary>
        public event Action<Tenum, Tenum> onStateChanged;
        private Tenum currentState;
        public Tenum state 
        { 
            get => currentState; 
            protected set 
            {
                if (currentState.Equals(value))
                    return;
                ChangeState(value);
            } 
        }
        public Tenum previousState { get; private set; }

        public virtual void ChangeState(Tenum nextState, Action<Tenum> onCompelete = null)
        {
            Debug.Log($"<color=cyan>[ {typeof(T).Name}: {state} -> {nextState} ]</color>");
            OnExit(nextState);
            previousState = state;

            currentState = nextState;
            OnEnter(previousState, onCompelete);
            onStateChanged?.Invoke(state, previousState);
        }

        public virtual void OnEnter(Tenum state, Action<Tenum> onCompelete = null) { }
        public virtual void OnExit(Tenum nextState) { }
    }
}