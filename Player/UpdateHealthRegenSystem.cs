using Scellecs.Morpeh;
using Scellecs.Morpeh.Native;
using Unity.Burst;
using Unity.Collections;
using Unity.IL2CPP.CompilerServices;
using Unity.Jobs;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateHealthRegen : ISystem
    {
        private Filter _filter;
        private Stash<HealthConfig> _healthConfigs;
        private Stash<HealthRegen> _healthRegens;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<HealthConfig>().With<HealthRegen>().With<Construct_OneFrame>();
            _healthRegens = World.GetStash<HealthRegen>();
            _healthConfigs = World.GetStash<HealthConfig>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            using var nativeFilter = _filter.AsNative();

            var parallelJob = new Job {
                entities = nativeFilter,
                _healthRegens = _healthRegens.AsNative(),
                _healthConfigs = _healthConfigs.AsNative(),
            };

            var parallelJobHandle = parallelJob.Schedule(nativeFilter.length, 64);
            parallelJobHandle.Complete();
        }

        [BurstCompile]
        public struct Job : IJobParallelFor
        {
            [ReadOnly] public NativeFilter entities;
            public NativeStash<HealthRegen> _healthRegens;
            [ReadOnly] public NativeStash<HealthConfig> _healthConfigs;

            public void Execute(int index)
            {
                ref var healthRegen = ref _healthRegens.Get(entities[index]);
                ref var healthConfig = ref _healthConfigs.Get(entities[index]);

                healthRegen.Value = healthConfig.RegenPerSecond;
            }
        }
    }
}