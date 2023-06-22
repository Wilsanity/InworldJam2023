using UnityEngine;

public class MovementController : MonoBehaviour
{
	[Header("Player")]
	[Tooltip("Move speed of the character in m/s")]
	[SerializeField] private float MoveSpeed = 4.0f;
	[Tooltip("Sprint speed of the character in m/s")]
	[SerializeField] private float SprintSpeed = 6.0f;
	[Tooltip("Crouch speed of the character in m/s")]
	[SerializeField] private float CrouchSpeed = 2.0f;
	[Tooltip("Speed of the character in in mid-air in m/s")]
	[SerializeField] private float AirSpeed = 2.0f;
	[Tooltip("Acceleration and deceleration")]
	[SerializeField] private float SpeedChangeRate = 10.0f;
	[Tooltip("Rotation speed of the character model")]
	[SerializeField] private float RotationSpeed = 10.0f;

	[Space(10)]
	[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
	[SerializeField] private float JumpTimeout = 0.1f;
	[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
	[SerializeField] private float FallTimeout = 0.15f;

	[Header("Player Grounded")]
	[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
	[SerializeField] private bool Grounded = true;
	[Tooltip("Useful for rough ground")]
	[SerializeField] private float GroundedOffset = -0.14f;
	[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
	[SerializeField] private float GroundedRadius = 0.5f;
	[Tooltip("What layers the character uses as ground")]
	[SerializeField] private LayerMask GroundLayers;

	[Space(10)]
	[Tooltip("The height the player can jump")]
	[SerializeField] private float JumpHeight = 1.2f;
	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	[SerializeField] private float Gravity = -15.0f;

	[Header("Character Controller")]
	[SerializeField] public CharacterController controller = null;

	[Header("Movement")]
	private float _speed = 4.0f;
	private float _verticalVelocity;
	private float _terminalVelocity = 53.0f;

	public AnimatorHandler animatorHandler;
	public InputHandler inputHandler;
	
	private void Update()
    {
		GroundedCheck();
		ApplyGravity();

		if (inputHandler.isInteracting)
			return;

		if (animatorHandler.canRotate)
			HandleRotation();

		HandleMovement(MoveSpeed);
    }

	private void GroundedCheck()
	{
		// set sphere position, with offset
		Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
		Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
	}


	private void ApplyGravity()
    {
		// apply gravity over time if under terminal
		if (_verticalVelocity < _terminalVelocity)
		{
			_verticalVelocity += Gravity * Time.deltaTime;
		}
	}

	private void HandleMovement(float targetSpeed)
    {
		Vector2 previousInput = inputHandler.movementInput; 

		if (previousInput == Vector2.zero)
			targetSpeed = 0.0f;

		float currentHorizontalSpeed = new Vector3(previousInput.x, 0.0f, previousInput.y).magnitude;

		float speedOffset = 0.1f;

		// accelerate or decelerate to target speed
		if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
		{
			// creates curved result rather than a linear one giving a more organic speed change
			// note T in Lerp is clamped, so we don't need to clamp our speed
			_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);

			// round speed to 3 decimal places
			_speed = Mathf.Round(_speed * 1000f) / 1000f;
		}
		else
		{
			_speed = targetSpeed;
		}

		controller.Move(transform.forward * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

		animatorHandler.UpdateAnimatorvalues(_verticalVelocity, previousInput.magnitude);		
	}
	
	public void HandleRotation()
    {
		Vector2 previousInput = inputHandler.movementInput;

		Vector3 targetDir = Camera.main.transform.forward * previousInput.y;
        targetDir += Camera.main.transform.right * previousInput.x;

		targetDir.Normalize();
		targetDir.y = 0;

		if (targetDir == Vector3.zero)
			targetDir = transform.forward;

		float rs = RotationSpeed;

		Quaternion tr = Quaternion.LookRotation(targetDir);
		Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * Time.deltaTime);

		transform.rotation = targetRotation;
	}

	public void AddKnockback(Vector3 impactLocation, int strength)
    {
		Vector3 dir = transform.position - impactLocation;
		dir.Normalize();

		dir.y = 0.5f;

		_verticalVelocity = -5f;

		GetComponent<ImpactReceiver>().AddImpact(dir, strength);
	}
}
