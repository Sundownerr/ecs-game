using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerUIExperience : ISystem
    {
        private readonly PlayerUI _playerUI;
        private Filter _botDeathExperienceFilter;
        private Stash<Experince> _experienceStash;
        private Stash<ExperienceLevel> _levelStash;
        private Filter _playerLevelFilter;

        public PlayerUIExperience(PlayerUI playerUI)
        {
            _playerUI = playerUI;
        }

        public void Dispose()
        { }

        public void OnAwake()
        {
            _botDeathExperienceFilter = World.Filter.With<BotDeathEvent>().With<GivenExperience>();
            _playerLevelFilter = World.Filter.With<PlayerMarker>().With<ExperienceLevel>().With<Experince>();
            _experienceStash = World.GetStash<Experince>();
            _levelStash = World.GetStash<ExperienceLevel>();

            var player = _playerLevelFilter.First();
            ref var playerExperience = ref _experienceStash.Get(player);
            ref var playerLevel = ref _levelStash.Get(player);

            _playerUI.SetExperience((float) playerExperience.Current / playerExperience.Max);
            _playerUI.SetLevel(playerLevel.Value);
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            var player = _playerLevelFilter.First();

            foreach (var entity in _botDeathExperienceFilter)
            {
                ref var playerExperience = ref _experienceStash.Get(player);
                ref var playerLevel = ref _levelStash.Get(player);

                _playerUI.SetExperience((float) playerExperience.Current / playerExperience.Max);
                _playerUI.SetLevel(playerLevel.Value);
            }
        }
    }
}