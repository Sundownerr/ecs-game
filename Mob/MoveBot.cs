using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MoveBot : ISystem
    {
        private Stash<DistanceToTarget> _distanceToTargets;
        private Filter _filter;
        private Stash<FramesPerUpdate> _framesPerUpdate;
        private Stash<LastNavMeshPosition> _lastNavMeshPositions;
        private Stash<NavMeshAgentRef> _navMeshAgents;
        private Stash<Target> _targets;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _navMeshAgents = World.GetStash<NavMeshAgentRef>();
            _targets = World.GetStash<Target>();
            _lastNavMeshPositions = World.GetStash<LastNavMeshPosition>();
            _framesPerUpdate = World.GetStash<FramesPerUpdate>();
            _distanceToTargets = World.GetStash<DistanceToTarget>();

            _filter = World.Filter.With<NavMeshAgentRef>().With<Target>().With<FramesPerUpdate>()
                .With<DistanceToTarget>().Without<CantMoveOnNavMesh>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var framesPerUpdate = ref _framesPerUpdate.Get(entity);

                if (framesPerUpdate.Current > 0)
                {
                    continue;
                }

                ref var distanceToTarget = ref _distanceToTargets.Get(entity);

                if (distanceToTarget.Value < 2f)
                {
                    continue;
                }

                ref var navMeshAgent = ref _navMeshAgents.Get(entity);

                if (!navMeshAgent.Value.enabled)
                {
                    continue;
                }

                ref var target = ref _targets.Get(entity);

                ref var targetNavMeshPosition = ref _lastNavMeshPositions.Get(target.Value);

                navMeshAgent.Value.SetDestinationDeferred(targetNavMeshPosition.Value);
            }
        }
    }
}