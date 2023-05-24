using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BotMeleeWeaponAttack : ISystem
    {
        private Stash<Damage> _damageStash;
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<BotMarker>().With<MeleeWeaponMarker>().With<MeleeWeaponSettings>()
                .With<Target>()
                .With<DistanceToTarget>()
                .With<Cooldown>()
                .With<IsUsable>()
                .With<Damage>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var target = ref entity.GetComponent<Target>();

                if (!target.Value.Has<Health>())
                {
                    continue;
                }

                ref var distanceToTarget = ref entity.GetComponent<DistanceToTarget>();
                ref var isUsable = ref entity.GetComponent<IsUsable>();
                ref var meleeWeaponSettings = ref entity.GetComponent<MeleeWeaponSettings>();

                if (isUsable.Value && distanceToTarget.Value <= meleeWeaponSettings.Range)
                {
                    ref var damage = ref entity.GetComponent<Damage>();
                    isUsable.Value = false;

                    var eventEntity = World.CreateEntity();

                    eventEntity.AddComponent<DamageEvent>();
                    eventEntity.AddComponent<OneFrameMarker>();
                    ref var damageToTarget = ref eventEntity.AddComponent<Damage>();
                    damageToTarget.Value = damage.Value;
                }
            }
        }
    }
}