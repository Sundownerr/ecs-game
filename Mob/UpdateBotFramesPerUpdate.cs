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
    public sealed class UpdateBotFramesPerUpdate : ISystem
    {
        private Stash<DistanceToTarget> _distanceToPlayerStash;
        private Filter _filter;
        private Stash<FramesPerUpdate> _framesPerUpdateStash;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<BotMarker>().With<FramesPerUpdate>().With<DistanceToTarget>();
            _framesPerUpdateStash = World.GetStash<FramesPerUpdate>();
            _distanceToPlayerStash = World.GetStash<DistanceToTarget>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            using var nativeFilter = _filter.AsNative();

            var parallelJob = new Job {
                entities = nativeFilter,
                framesPerUpdateStash = _framesPerUpdateStash.AsNative(),
                distanceToPlayerStash = _distanceToPlayerStash.AsNative(),
            };

            var parallelJobHandle = parallelJob.Schedule(nativeFilter.length, 64);
            parallelJobHandle.Complete();
        }

        [BurstCompile]
        public struct Job : IJobParallelFor
        {
            [ReadOnly] public NativeFilter entities;
            public NativeStash<FramesPerUpdate> framesPerUpdateStash;
            [ReadOnly] public NativeStash<DistanceToTarget> distanceToPlayerStash;

            public void Execute(int index)
            {
                ref var framesPerUpdate = ref framesPerUpdateStash.Get(entities[index]);
                ref var distanceToPlayer = ref distanceToPlayerStash.Get(entities[index]);

                // framesPerUpdate.Max = (int)(distanceToPlayer.Value );
                framesPerUpdate.Max = (int)(distanceToPlayer.Value * 1.2f);
            }
        }
    }
}