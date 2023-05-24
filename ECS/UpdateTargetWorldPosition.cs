using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateTargetWorldPosition : ISystem
    {
        private Filter _filter;
        private Stash<Target> _targetStash;
        private Stash<TargetWorldPosition> _targetWorldPositionStash;
        private Stash<WorldPosition> _worldPositionStash;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<Target>().With<TargetWorldPosition>();
            _targetStash = World.GetStash<Target>();
            _worldPositionStash = World.GetStash<WorldPosition>();
            _targetWorldPositionStash = World.GetStash<TargetWorldPosition>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var target = ref _targetStash.Get(entity);
                ref var targetWorldPosition = ref _targetWorldPositionStash.Get(entity);
                
                targetWorldPosition.Value = _worldPositionStash.Get(target.Value).Value;
            }
        }
    }
}