using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Jumping : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<VerticalVelocity>().With<JumpSettings>().With<Gravity>().With<IsGrounded>()
                .With<PlayerInput_Jump_IsPressed>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var verticalVelocity = ref entity.GetComponent<VerticalVelocity>();
                ref var gravity = ref entity.GetComponent<Gravity>();
                ref var isGrounded = ref entity.GetComponent<IsGrounded>();
                ref var jump = ref entity.GetComponent<PlayerInput_Jump_IsPressed>();
                ref var jumpSettings = ref entity.GetComponent<JumpSettings>();
                
                if (jump.Value && isGrounded.Value)
                {
                    verticalVelocity.Value = Mathf.Sqrt(jumpSettings.Height * -2f * gravity.Value);
                }
            }
        }
    }
}