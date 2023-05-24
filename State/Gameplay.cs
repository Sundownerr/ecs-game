using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class Gameplay : IState, ITickableState, ILateTickableState, IFixedTickableState
    {
        private readonly BotSpawner _botSpawner;
        private readonly FPSInput _input;
        private readonly MorpehECS _morpehEcs;
        private readonly StateMachine _stateMachine;

        public Gameplay(StateMachine stateMachine, FPSInput input, MorpehECS morpehEcs, BotSpawner botSpawner)
        {
            _stateMachine = stateMachine;
            _input = input;
            _morpehEcs = morpehEcs;
            _botSpawner = botSpawner;
        }

        public void FixedTick()
        {
            _morpehEcs.FixedUpdate();
        }

        public void LateTick()
        {
            _morpehEcs.LateUpdate();
        }

        public void Enter()
        {
            NavMesh.avoidancePredictionTime = 0.25f;
            NavMesh.pathfindingIterationsPerFrame = 5000;

            _input.Enable();
            _botSpawner.StartSpawn();

            GameplayEvents.PlayerDied += OnPlayerDied;
        }

        public void Exit()
        {
            _input.Disable();

            GameplayEvents.PlayerDied -= OnPlayerDied;
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked
                    ? CursorLockMode.None
                    : CursorLockMode.Locked;
            }

            _morpehEcs.Update();
        }

        private void OnPlayerDied()
        {
            _stateMachine.ChangeState<PlayerDead>();
            Cursor.lockState = CursorLockMode.None;
        }
    }
}