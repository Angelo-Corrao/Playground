using UnityEngine;

public class ThirdPersonController : MonoBehaviour {
	public float speed = 10f;
	public float sprintSpeedMultiplier = 1.5f;
	public float gravityScale = 1f;
	public float maxJumpHeight = 2f;
	public float jumpBufferTimer = 0.15f;
	public float jumpHangTimer = 0.1f;
	public LayerMask _groundMask;
	public Transform body;
	public float rotationSpeed = 5.0f;

	private CharacterController _controller;
	private Vector3 _velocity;
	private float gravity;
	private float _maxJumpTime;
	private bool _isMaxJumpHeightReached = false;
	private bool _isDoubleJumpPressed = false;
	private float _jumpBufferCounter = 0f;
	private float _jumpHangCounter = 0f;
	private float _jumpTimeCounter = 0f;
	private float baseSpeed;
	private float y = 0f;
	private Camera mainCam;

	private void Start() {
		_controller = GetComponent<CharacterController>();
		baseSpeed = speed;
		gravity = -9.81f * gravityScale;
		mainCam = Camera.main;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update() {
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		if (IsGrounded()) {
			y = gravity;
			_isDoubleJumpPressed = false;
			_isMaxJumpHeightReached = false;

			if (Input.GetKeyDown(KeyCode.Space)) {
				Jump();
			}

			if (_jumpBufferCounter > 0) {
				Jump();
				_jumpBufferCounter = 0f;
			}

			_jumpHangCounter = jumpHangTimer;
		}
		else {
			y += gravity * Time.deltaTime;

			if (Input.GetKeyDown(KeyCode.Space)) {
				if (_jumpHangCounter <= 0) {
					if (!_isDoubleJumpPressed) {
						Jump();
						_isDoubleJumpPressed = true;
					}
					else {
						_jumpBufferCounter = jumpBufferTimer;
					}
				}
				else {
					Jump();
					_jumpHangCounter = 0f;
				}
			}

			if (Input.GetKeyUp(KeyCode.Space) && !_isMaxJumpHeightReached) {
				y = 0f;
				_isMaxJumpHeightReached = true;
			}

			if (_jumpBufferCounter > 0) {
				_jumpBufferCounter -= Time.deltaTime;
			}

			if (_jumpHangCounter > 0) {
				_jumpHangCounter -= Time.deltaTime;
			}

			if (_jumpTimeCounter >= _maxJumpTime) {
				_isMaxJumpHeightReached = true;
			}
			else {
				_jumpTimeCounter += Time.deltaTime;
			}
		}

		if (Input.GetKey(KeyCode.LeftShift))
			speed = baseSpeed * sprintSpeedMultiplier;
		else
			speed = baseSpeed;

		_velocity = transform.right * x + transform.forward * z;
		Vector3 movementToCamSpace = RotatePlayerVector(_velocity);
		if (movementToCamSpace.x != 0 || movementToCamSpace.z != 0)
			RotatePlayer(movementToCamSpace);

		if (movementToCamSpace.magnitude > 1)
			movementToCamSpace = movementToCamSpace.normalized * speed;
		else
			movementToCamSpace *= speed;
		movementToCamSpace.y = y;

		_controller.Move(movementToCamSpace * Time.deltaTime);
	}

	bool IsGrounded() {
		float spherePositionY = transform.position.y - (_controller.height / 2) + _controller.radius - 0.002f;
		Vector3 spherePosition = new Vector3(transform.position.x, spherePositionY, transform.position.z);
		return Physics.CheckSphere(spherePosition, _controller.radius - 0.001f, _groundMask);
	}

	void Jump() {
		y = Mathf.Sqrt(-2.0f * gravity * maxJumpHeight);
		_maxJumpTime = -(y / gravity);
		_jumpTimeCounter = 0;
		_isMaxJumpHeightReached = false;
	}

	Vector3 RotatePlayerVector(Vector3 playerDirection) {
		Vector3 camForward = mainCam.transform.forward;
		Vector3 camRight = mainCam.transform.right;

		Vector3 movementToCamSpace = playerDirection.x * camRight + playerDirection.z * camForward;

		return movementToCamSpace;
	}

	void RotatePlayer(Vector3 movementToCamSpace) {
		float targetLocalAngle = Mathf.Atan2(_velocity.x, _velocity.z) * Mathf.Rad2Deg;
		float targetCameraAngle = mainCam.transform.localEulerAngles.y;
		float targetAngle = targetLocalAngle + targetCameraAngle;
		float targetAngleInterpolated = Mathf.LerpAngle(body.localEulerAngles.y, targetAngle, Time.deltaTime * rotationSpeed);
		body.localRotation = Quaternion.Euler(body.localEulerAngles.x, targetAngleInterpolated, body.localEulerAngles.z);
	}

	/*private void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		float spherePositionY = transform.position.y - (_controller.height / 2) + _controller.radius - 0.002f - jumpBufferTimer;
		Vector3 spherePosition = new Vector3(transform.position.x, spherePositionY, transform.position.z);
		Gizmos.DrawSphere(spherePosition, _controller.radius - 0.001f);
	}*/
}
