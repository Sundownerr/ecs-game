using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BotDamaged : ISystem
    {
        private Stash<BotMarker> _botMarker;
        private Filter _filter;
        private Stash<Health> _healthStash;
        private Stash<OneFrameMarker> _oneFrameMarker;
        private Stash<Target> _targetStash;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<DamageEvent>().With<Target>();
            _targetStash = World.GetStash<Target>();
            _healthStash = World.GetStash<Health>();
            _botMarker = World.GetStash<BotMarker>();
            _oneFrameMarker = World.GetStash<OneFrameMarker>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var target = ref _targetStash.Get(entity);
                ref var health = ref _healthStash.Get(target.Value);

                if (health.Value > 0)
                {
                    continue;
                }

                if (!_botMarker.Has(entity))
                {
                    continue;
                }

                if (!_oneFrameMarker.Has(target.Value))
                {
                    _oneFrameMarker.Add(target.Value);
                }
            }
        }
    }
}