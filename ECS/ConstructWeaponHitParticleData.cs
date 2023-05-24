using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ConstructWeaponHitParticleData : ISystem
    {
        private Filter _filter;

        private Stash<ParticleCollisionData> _particleCollisionData;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<WeaponHitEvent>().With<ParticleCollisionData>();
            _particleCollisionData = World.GetStash<ParticleCollisionData>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var particleCollisionData = ref _particleCollisionData.Get(entity);
                ref var damageDirection = ref entity.AddComponent<DamageDirection>();
                ref var position = ref entity.AddComponent<WorldPosition>();
                ref var gameObjectRef = ref entity.AddComponent<GameObjectRef>();

                damageDirection.Value = particleCollisionData.Value.velocity;
                position.Value = particleCollisionData.Value.intersection;
                gameObjectRef.Value = particleCollisionData.Value.colliderComponent.gameObject;
            }
        }
    }
}