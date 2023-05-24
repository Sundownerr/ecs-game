using Scellecs.Morpeh;
using Scellecs.Morpeh.Native;
using Unity.Burst;
using Unity.Collections;
using Unity.IL2CPP.CompilerServices;
using Unity.Jobs;
using Unity.Mathematics;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ClampHealth : ISystem
    {
        private Filter _filter;
        private Stash<Health> _health;
        private Stash<HealthConfig> _healthConfig;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<HealthConfig>().With<Health>();
            _healthConfig = World.GetStash<HealthConfig>();
            _health = World.GetStash<Health>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            using var nativeFilter = _filter.AsNative();
            var parallelJob = new Job {
                entities = nativeFilter,
                _health = _health.AsNative(),
                _healthConfig = _healthConfig.AsNative(),
            };

            var parallelJobHandle = parallelJob.Schedule(nativeFilter.length, 64);
            parallelJobHandle.Complete();
        }

        [BurstCompile]
        public struct Job : IJobParallelFor
        {
            [ReadOnly] public NativeFilter entities;
            public NativeStash<Health> _health;
            [ReadOnly] public NativeStash<HealthConfig> _healthConfig;
           
            public void Execute(int index)
            {
                ref var health = ref _health.Get(entities[index]);
                ref var config = ref _healthConfig.Get(entities[index]);

                health.Value = math.clamp(health.Value, 0, config.Max);
            }
        }
    }
}