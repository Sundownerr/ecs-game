using System.Collections.Generic;
using VContainer.Unity;

namespace Game
{
    public class BootstrapEntryPoint : IStartable
    {
        private readonly IEnumerable<IState> _gameStates;
        private readonly StateMachine _stateMachine;

        public BootstrapEntryPoint(IEnumerable<IState> gameStates, StateMachine stateMachine)
        {
            _gameStates = gameStates;
            _stateMachine = stateMachine;
        }

        public void Start()
        {
            foreach (var gameState in _gameStates)
            {
                _stateMachine.AddState(gameState);
            }

            _stateMachine.ChangeState<InMainMenu>();
        }
    }
}