using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ConstructAoEWeaponHit : ISystem
    {
        private readonly Collider[] _results = new Collider[5000];
        private Stash<AoeConfig> _aoeConfig;
        private Stash<AoEMarker> _aoeMarker;
        private Stash<Damage> _damage;
        private Stash<DamageExplosionConfig> _damageExplosionConfig;
        private Filter _filter;
        private Stash<GameObjectRef> _gameObjectRef;
        private Stash<HitPosition> _hitPosition;
        private Stash<ParticleCollisionData> _particleCollisionData;
        private Stash<WorldPosition> _worldPosition;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<WeaponHitEvent>().With<WorldPosition>().With<AoeConfig>()
                .With<DamageExplosionConfig>()
                .With<ParticleCollisionData>();

            _particleCollisionData = World.GetStash<ParticleCollisionData>();
            _aoeConfig = World.GetStash<AoeConfig>();
            _aoeMarker = World.GetStash<AoEMarker>();
            _damage = World.GetStash<Damage>();
            _hitPosition = World.GetStash<HitPosition>();
            _worldPosition = World.GetStash<WorldPosition>();
            _gameObjectRef = World.GetStash<GameObjectRef>();
            _damageExplosionConfig = World.GetStash<DamageExplosionConfig>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var aoeConfig = ref _aoeConfig.Get(entity);
                ref var particleCollisionData = ref _particleCollisionData.Get(entity);

                var hitPoint = particleCollisionData.Value.intersection;
                var hitCount = Physics.OverlapSphereNonAlloc(hitPoint, aoeConfig.Radius, _results,
                    aoeConfig.HitMask, QueryTriggerInteraction.Collide);

                ref var damage = ref _damage.Get(entity);
                ref var mainProjectileHitPosition = ref _hitPosition.Add(entity);
                mainProjectileHitPosition.Value = hitPoint;

                ref var damageExplosionConfig = ref _damageExplosionConfig.Get(entity);

                for (var i = 0; i < hitCount; i++)
                {
                    var damageEvent = World.CreateDamageEvent(damage.Value);
                    _aoeMarker.Add(damageEvent);

                    ref var explosionConfigRef = ref _damageExplosionConfig.Add(damageEvent);
                    explosionConfigRef = damageExplosionConfig;

                    ref var aoeConfigRef = ref _aoeConfig.Add(damageEvent);
                    aoeConfigRef = aoeConfig;

                    ref var gameObjectRef = ref _gameObjectRef.Add(damageEvent);
                    gameObjectRef.Value = _results[i].gameObject;

                    ref var position = ref _worldPosition.Add(damageEvent);
                    position.Value = _results[i].transform.position;

                    ref var hitPosition = ref _hitPosition.Add(damageEvent);
                    hitPosition = mainProjectileHitPosition;
                }
            }
        }
    }
}