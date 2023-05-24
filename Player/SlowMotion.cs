using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SlowMotion : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<SlowMotionConfig>().With<SlowMotionToggle>().With<PlayerInput_SlowMo_WasPressed>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var slowMotionConfig = ref entity.GetComponent<SlowMotionConfig>();
                ref var slowMotionToggle = ref entity.GetComponent<SlowMotionToggle>();
                ref var wasPressed = ref entity.GetComponent<PlayerInput_SlowMo_WasPressed>();

                if (wasPressed.Value)
                {
                    slowMotionToggle.Value = !slowMotionToggle.Value;
                    Time.timeScale = slowMotionToggle.Value ? slowMotionConfig.TimeScale : 1f;
                }
            }
        }
    }
}