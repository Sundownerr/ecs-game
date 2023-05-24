using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SetTargetMarkerToDamageEvent : ISystem
    {
        private Filter _filter;
        private Stash<Target> _targetStash;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<DamageEvent>().With<Target>();
            _targetStash = World.GetStash<Target>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var target = ref _targetStash.Get(entity);

                if (target.Value.Has<BotMarker>())
                {
                    entity.AddComponent<BotMarker>();
                }
                
                if (target.Value.Has<PlayerMarker>())
                {
                    entity.AddComponent<PlayerMarker>();
                }
            }
        }
    }
}