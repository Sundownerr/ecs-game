using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BotGroupDamaged : ISystem
    {
        private Stash<Damage> _damageStash;
        private Filter _filter;
        private Stash<GivenExperience> _givenExperienceStash;
        private Stash<Health> _healthStash;
        private Stash<Targets> _targetStash;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<DamageEvent>().With<Targets>();
            _targetStash = World.GetStash<Targets>();
            _healthStash = World.GetStash<Health>();
            _givenExperienceStash = World.GetStash<GivenExperience>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var target = ref _targetStash.Get(entity);

                for (var i = 0; i < target.Value.Length; i++)
                {
                    ref var health = ref _healthStash.Get(target.Value[i]);

                    if (health.Value > 0)
                    {
                        continue;
                    }

                    if (!target.Value[i].Has<BotMarker>())
                    {
                        continue;
                    }

                    if (!entity.Has<BotGroupDeathEvent>())
                    {
                        ref var eventComponent = ref entity.AddComponent<BotGroupDeathEvent>();
                        ref var givenExperience = ref entity.AddComponent<GivenExperienceFromGroup>();

                        eventComponent.Bots = target.Value;
                        givenExperience.Value = new int[target.Value.Length];
                    }

                    ref var givenExperienceFromGroup = ref entity.GetComponent<GivenExperienceFromGroup>();
                    givenExperienceFromGroup.Value[i] = _givenExperienceStash.Get(target.Value[i]).Value;
                }
            }
        }
    }
}