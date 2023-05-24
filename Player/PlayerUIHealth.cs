using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerUIHealth : ISystem
    {
        private readonly PlayerUI _playerUI;
        private Stash<Health> _health;
        private Stash<HealthConfig> _healthConfig;

        private Filter _playerDamagedEventFilter;
        private Filter _playerHealthFilter;

        public PlayerUIHealth(PlayerUI playerUI)
        {
            _playerUI = playerUI;
        }

        public void Dispose()
        { }

        public void OnAwake()
        {
            _playerDamagedEventFilter = World.Filter.With<DamageEvent>().With<PlayerMarker>().With<Damage>();

            _playerHealthFilter = World.Filter.With<PlayerMarker>().With<Health>().With<HealthRegen>()
                .With<HealthConfig>();

            _health = World.GetStash<Health>();
            _healthConfig = World.GetStash<HealthConfig>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            // var player = _playerHealthFilter.First();

            foreach (var entity in _playerHealthFilter)
            {
                ref var health = ref _health.Get(entity);
                ref var healthConfig = ref _healthConfig.Get(entity);
                _playerUI.SetHealth(health.Value, healthConfig.Max);
            }
        }
    }
}