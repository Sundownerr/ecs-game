using System.Collections.Generic;
using Game.ECS;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class PlayerFactory
    {
        private readonly Dictionary<int, EntitySO> _entities;
        private readonly Transform _levelRoot;
        private readonly Dictionary<int, GameObject> _prefabs;

        public PlayerFactory(Transform levelRoot, Dictionary<int, GameObject> prefabs, Dictionary<int, EntitySO> entities)
        {
            _levelRoot = levelRoot;
            _prefabs = prefabs;
            _entities = entities;
        }

        public (Entity, PlayerPrefab) Create(int id, Vector3 position)
        {
            var entity = _entities[id];
            var prefab = _prefabs[id];

            var playerInstance = Object.Instantiate(prefab, position, Quaternion.identity, _levelRoot);
            var playerEntity = entity.Create(World.Default, playerInstance);

            return (playerEntity, playerInstance.GetComponent<PlayerPrefab>());
        }
    }
}