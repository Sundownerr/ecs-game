using System;
using System.Collections.Generic;
using VContainer.Unity;

namespace Game
{
    public class StateMachine : ITickable, IFixedTickable, ILateTickable
    {
        private readonly Dictionary<Type, IState> _states = new();
        private IState _currentState;

        public void FixedTick()
        {
            if (_currentState is IFixedTickableState state)
            {
                state.FixedTick();
            }
        }

        public void LateTick()
        {
            if (_currentState is ILateTickableState state)
            {
                state.LateTick();
            }
        }

        public void Tick()
        {
            if (_currentState is ITickableState state)
            {
                state.Tick();
            }
        }

        public void AddState<T>(T state) where T : IState
        {
            var type = state.GetType();

            if (_states.ContainsKey(type))
            {
                return;
            }

            _states.Add(type, state);
        }

        public void ChangeState<T>() where T : IState
        {
            _currentState?.Exit();
            _currentState = _states[typeof(T)];
            _currentState.Enter();
        }
    }
}