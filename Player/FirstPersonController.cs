using UnityEngine;
#if ENABLE_INPUT_SYSTEM
#endif

namespace Game
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        private const float _threshold = 0.01f;
        [SerializeField] public CharacterController _controller;
        [Header("Player")] [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 4.0f;
        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 6.0f;
        [Tooltip("Rotation speed of the character")]
        public float RotationSpeed = 1.0f;
        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)] [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.1f;
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;
        [Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;
        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.5f;
        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 90.0f;
        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -90.0f;
        private readonly float _terminalVelocity = 53.0f;

        private float _cinemachineTargetPitch;
        private float _fallTimeoutDelta;
        private FPSInput _input;
        private float _jumpTimeoutDelta;

        private float _rotationVelocity;
        private float _speed;
        private float _verticalVelocity;

        private void OnDrawGizmosSelected()
        {
            var transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            var transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded)
            {
                Gizmos.color = transparentGreen;
            }
            else
            {
                Gizmos.color = transparentRed;
            }

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        public void CustomUpdate(float deltaTime)
        {
            UpdateTimers(deltaTime);
            
            var jump = _input.Player.Jump;
            var sprint = _input.Player.Sprint;
            var lookValue = _input.Player.Look.ReadValue<Vector2>();
            var moveInputValue = _input.Player.Move.ReadValue<Vector2>();

            // if (jump.IsPressed() && Grounded && _jumpTimeoutDelta <= 0.0f)
            // {
            //     _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            // }

            // if (lookValue.sqrMagnitude >= _threshold)
            // {
            //     CameraRotation(lookValue);
            // }

            // GroundedCheck();

            // var targetSpeed = moveInputValue == Vector2.zero ? 0.0f :
            //     sprint.IsPressed() ? SprintSpeed : MoveSpeed;

            // ChangeSpeed(moveInputValue.magnitude, targetSpeed, deltaTime);
            // Move(moveInputValue, _speed, deltaTime);
            // ApplyGravity(deltaTime);
        }

        public void Construct(FPSInput input)
        {
            _input = input;

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            var position = transform.position;
            position.y -= GroundedOffset;

            Grounded = Physics.CheckSphere(position, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
        }

        private void CameraRotation(Vector2 lookValue)
        {
            //Don't multiply mouse input by Time.deltaTime, on gamepad you should multiply by deltatime
            var deltaTimeMultiplier = 1.0f;

            _cinemachineTargetPitch += lookValue.y * RotationSpeed * deltaTimeMultiplier;
            _rotationVelocity = lookValue.x * RotationSpeed * deltaTimeMultiplier;

            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
            transform.Rotate(Vector3.up * _rotationVelocity);

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

        private void Move(Vector2 inputDirection, float moveSpeed, float deltaTime)
        {
            var verticalDirection = Vector3.up * _verticalVelocity * deltaTime;
            var moveDirection = Vector3.zero;

            if (inputDirection != Vector2.zero)
            {
                moveDirection = transform.right * inputDirection.x + transform.forward * inputDirection.y;
            }

            moveDirection = moveDirection.normalized * (moveSpeed * deltaTime);

            _controller.Move(moveDirection + verticalDirection);
        }

        private void ChangeSpeed(float moveMagnitude, float targetSpeed, float deltaTime)
        {
            var currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            var speedOffset = 0.1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * moveMagnitude,
                    deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }
        }

        public void UpdateTimers(float deltaTime)
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= deltaTime;
                }
            }
        }

        public void ApplyGravity(float deltaTime)
        {
            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * deltaTime;
            }

            if (Grounded)
            {
                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
            }
        }

    }
}