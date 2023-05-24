using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ApplyMoveSpeed : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<MoveDirection>().With<MoveSpeed>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var moveDirection = ref entity.GetComponent<MoveDirection>();
                ref var moveSpeed = ref entity.GetComponent<MoveSpeed>();

                moveDirection.Value *= moveSpeed.Value * deltaTime;
            }
        }
    }
}