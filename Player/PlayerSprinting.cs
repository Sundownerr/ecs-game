using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerSprinting : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<MoveSpeed>().With<SprintConfig>().With<SprintMarker>()
                .With<PlayerInput_Sprint_IsPressed>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var moveSpeed = ref entity.GetComponent<MoveSpeed>();
                ref var sprintConfig = ref entity.GetComponent<SprintConfig>();
                ref var sprintPressed = ref entity.GetComponent<PlayerInput_Sprint_IsPressed>();

                moveSpeed.Value = math.select(moveSpeed.Value, sprintConfig.SprintSpeed, sprintPressed.Value);
            }
        }
    }
}