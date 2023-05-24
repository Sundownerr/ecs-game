using System.Collections.Generic;
using VContainer.Unity;

namespace Game
{
    public class LevelEntryPoint : IStartable
    {
        private readonly IEnumerable<IState> _gameStates;
        private readonly StateMachine _stateMachine;

        public LevelEntryPoint(StateMachine stateMachine, IEnumerable<IState> gameStates)
        {
            _stateMachine = stateMachine;
            _gameStates = gameStates;
        }

        public void Start()
        {
            foreach (var gameState in _gameStates)
            {
                _stateMachine.AddState(gameState);
            }

            _stateMachine.ChangeState<CreatingECSWorld>();
        }
    }
} 