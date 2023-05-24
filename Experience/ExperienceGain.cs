using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ExperienceGain : ISystem
    {
        private Stash<Experince> _experienceStash;
        private Filter _filter;
        private Stash<ExperienceLevel> _levelStash;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<ExperienceLevel>().With<Experince>();
            _levelStash = World.GetStash<ExperienceLevel>();
            _experienceStash = World.GetStash<Experince>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var level = ref _levelStash.Get(entity);
                ref var experience = ref _experienceStash.Get(entity);

                if (experience.Current < experience.Max)
                {
                    continue;
                }

                var levelUps = 1;
                var remainingExperience = experience.Current - experience.Max;
                
                experience.Max = (int) (experience.Max * 1.5f);

                while (remainingExperience >= experience.Max)
                {
                    levelUps++;
                    remainingExperience -= experience.Max;
                    
                    experience.Max = (int) (experience.Max * 1.5f);
                }

                experience.Current = 0;

                level.Value += levelUps;
            }
        }
    }
}