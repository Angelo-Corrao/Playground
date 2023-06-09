using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivityX = 100f;
    public float mouseSensitivityY = 100f;
    public Transform player;

    private float _xRotationMin = -90;
    private float _xRotationMax = 90;

	float xRotation = 0;

	void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
	}

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;

        player.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, _xRotationMin, _xRotationMax);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
