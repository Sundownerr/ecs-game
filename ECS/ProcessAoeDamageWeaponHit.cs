using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ProcessAoeDamageWeaponHit : ISystem
    {
        private Stash<AoeConfig> _aoeConfig;
        private Stash<DamageDirection> _damageDirection;
        private Filter _filter;
        private Stash<WeaponConfig> _weaponConfig;
        private Stash<WorldPosition> _worldPosition;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<WeaponHitEvent>().With<Targets>().With<WorldPosition>().With<AoeConfig>()
                .With<WeaponConfig>();

            _aoeConfig = World.GetStash<AoeConfig>();
            _weaponConfig = World.GetStash<WeaponConfig>();
            _worldPosition = World.GetStash<WorldPosition>();
            _damageDirection = World.GetStash<DamageDirection>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var aoeSettings = ref _aoeConfig.Get(entity);
                ref var weaponConfig = ref _weaponConfig.Get(entity);
                ref var targets = ref entity.GetComponent<Targets>();
                ref var hitPosition = ref _worldPosition.Get(entity);
                
                
                for (var i = 0; i < targets.Value.Length; i++)
                {
                    if (targets.Value[i].Has<Health>())
                    {
                        ref var health = ref targets.Value[i].GetComponent<Health>();
                        health.Value -= weaponConfig.Damage;
                    }

                    if (targets.Value[i].Has<DamageDirection>())
                    {
                        ref var damageDirection = ref _damageDirection.Get(targets.Value[i]);
                        ref var position = ref _worldPosition.Get(targets.Value[i]);

                        var upDirection = math.up() * aoeSettings.Radius * 4f;
                        var outDirecton = (position.Value - hitPosition.Value) *
                                          aoeSettings.Radius;
                        damageDirection.Value = outDirecton + upDirection;
                        damageDirection.Value *= 8f;
                    }
                }
            }
        }
    }
}