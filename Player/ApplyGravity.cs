using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ApplyGravity : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<VerticalVelocity>().With<Gravity>().With<IsGrounded>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var verticalVelocity = ref entity.GetComponent<VerticalVelocity>();
                ref var gravity = ref entity.GetComponent<Gravity>();
                ref var isGrounded = ref entity.GetComponent<IsGrounded>();

                if (!isGrounded.Value && verticalVelocity.Value < 53f)
                {
                    verticalVelocity.Value += gravity.Value * deltaTime;
                }

                if (isGrounded.Value && verticalVelocity.Value < 0.0f)
                {
                    // stop our velocity dropping infinitely when grounded
                    verticalVelocity.Value = -2f;
                }
            }
        }
    }
}