using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdatePlayerPosition : ISystem
    {
        private const string LayerName = "Environment";
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<PlayerMarker>().With<WorldPosition>().With<LastNavMeshPosition>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var position = ref entity.GetComponent<WorldPosition>();
                position.Value = entity.GetComponent<TransformRef>().Value.position;

                ref var lastNavMeshPosition = ref entity.GetComponent<LastNavMeshPosition>();

                if (NavMesh.SamplePosition(position.Value, out var hit, 1f, NavMesh.AllAreas))
                {
                    lastNavMeshPosition.Value = hit.position;
                }
                else
                {
                    var hitFloor = Physics.Raycast(position.Value, Vector3.down, out var hitInfo, 200f, 1 << 6);

                    if (hitFloor && NavMesh.SamplePosition(hitInfo.point, out var rayHit, 1f, NavMesh.AllAreas))
                    {
                        lastNavMeshPosition.Value = rayHit.position;
                    }
                }
            }
        }
    }
}