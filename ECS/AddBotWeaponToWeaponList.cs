using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AddBotWeaponToWeaponList : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<Construct_OneFrame>().With<ParentEntity>().With<MeleeWeaponMarker>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var parentEntity = ref entity.GetComponent<ParentEntity>();
                ref var weaponList = ref parentEntity.Value.GetComponent<MeleeWeapons>();

                weaponList.Value.Add(entity);
            }
        }
    }
}