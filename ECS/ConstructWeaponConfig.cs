using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ConstructWeaponConfig : ISystem
    {
        private Stash<Damage> _damageStash;
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<Construct_OneFrame>().With<WeaponConfig>().With<Damage>()
                .With<Cooldown>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var weaponConfig = ref entity.GetComponent<WeaponConfig>();
                ref var damage = ref entity.GetComponent<Damage>();
                ref var cooldown = ref entity.GetComponent<Cooldown>();

                damage.Value = weaponConfig.Damage;
                cooldown.Max = weaponConfig.Cooldown;
            }
        }
    }
}