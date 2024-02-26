using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Wheel {
    public GameObject mesh;
    public WheelCollider collider;
}

public class CarMovement : MonoBehaviour
{
    public List<Wheel> frontWheels;
	public List<Wheel> rearWheels;
	public float acceleration;
	public float brakeForce;
	public float turnAngle;

	private CarInputs _carInputs;
	private Vector2 _movement;
	private float currentBrakeForce = 0;

	private void OnEnable() {
		_carInputs.Enable();
	}

	private void OnDisable() {
		_carInputs.Disable();
	}
	private void Awake() {
		Cursor.lockState = CursorLockMode.Locked;
		_carInputs = new CarInputs();

		_carInputs.Car.Move.performed += _ => _movement = _carInputs.Car.Move.ReadValue<Vector2>();
		_carInputs.Car.Move.canceled += _ => _movement = Vector2.zero;

		_carInputs.Car.Brake.started += _ => currentBrakeForce = brakeForce;
		_carInputs.Car.Brake.canceled += _ => currentBrakeForce = 0;
	}

	private void FixedUpdate() {
		foreach (Wheel wheel in frontWheels) {
			wheel.collider.motorTorque = acceleration * _movement.y;
			wheel.collider.brakeTorque = currentBrakeForce;
			wheel.collider.steerAngle = turnAngle * _movement.x;
			RotateWheel(wheel);
		}

		foreach (Wheel wheel in rearWheels) {
			wheel.collider.brakeTorque = currentBrakeForce;
			RotateWheel(wheel);
		}
	}

	private void RotateWheel(Wheel wheel) {
		Vector3 position;
		Quaternion rotation;
		wheel.collider.GetWorldPose(out position, out rotation);
		wheel.mesh.transform.rotation = rotation;
	}
}
