using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class CreatePlayer : IState
    {
        private readonly PlayerFactory _playerFactory;
        private readonly StateMachine _stateMachine;
        private readonly WeaponFactory _weaponFactory;
        private readonly GameIDs _gameIDs;

        public CreatePlayer(StateMachine stateMachine, PlayerFactory playerFactory, WeaponFactory weaponFactory, GameIDs gameIDs)
        {
            _stateMachine = stateMachine;
            _playerFactory = playerFactory;
            _weaponFactory = weaponFactory;
            _gameIDs = gameIDs;
        }

        public void Enter()
        {
            var (playerEntity, playerPrefab) = _playerFactory.Create(_gameIDs.Player.Value, Vector3.zero);

            // _weaponFactory.Create(_gameIDs.PrimaryWeapon.Value, playerPrefab.PrimaryWeaponHolder);
            // _weaponFactory.Create(_gameIDs.SecondaryWeapon.Value, playerPrefab.SecondaryWeaponHolder);
          
            _stateMachine.ChangeState<Gameplay>();
        }

        public void Exit()
        { }
    }
}