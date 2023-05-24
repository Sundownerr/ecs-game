using Scellecs.Morpeh;
using Unity.Collections;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CreateBotDeathVfx : ISystem
    {
        private readonly ParticleSystemEmitter _deathVfxEmitter;
        private Stash<BotDeathSettings> _botDeathSettings;
        private Stash<BotPrefabRef> _botPrefabRef;
        private Stash<DamageDirection> _damageDirection;

        private Filter _filter;
        private Filter _groupFilter;

        public CreateBotDeathVfx(ParticleSystemEmitter deathVfxEmitter)
        {
            _deathVfxEmitter = deathVfxEmitter;
        }

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<BotMarker>().With<BotPrefabRef>().With<OneFrameMarker>();
            _botPrefabRef = World.GetStash<BotPrefabRef>();
            _botDeathSettings = World.GetStash<BotDeathSettings>();
            _damageDirection = World.GetStash<DamageDirection>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            var total = _filter.GetLengthSlow() * 2;

            if (total == 0)
            {
                return;
            }

            var positionsAndDirections = new NativeArray<float3>(_filter.GetLengthSlow() * 2, Allocator.Temp);
            var index = 0;

            foreach (var entity in _filter)
            {
                ref var deathVfxSettings = ref _botDeathSettings.Get(entity);
                ref var botPrefab = ref _botPrefabRef.Get(entity).Value;
                ref var damageDirection = ref _damageDirection.Get(entity).Value;

                positionsAndDirections[index] = botPrefab.DeathVfxPositionWorld();
                positionsAndDirections[index + 1] = damageDirection * deathVfxSettings.DamageDirectionModifier;
                index += 2;
            }

            _deathVfxEmitter.EmitAt2(ref positionsAndDirections);

            positionsAndDirections.Dispose();
        }
    }
}