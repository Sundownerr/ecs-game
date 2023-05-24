using System.Collections.Generic;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class ParticleWeaponProjectile : WeaponFireModule
    {
        [SerializeField] private LayerMask _hitMask;
        [SerializeField] private float _explosionRadius = 10f;
        private readonly Collider[] _results = new Collider[1000];
        private readonly List<ParticleCollisionEvent> collisionEvents = new(1);
        private ParticleSystem _particleSystem;

        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnParticleCollision(GameObject other)
        {
            var collisionCount = _particleSystem.GetCollisionEvents(other, collisionEvents);

            var hitCount = Physics.OverlapSphereNonAlloc(
                collisionEvents[0].intersection,
                _explosionRadius,
                _results,
                _hitMask, QueryTriggerInteraction.Collide);

            for (var i = 0; i < hitCount; i++)
            {
                var hitEvent = World.Default.CreateEntity();
                hitEvent.AddComponent<DamageEvent>();
                hitEvent.AddComponent<OneFrameMarker>();
                ref var damage = ref hitEvent.AddComponent<Damage>();
                ref var damageDirection = ref hitEvent.AddComponent<DamageDirection>();
                ref var position = ref hitEvent.AddComponent<WorldPosition>();
                ref var gameObjectRef = ref hitEvent.AddComponent<GameObjectRef>();

                var hitTransform = _results[i].transform;
                var hitTransformPosition = hitTransform.position;
                damageDirection.Value = (hitTransformPosition - collisionEvents[0].intersection) * _explosionRadius +
                                        Vector3.up * _explosionRadius * 2;
                damageDirection.Value *= 10f;
                position.Value = hitTransformPosition;
                damage.Value = 1;
                gameObjectRef.Value = _results[i].gameObject;

                if (_results[i].TryGetComponent(out EntityReference entityReference))
                {
                    if (entityReference.Entity.IsNullOrDisposed())
                    {
                        continue;
                    }
                    ref var targ = ref hitEvent.AddComponent<Target>();
                    targ.Value = entityReference.Entity;
                }
            }
        }
    }
}