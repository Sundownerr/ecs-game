using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Flying : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<VerticalVelocity>().With<Flying_Marker>().With<FlyingConfig>()
                .With<PlayerInput_Jump_IsPressed>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var verticalVelocity = ref entity.GetComponent<VerticalVelocity>();
                ref var flyingConfig = ref entity.GetComponent<FlyingConfig>();
                ref var jumpPressed = ref entity.GetComponent<PlayerInput_Jump_IsPressed>();

                if (jumpPressed.Value)
                {
                    verticalVelocity.Value += flyingConfig.AscendSpeed * deltaTime;
                }
                
            }
        }
    }
}