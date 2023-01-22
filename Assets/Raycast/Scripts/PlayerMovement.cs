using UnityEditor;
using UnityEditor.Experimental.GraphView;
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
	private bool isJumping = false;
	private bool _isDoubleJumpPressed = false;
	private float _jumpBufferCounter = 0f;
	private float _jumpHangCounter = 0f;
	private float baseSpeed;

	float y = 0f;

	private void Start() {
		_controller= GetComponent<CharacterController>();
		baseSpeed = speed;
	}

	void Update()
    {
		float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

		gravity = -9.81f * gravityScale;

		if (IsGrounded()) {
			y = gravity;
			isJumping = false;
			_isDoubleJumpPressed = false;

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

			if (_jumpBufferCounter > 0) {
				_jumpBufferCounter -= Time.deltaTime;
			}

			if (_jumpHangCounter > 0) {
				_jumpHangCounter -= Time.deltaTime;
			}
		}

		if (Input.GetKeyDown(KeyCode.Space) && !isJumping) {
			Jump();
		}

		if (Input.GetKey(KeyCode.LeftShift))
			speed = baseSpeed * sprintSpeedMultiplier;
		else
			speed = baseSpeed;

		/* Per far si che il personaggio si fermi esattamente quando si rilascia WASD (quindi senza slittare) invece di
			GetAxis si può usare GetKey per ogni tasto e quando nessuno di questi viene premuto si imposta la velocity a 0*/ 
		_velocity = transform.right * x + transform.forward * z;
		if (_velocity.magnitude > 1)
			_velocity = _velocity.normalized * speed; // senza normalizzarlo muovendosi in diagonale la velocity aumenta
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
		isJumping = true;
		y = Mathf.Sqrt(-2.0f * gravity * maxJumpHeight);
		//Debug.Log(2 * (y / gravity));
		// 2 * (y / gravity); Total jump's time
		// Non c'è bisogno di creare un parametro per controllare la durata del salto perchè questa si può cambiare incrementando o diminuendo la gravità.
	}

	/*private void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		float spherePositionY = transform.position.y - (_controller.height / 2) + _controller.radius - 0.002f - jumpBufferTimer;
		Vector3 spherePosition = new Vector3(transform.position.x, spherePositionY, transform.position.z);
		Gizmos.DrawSphere(spherePosition, _controller.radius - 0.001f);
	}*/
}
