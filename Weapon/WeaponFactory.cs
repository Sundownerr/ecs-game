using System.Collections.Generic;
using Game.ECS;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class WeaponSlot : MonoBehaviour
    {
        public ID ID;
        public Transform Slot;
    }

    public class WeaponFactory
    {
        private readonly Dictionary<int, EntitySO> _entites;
        private readonly Dictionary<int, GameObject> _prefabs;

        public WeaponFactory(Dictionary<int, GameObject> prefabs, Dictionary<int, EntitySO> entites)
        {
            _prefabs = prefabs;
            _entites = entites;
        }

        public Entity Create(int id, Transform weaponSlot)
        {
            var weaponTemplate = _entites[id];
            var weaponPrefab = _prefabs[id];
            var weaponInstance = Object.Instantiate(weaponPrefab, weaponSlot);
            var weaponEntity = weaponTemplate.Create(World.Default, weaponInstance);

            return weaponEntity;
        }
    }
}