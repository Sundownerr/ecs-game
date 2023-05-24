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
    public sealed class UpdateCooldown : ISystem
    {
        private Stash<Cooldown> _cooldown;
        private Filter _filter;
        private Stash<IsUsable> _isUsable;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<Cooldown>().With<IsUsable>();
            _cooldown = World.GetStash<Cooldown>();
            _isUsable = World.GetStash<IsUsable>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            // foreach (var entity in _filter)
            // {
            //     ref var cooldown = ref entity.GetComponent<Cooldown>();
            //     ref var isUsable = ref entity.GetComponent<IsUsable>();
            //
            //     if (!isUsable.Value && cooldown.Current > 0)
            //     {
            //         cooldown.Current -= deltaTime;
            //     }
            //
            //     if (!isUsable.Value && cooldown.Current <= 0)
            //     {
            //         isUsable.Value = true;
            //         cooldown.Current = cooldown.Max;
            //     }
            // }

            using var nativeFilter = _filter.AsNative();

            var parallelJob = new Job {
                entities = nativeFilter,
                _cooldown = _cooldown.AsNative(),
                _isUsable = _isUsable.AsNative(),
                deltaTime = deltaTime,
            };

            var parallelJobHandle = parallelJob.Schedule(nativeFilter.length, 64);
            parallelJobHandle.Complete();
        }

        [BurstCompile]
        public struct Job : IJobParallelFor
        {
            [ReadOnly] public NativeFilter entities;
            public NativeStash<Cooldown> _cooldown;
            public NativeStash<IsUsable> _isUsable;
            public float deltaTime;

            public void Execute(int index)
            {
                ref var cooldown = ref _cooldown.Get(entities[index]);
                ref var isUsable = ref _isUsable.Get(entities[index]);

                if (!isUsable.Value && cooldown.Current > 0)
                {
                    cooldown.Current -= deltaTime;
                }

                if (!isUsable.Value && cooldown.Current <= 0)
                {
                    isUsable.Value = true;
                    cooldown.Current = cooldown.Max;
                }
            }
        }
    }
}