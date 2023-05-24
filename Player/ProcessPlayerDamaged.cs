using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ProcessPlayerDamaged : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<DamageEvent>().With<PlayerMarker>().With<Target>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var eventTarget = ref entity.GetComponent<Target>();
                ref var health = ref eventTarget.Value.GetComponent<Health>();

                if (health.Value <= 0)
                {
                    GameplayEvents.PlayerDied?.Invoke();
                }
            }
        }
    }
}