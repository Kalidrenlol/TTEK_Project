using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField] private float speed;

	private Vector3 velocity = Vector3.zero;
//	private Vector3 rotation = Vector3.zero;
	private VirtualJoystick joystick;
	private Rigidbody rb;


//	private float cameraRotationX = 0f;
//	private float currentCameraRotationX = 0f;
//	[SerializeField] private float cameraRotationLimit = 85f;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	public void Move(Vector3 _velocity) {
		velocity = _velocity;
	}

/*	public void Rotate(Vector3 _rotation) {
		rotation = _rotation;
	}

/*	public void cameraRotate(float _cameraRotationX) {
		cameraRotationX = _cameraRotationX;
	}

*/
	void FixedUpdate() {
		PerformMovement();
		//PerformRotation();
	}

	void PerformMovement() {
		if (velocity != Vector3.zero) {
			rb.MovePosition(rb.position + velocity * speed * Time.fixedDeltaTime);
		}
	}

/*	void PerformRotation() {
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		if (cam != null) {
			currentCameraRotationX -= cameraRotationX;
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

			cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
		}
	}
*/
}
