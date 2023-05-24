using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ConstructWeaponHitDamage : ISystem
    {
        private Stash<Damage> _damage;
        private Filter _filter;
        private Stash<ParentEntity> _parentEntity;
        private Stash<WeaponConfig> _weaponConfig;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<WeaponHitEvent>().With<ParentEntity>().With<Damage>().With<OneFrameMarker>();

            _damage = World.GetStash<Damage>();
            _parentEntity = World.GetStash<ParentEntity>();
            _weaponConfig = World.GetStash<WeaponConfig>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var parent = ref _parentEntity.Get(entity);
                ref var weaponConfig = ref _weaponConfig.Get(parent.Value);
                ref var damage = ref _damage.Get(entity);

                damage.Value = weaponConfig.Damage;
                entity.AddComponent<DamageEvent>();
            }
        }
    }
}