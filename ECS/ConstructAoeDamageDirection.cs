using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ConstructAoeDamageDirection : ISystem
    {
        private Stash<AoeConfig> _aoeConfig;
        private Stash<DamageDirection> _damageDirection;
        private Stash<DamageExplosionConfig> _damageExplosionConfig;
        private Filter _filter;
        private Stash<HitPosition> _hitPosition;
        private Stash<Target> _target;
        private Stash<WorldPosition> _worldPosition;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<DamageEvent>().With<AoEMarker>().With<AoeConfig>()
                .With<DamageExplosionConfig>()
                .With<WorldPosition>()
                .With<HitPosition>()
                .With<Target>();

            _aoeConfig = World.GetStash<AoeConfig>();
            _damageExplosionConfig = World.GetStash<DamageExplosionConfig>();
            _worldPosition = World.GetStash<WorldPosition>();
            _hitPosition = World.GetStash<HitPosition>();
            _target = World.GetStash<Target>();
            _damageDirection = World.GetStash<DamageDirection>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var target = ref _target.Get(entity);
                ref var damageDirection = ref _damageDirection.Get(target.Value);
                ref var aoeConfig = ref _aoeConfig.Get(entity);
                ref var targetPosition = ref _worldPosition.Get(entity);
                ref var hitPosition = ref _hitPosition.Get(entity);
                ref var explosionConfig = ref _damageExplosionConfig.Get(entity);

                var upDirection = math.up() * aoeConfig.Radius * explosionConfig.UpForce;
                var outDirecton = (targetPosition.Value - hitPosition.Value) * aoeConfig.Radius;
                damageDirection.Value = (outDirecton + upDirection) * explosionConfig.OutForce;
            }
        }
    }
}