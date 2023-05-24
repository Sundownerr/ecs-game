using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerExperienceGain : ISystem
    {
        private Stash<Experince> _experienceStash;
        private Filter _deadBotFilter;
        private Filter _playerFilter;
        private Stash<GivenExperience> _givenExperienceStash;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _deadBotFilter = World.Filter.With<BotMarker>().With<BotPrefabRef>().With<OneFrameMarker>().With<GivenExperience>();
            _playerFilter = World.Filter.With<PlayerMarker>().With<ExperienceLevel>().With<Experince>();
          
            _experienceStash = World.GetStash<Experince>();
            _givenExperienceStash = World.GetStash<GivenExperience>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            var player = _playerFilter.First();
            
            foreach (var entity in _deadBotFilter)
            {
                ref var playerExperience = ref _experienceStash.Get(player);
                ref var mobExperience = ref _givenExperienceStash.Get(entity);
                
                playerExperience.Current += mobExperience.Value;
            }
        }
    }
}