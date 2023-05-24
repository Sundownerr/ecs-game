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
    public sealed class RegenrateHealth : ISystem
    {
        private Filter _filter;
        private Stash<Health> _health;
        private Stash<HealthRegen> _healthRegen;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<Health>().With<HealthRegen>();
            _healthRegen = World.GetStash<HealthRegen>();
            _health = World.GetStash<Health>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            using var nativeFilter = _filter.AsNative();
            var parallelJob = new Job {
                entities = nativeFilter,
                _health = _health.AsNative(),
                _healthRegen = _healthRegen.AsNative(),
                deltaTime = deltaTime
            };

            var parallelJobHandle = parallelJob.Schedule(nativeFilter.length, 64);
            parallelJobHandle.Complete();
        }

        [BurstCompile]
        public struct Job : IJobParallelFor
        {
            [ReadOnly] public NativeFilter entities;
            public NativeStash<Health> _health;
            [ReadOnly] public NativeStash<HealthRegen> _healthRegen;
            public float deltaTime;

            public void Execute(int index)
            {
                ref var health = ref _health.Get(entities[index]);
                ref var healthRegen = ref _healthRegen.Get(entities[index]);

                health.Value += healthRegen.Value * deltaTime;
            }
        }
    }
}