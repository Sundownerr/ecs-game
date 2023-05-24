using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Dash : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<DashAbilityMarker>()
                .With<CharacterControllerRef>()
                .With<TransformRef>()
                .With<PlayerInput_Sprint_IsPressed>().With<IsUsable>().With<PlayerInput_MoveDirection>()
                .With<DashAbilitySettings>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var characterControllerRef = ref entity.GetComponent<CharacterControllerRef>();
                ref var sprintPressed = ref entity.GetComponent<PlayerInput_Sprint_IsPressed>();
                ref var isUsable = ref entity.GetComponent<IsUsable>();
                ref var moveDirection = ref entity.GetComponent<PlayerInput_MoveDirection>();
                ref var dashAbilitySettings = ref entity.GetComponent<DashAbilitySettings>();
                ref var transformRef = ref entity.GetComponent<TransformRef>();

                if (sprintPressed.Value && isUsable.Value)
                {
                    var direction = transformRef.Value.right * moveDirection.Value.x +
                                    transformRef.Value.forward * moveDirection.Value.y;
                    direction = direction.normalized;

                    characterControllerRef.Value.Move(direction * dashAbilitySettings.Force * deltaTime);

                    isUsable.Value = false;
                }
            }
        }
    }
}