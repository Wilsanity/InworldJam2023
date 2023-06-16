using UnityEngine;

public class MovementController : MonoBehaviour
{
	enum MoveState
	{
		None,
		Idle,
		Walk,
		Sprint,
		Crouch,
		Slide,
		Jump,
		Fall
	};

	[SerializeField] private MoveState state;

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
	[SerializeField] private CharacterController controller = null;

	private Vector2 previousInput;

	//Movement
	private float _speed = 4.0f;
	private float _verticalVelocity;
	private float _terminalVelocity = 53.0f;
	public Vector3 momentum = Vector3.zero;

	// timeout deltatime
	private float _jumpTimeoutDelta;
	private float _fallTimeoutDelta;

	private Animator animator;
	public Animator Animator 
	{ 
		get
		{
			if (animator != null) return animator;
			return animator = GetComponent<Animator>();
		} 
	}

	[SerializeField] private bool isSprint;
	[SerializeField] private bool isCrouch;

	private Controls controls;

	private Controls Controls
    {
        get
        {
			if(controls != null) { return controls; }
			return controls = new Controls();
        }
    }
    public void Start()
    {
		enabled = true;

		Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
		Controls.Player.Move.canceled += ctx => ResetMovement();

		Controls.Player.Sprint.performed += ctx => Sprint();

		Controls.Player.Jump.performed += ctx => Jump();

		Controls.Player.Crouch.performed += ctx => StartSlide();

		state = MoveState.Walk;
	}

	private void OnEnable() => Controls.Enable();

	private void OnDisable() => Controls.Disable();

	private void Update()
    {
		GroundedCheck();
		ApplyGravity();

        switch (state)
        {
			case (MoveState.None):
				break;
			case (MoveState.Idle):
				break;
			case (MoveState.Walk):
				Move(MoveSpeed);
				break;
			case (MoveState.Sprint):
				Move(SprintSpeed);
				break;
			case (MoveState.Crouch):
				Move(CrouchSpeed);
				break;
			case (MoveState.Slide):
				Slide();
				break;
			case (MoveState.Fall):
				Fall();
				break;
			default:
				break;
		}

    }

	private void SetMovement(Vector2 movement) => previousInput = movement;

	private void ResetMovement() => previousInput = Vector2.zero;


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

	private void Fall()
    {
		if (Grounded && Time.time >= _fallTimeoutDelta)
		{
			Animator.SetBool("Jump", false);
			state = MoveState.Walk;

			if (isSprint)
			{
				isCrouch = false;
				state = MoveState.Sprint;
			}
			else if (isCrouch)
			{
				isSprint = false;
				state = MoveState.Crouch;
			}
		}
		else
		{
			Move(AirSpeed);

			//Add Momentum
			controller.Move(momentum * Time.deltaTime + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}
    }

	private void Crouch()
	{
		if (state == MoveState.Slide)
			return;

		isCrouch = !isCrouch;
		isSprint = false;

		if (isCrouch)
		{
			state = MoveState.Walk;
		}
		else if (state != MoveState.Fall)
		{
			state = MoveState.Crouch;
		}
	}

	private void Sprint()
	{
		if (state == MoveState.Slide)
			return;

		isSprint = !isSprint;
		isCrouch = false;

		//No longer sprinting - change to walking
		if (!isSprint)
		{
			state = MoveState.Walk;
			return;
		}
		else if (state != MoveState.Fall)
		{
			state = MoveState.Sprint;
		}
	}

	private void Move(float targetSpeed)
    {
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
		// normalise input direction
		Vector3 inputDirection = new Vector3(previousInput.x, 0.0f, previousInput.y).normalized;

		// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is a move input rotate player when the player is moving
		if (inputDirection != Vector3.zero)
		{
			// move
			inputDirection = transform.right * previousInput.x + transform.forward * previousInput.y;
		}

		Animator.SetFloat("Speed", _speed);

		if (state != MoveState.Fall)
		{
			momentum = inputDirection.normalized * (_speed);
			// move the player
			controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}
		else
		{
			momentum += inputDirection.normalized * (_speed * Time.deltaTime);
		}
	}

	private void Jump()
    {
		if (Grounded)
		{
			if (Time.time < _jumpTimeoutDelta)
				return;

			Animator.SetBool("Jump", true);

			//Jump 30% higher after a slide
			float targetJumpHeight = state == MoveState.Slide ? JumpHeight * 1.3f : JumpHeight;

			_verticalVelocity = Mathf.Sqrt(targetJumpHeight * -2f * Gravity);						
			_jumpTimeoutDelta = Time.time + JumpTimeout;
			_fallTimeoutDelta = Time.time + FallTimeout;

			state = MoveState.Fall;
		}
	}

	float slideSpeed = 20.0f;
	float slideLength = 2.0f;
	float currentslideSpeed;

	float slideTimeDelta;

	private void Slide()
	{
		if (slideTimeDelta >= slideLength)
		{
			EndSlide();
		}

		float speed = Mathf.Lerp(slideSpeed, 1.0f, slideTimeDelta/slideLength);

		//lerp slide speed -> 0 based on legth of slide

		Vector3 targetSpeed = transform.forward * speed * Time.deltaTime;

		controller.Move(targetSpeed + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		slideTimeDelta += Time.deltaTime;

		if (state != MoveState.Fall)
			momentum = targetSpeed * 100;
	}

	private void EndSlide()
	{
		state = MoveState.Crouch;
	}

	private void StartSlide()
	{
		if (state != MoveState.Sprint)
			return;

		slideTimeDelta = 0;
		state = MoveState.Slide;
	}
}
