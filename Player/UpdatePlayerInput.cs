using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdatePlayerInput : ISystem
    {
        private readonly FPSInput _input;
        private Filter _look_Direction;
        private Filter _moveBackward_Pressed;
        private Filter _move_Direction;
        private Filter _moveForward_Pressed;
        private Filter _moveLeft_Pressed;
        private Filter _moveRight_Pressed;
        private Filter _primaryAbility_Pressed;
        private Filter _primaryAttack_Pressed;
        private Filter _secondaryAbility_Pressed;
        private Filter _secondaryAttack_Pressed;
        private Filter _space_Pressed;
        private Filter _sprint_Pressed;
        private Filter _slowTime_WasPressed;

        public UpdatePlayerInput(FPSInput input)
        {
            _input = input;
        }

        public void Dispose()
        { }

        public void OnAwake()
        {
            _primaryAttack_Pressed = World.Filter.With<PlayerInput_PrimaryAttack_IsPressed>();
            _secondaryAttack_Pressed = World.Filter.With<PlayerInput_SecondaryAttack_IsPressed>();
            _secondaryAbility_Pressed = World.Filter.With<PlayerInput_SecondaryAbility_IsPressed>();
            _primaryAbility_Pressed = World.Filter.With<PlayerInput_PrimaryAbility_IsPressed>();
            _slowTime_WasPressed = World.Filter.With<PlayerInput_SlowMo_WasPressed>();

            _moveForward_Pressed = World.Filter.With<PlayerInput_MoveForward_IsPressed>();
            _moveBackward_Pressed = World.Filter.With<PlayerInput_MoveBackward_IsPressed>();
            _moveLeft_Pressed = World.Filter.With<PlayerInput_MoveLeft_IsPressed>();
            _moveRight_Pressed = World.Filter.With<PlayerInput_MoveRight_IsPressed>();
            _space_Pressed = World.Filter.With<PlayerInput_Jump_IsPressed>();
            _sprint_Pressed = World.Filter.With<PlayerInput_Sprint_IsPressed>();

            _move_Direction = World.Filter.With<PlayerInput_MoveDirection>();
            _look_Direction = World.Filter.With<PlayerInput_LookDirection>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            var playerInput = _input.Player;

            foreach (var entity in _primaryAttack_Pressed)
            {
                ref var primaryAttack = ref entity.GetComponent<PlayerInput_PrimaryAttack_IsPressed>();
                primaryAttack.Value = playerInput.PrimaryAttack.IsPressed();
            }

            foreach (var entity in _secondaryAttack_Pressed)
            {
                ref var secondaryAttack = ref entity.GetComponent<PlayerInput_SecondaryAttack_IsPressed>();
                secondaryAttack.Value = playerInput.SecondaryAttack.IsPressed();
            }

            foreach (var entity in _moveForward_Pressed)
            {
                ref var moveForward = ref entity.GetComponent<PlayerInput_MoveForward_IsPressed>();
                moveForward.Value = playerInput.Move.ReadValue<Vector2>().y >= 1f;
            }

            foreach (var entity in _moveBackward_Pressed)
            {
                ref var moveBackward = ref entity.GetComponent<PlayerInput_MoveBackward_IsPressed>();
                moveBackward.Value = playerInput.Move.ReadValue<Vector2>().y <= -1f;
            }

            foreach (var entity in _moveLeft_Pressed)
            {
                ref var moveLeft = ref entity.GetComponent<PlayerInput_MoveLeft_IsPressed>();
                moveLeft.Value = playerInput.Move.ReadValue<Vector2>().x <= -1f;
            }

            foreach (var entity in _moveRight_Pressed)
            {
                ref var moveRight = ref entity.GetComponent<PlayerInput_MoveRight_IsPressed>();
                moveRight.Value = playerInput.Move.ReadValue<Vector2>().x >= 1f;
            }

            foreach (var entity in _secondaryAbility_Pressed)
            {
                ref var secondaryAbility = ref entity.GetComponent<PlayerInput_SecondaryAbility_IsPressed>();
                secondaryAbility.Value = playerInput.SecondaryAbility.IsPressed();
            }

            foreach (var entity in _primaryAbility_Pressed)
            {
                ref var primaryAbility = ref entity.GetComponent<PlayerInput_PrimaryAbility_IsPressed>();
                primaryAbility.Value = playerInput.PrimaryAbility.IsPressed();
            }

            foreach (var entity in _space_Pressed)
            {
                ref var spacePressed = ref entity.GetComponent<PlayerInput_Jump_IsPressed>();
                spacePressed.Value = playerInput.Jump.IsPressed();
            }

            foreach (var entity in _sprint_Pressed)
            {
                ref var sprintPressed = ref entity.GetComponent<PlayerInput_Sprint_IsPressed>();
                sprintPressed.Value = playerInput.Sprint.IsPressed();
            }
            
            foreach (var entity in _move_Direction)
            {
                ref var moveDirection = ref entity.GetComponent<PlayerInput_MoveDirection>();
                moveDirection.Value = playerInput.Move.ReadValue<Vector2>();
            }

            foreach (var entity in _look_Direction)
            {
                ref var lookDirection = ref entity.GetComponent<PlayerInput_LookDirection>();
                lookDirection.Value = playerInput.Look.ReadValue<Vector2>();
            }
            
            foreach (var entity in _slowTime_WasPressed)
            {
                ref var slowTimeWasPressed = ref entity.GetComponent<PlayerInput_SlowMo_WasPressed>();
                slowTimeWasPressed.Value = playerInput.SlowMoAbility.WasPerformedThisFrame();
            }
        }
    }
}