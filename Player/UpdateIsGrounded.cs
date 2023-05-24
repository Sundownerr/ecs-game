using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateIsGrounded : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<TransformRef>().With<IsGrounded>().With<IsGroundedSettings>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transform = ref entity.GetComponent<TransformRef>();
                ref var isGrounded = ref entity.GetComponent<IsGrounded>();
                ref var settings = ref entity.GetComponent<IsGroundedSettings>();
                
                // set sphere position, with offset
                var position = transform.Value.position;
                position.y -= settings.Offset;

                isGrounded.Value = Physics.CheckSphere(position, settings.Radius, settings.GroundMask,
                    QueryTriggerInteraction.Ignore);
            }
        }
    }
}