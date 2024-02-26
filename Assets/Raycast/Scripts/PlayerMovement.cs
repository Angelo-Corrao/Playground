using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
	public float sprintSpeedMultiplier = 0.5f;
    public float gravityScale = 1f;
	public float maxJumpHeight = 2f;
	public float jumpBufferTimer = 0.15f;
	public float jumpHangTimer = 0.1f;
	public LayerMask _groundMask;

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

	float y = 0f;

	private void Start() {
		_controller= GetComponent<CharacterController>();
		baseSpeed = speed;
		gravity = -9.81f * gravityScale;
	}

	void Update()
    {
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
		if (_velocity.magnitude > 1)
			_velocity = _velocity.normalized * speed;
		else
			_velocity *= speed;
		_velocity.y = y;

		_controller.Move(_velocity * Time.deltaTime);
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

	/*private void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		float spherePositionY = transform.position.y - (_controller.height / 2) + _controller.radius - 0.002f - jumpBufferTimer;
		Vector3 spherePosition = new Vector3(transform.position.x, spherePositionY, transform.position.z);
		Gizmos.DrawSphere(spherePosition, _controller.radius - 0.001f);
	}*/
}
