using UnityEngine;

namespace Game
{
    public class InitializingLevel : IState
    {
        
        public void Enter()
        {
            
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }

    public class CreatingECSWorld : IState
    {
        private readonly MorpehECS _morpehEcs;
        private readonly StateMachine _stateMachine;

        public CreatingECSWorld(StateMachine stateMachine, MorpehECS morpehEcs)
        {
            _stateMachine = stateMachine;
            _morpehEcs = morpehEcs;
        }

        public void Enter()
        {
            _morpehEcs.Install();

            _stateMachine.ChangeState<CreatePlayer>();
        }

        public void Exit()
        { }
    }
}