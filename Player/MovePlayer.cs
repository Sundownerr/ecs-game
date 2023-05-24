using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MovePlayer : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<PlayerMarker>().With<CharacterControllerRef>().With<MoveDirection>().With<VerticalVelocity>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var characterController = ref entity.GetComponent<CharacterControllerRef>();
                ref var moveDirection = ref entity.GetComponent<MoveDirection>();
                ref var verticalVelocity = ref entity.GetComponent<VerticalVelocity>();
                
                characterController.Value.Move(moveDirection.Value + Vector3.up * verticalVelocity.Value * deltaTime);
            }
        }
    }
}