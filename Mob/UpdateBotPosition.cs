using Scellecs.Morpeh;
using Scellecs.Morpeh.Native;
using Unity.Burst;
using Unity.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Jobs;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateBotPosition : ISystem
    {
        private Filter _filter;
        private TransformAccessArray _transformAccessArray;
        private Stash<TransformRef> _transformStash;
        private Stash<WorldPosition> _worldPositionStash;

        public void Dispose()
        {
            // _transformAccessArray.Dispose();
        }

        public void OnAwake()
        {
            _worldPositionStash = World.GetStash<WorldPosition>();
            _transformStash = World.GetStash<TransformRef>();
            _filter = World.Filter.With<BotMarker>().With<WorldPosition>().With<TransformRef>().With<BotPrefabRef>()
                .With<NavMeshAgentRef>();

            // _transformAccessArray = new TransformAccessArray(1);
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var worldPosition = ref _worldPositionStash.Get(entity);
                ref var transform = ref _transformStash.Get(entity);
            
                worldPosition.Value = transform.Value.position;
            }

            // var filterLength = _filter.GetLengthSlow();
            // var transformsLength = _transformAccessArray.length;
            //
            // Debug.Log($" {transformsLength} - {filterLength}");
            //
            // if (transformsLength > filterLength)
            // {
            //     for (var i = filterLength; i < transformsLength; i++)
            //     {
            //         _transformAccessArray.RemoveAtSwapBack(filterLength);
            //         Debug.Log($"Removed {i}");
            //     }
            // }
            // else if (filterLength > transformsLength)
            // {
            //     var index = 0;
            //
            //     foreach (var entity in _filter)
            //     {
            //         if (index > transformsLength -1)
            //         {
            //             _transformAccessArray.Add(_transformStash.Get(entity).Value);
            //         }
            //         else
            //         {
            //             _transformAccessArray[index] = _transformStash.Get(entity).Value;
            //         }
            //
            //         index++;
            //     }
            //
            //     Debug.Log("changed");
            // }
            //
            // using (var nativeFilter = _filter.AsNative())
            // {
            //     var parallelJob = new PositionUpdateJob {
            //         entities = nativeFilter,
            //         positions = _worldPositionStash.AsNative(),
            //     };
            //
            //     var parallelJobHandle = parallelJob.Schedule(_transformAccessArray);
            //     parallelJobHandle.Complete();
            // }
        }

        // [BurstCompile]
        // public struct PositionUpdateJob : IJobParallelForTransform
        // {
        //     [ReadOnly] public NativeFilter entities;
        //     public NativeStash<WorldPosition> positions;
        //
        //     public void Execute(int index, TransformAccess transform)
        //     {
        //         ref var position = ref positions.Get(entities[index]);
        //         position.Value = transform.position;
        //     }
        // }
    }
}