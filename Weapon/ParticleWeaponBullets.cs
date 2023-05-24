using System.Collections.Generic;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class ParticleWeaponBullets : WeaponFireModule
    {
        private readonly List<ParticleCollisionEvent> _collisionEvents = new(10);
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnParticleCollision(GameObject other)
        {
            if (!_weaponEntity.Has<WeaponHitEntites>())
            {
                return;
            }

            var hits = _particleSystem.GetCollisionEvents(other, _collisionEvents);
            var weaponHitEntities = _weaponEntity.GetComponent<WeaponHitEntites>().Value;

            for (var i = 0; i < hits; i++)
            {
                for (var j = 0; j < weaponHitEntities.Length; j++)
                {
                    var entity = weaponHitEntities[j].Create(_world, null);
                    ref var parent = ref entity.AddComponent<ParentEntity>();
                    parent.Value = _weaponEntity;

                    ref var data = ref entity.AddComponent<ParticleCollisionData>();
                    data.Value = _collisionEvents[i];
                }
            }

            // _collisionEvents.Clear();
        }
    }
}