using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DecreaseHealthOnDamageEvent : ISystem
    {
        private Stash<Damage> _damageStash;
        private Filter _filter;
        private Stash<Health> _healthStash;
        private Stash<Target> _targetStash;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<DamageEvent>().With<Target>().With<Damage>();
            _targetStash = World.GetStash<Target>();
            _damageStash = World.GetStash<Damage>();
            _healthStash = World.GetStash<Health>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var target = ref _targetStash.Get(entity);
                ref var damage = ref _damageStash.Get(entity);
                ref var health = ref _healthStash.Get(target.Value);

                health.Value -= damage.Value;
            }
        }
    }
}