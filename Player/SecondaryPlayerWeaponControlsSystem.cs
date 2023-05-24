using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SecondaryPlayerWeaponControls : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<WeaponRef>().With<SecondaryMarker>().With<IsUsable>()
                .With<PlayerInput_SecondaryAttack_IsPressed>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var primaryAttack = ref entity.GetComponent<PlayerInput_SecondaryAttack_IsPressed>();
                ref var isUsable = ref entity.GetComponent<IsUsable>();

                if (isUsable.Value && primaryAttack.Value)
                {
                    ref var weapon = ref entity.GetComponent<WeaponRef>();
                    weapon.Value.Use();

                    isUsable.Value = false;
                }
            }
        }
    }
}