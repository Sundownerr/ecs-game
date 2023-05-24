using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SetDamageDirectionOnHit : ISystem
    {
        private Filter _filter;
        private Stash<Target> _targetStash;
        private Stash<DamageDirection> _damageDirectionStash;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<DamageEvent>().With<DamageDirection>().With<Target>();
            _targetStash = World.GetStash<Target>();
            _damageDirectionStash = World.GetStash<DamageDirection>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var target = ref _targetStash.Get(entity);

                ref var targetLastDamageDirection = ref _damageDirectionStash.Get(target.Value);
                ref var eventDamageDirection = ref _damageDirectionStash.Get(entity);

                targetLastDamageDirection.Value = eventDamageDirection.Value;
            }
        }
    }
}