using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdatePlayerMoveDirection : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<PlayerMarker>().With<MoveDirection>().With<PlayerInput_MoveDirection>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var input = ref entity.GetComponent<PlayerInput_MoveDirection>();
                ref var moveDirection = ref entity.GetComponent<MoveDirection>();
                ref var transform = ref entity.GetComponent<TransformRef>();

                moveDirection.Value.x = 0;
                moveDirection.Value.z = 0;

                if (input.Value.x != 0 || input.Value.y != 0)
                {
                    moveDirection.Value += transform.Value.right * input.Value.x + 
                                           transform.Value.forward * input.Value.y;
                }

                moveDirection.Value = moveDirection.Value.normalized;
            }
        }
    }
}