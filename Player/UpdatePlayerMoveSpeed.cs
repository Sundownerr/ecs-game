using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdatePlayerMoveSpeed : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<PlayerInput_MoveDirection>().With<MoveSpeed>().With<PlayerMoveSpeedSettings>()
                .With<CharacterControllerRef>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var moveDirection = ref entity.GetComponent<PlayerInput_MoveDirection>();
                ref var characterController = ref entity.GetComponent<CharacterControllerRef>();
                ref var moveSpeed = ref entity.GetComponent<MoveSpeed>();
                ref var moveSpeedSettings = ref entity.GetComponent<PlayerMoveSpeedSettings>();

                var moveMagnitude = math.length(moveDirection.Value);
                var targetSpeed = moveMagnitude == 0 ? 0.0f : moveSpeedSettings.Normal;

                var velocity = characterController.Value.velocity;
                var currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;
                var speedOffset = 0.1f;

                if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                    currentHorizontalSpeed > targetSpeed + speedOffset)
                {
                    moveSpeed.Value = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * moveMagnitude,
                        deltaTime * moveSpeedSettings.ChangeRate);

                    // round speed to 3 decimal places
                    moveSpeed.Value = Mathf.Round(moveSpeed.Value * 1000f) / 1000f;
                }
                else
                {
                    moveSpeed.Value = targetSpeed;
                }
            }
        }
    }
}