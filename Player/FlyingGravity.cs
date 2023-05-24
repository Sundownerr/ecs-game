using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FlyingGravity : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<VerticalVelocity>().With<Flying_Marker>()
                .With<PlayerInput_Jump_IsPressed>().With<Gravity>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var verticalVelocity = ref entity.GetComponent<VerticalVelocity>();
                ref var jumpPressed = ref entity.GetComponent<PlayerInput_Jump_IsPressed>();
                ref var gravity = ref entity.GetComponent<Gravity>();

                if (jumpPressed.Value)
                {
                    verticalVelocity.Value += Mathf.Abs(gravity.Value)*0.5f * deltaTime;
                }
            }
        }
    }
}