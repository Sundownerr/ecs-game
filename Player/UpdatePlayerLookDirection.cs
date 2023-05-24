using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdatePlayerLookDirection : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<TransformRef>().With<LookDirection>().With<LookConfig>()
                .With<CinemachineCameraTargetRef>().With<PlayerInput_LookDirection>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var input = ref entity.GetComponent<PlayerInput_LookDirection>();
                ref var lookDirection = ref entity.GetComponent<LookDirection>();
                ref var lookConfig = ref entity.GetComponent<LookConfig>();
                ref var transform = ref entity.GetComponent<TransformRef>().Value;
                ref var CinemachineCameraTarget = ref entity.GetComponent<CinemachineCameraTargetRef>().Value;

                if (math.lengthsq(input.Value) < 0.01f)
                {
                    continue;
                }

                //Don't multiply mouse input by Time.deltaTime, on gamepad you should multiply by deltatime
                var deltaTimeMultiplier = 1.0f;

                lookDirection.Value.y +=  input.Value.y * lookConfig.RotationSpeed * deltaTimeMultiplier;
                var rotationVelocity = input.Value.x * lookConfig.RotationSpeed * deltaTimeMultiplier;

                lookDirection.Value.y = ClampAngle(lookDirection.Value.y, -89, 89);
                CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(lookDirection.Value.y, 0.0f, 0.0f);
                transform.Rotate(Vector3.up * rotationVelocity);

                float ClampAngle(float lfAngle, float lfMin, float lfMax)
                {
                    if (lfAngle < -360f)
                    {
                        lfAngle += 360f;
                    }

                    if (lfAngle > 360f)
                    {
                        lfAngle -= 360f;
                    }

                    return Mathf.Clamp(lfAngle, lfMin, lfMax);
                }
            }
        }
    }
}