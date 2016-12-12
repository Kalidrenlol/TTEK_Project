using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField] private float speed;

	private Vector3 velocity = Vector3.zero;
	private VirtualJoystick joystick;
	private Rigidbody rb;


	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	public void Move(Vector3 _velocity) {
		velocity = _velocity;
	}
		
	void FixedUpdate() {
		PerformMovement();
	}

	void PerformMovement() {
		if (velocity != Vector3.zero) {
/*			GameObject _player = gameObject;
			GameObject _camera = GameObject.FindGameObjectWithTag("MainCamera");
			Vector3 _cameraPos = _camera.transform.rotation.eulerAngles;
			Vector3 _relative = _player.transform.rotation.eulerAngles;
			Debug.Log("Camera: "+_cameraPos.normalized);
*/

			rb.MovePosition(rb.position + velocity * speed * Time.fixedDeltaTime);
		}
	}

}
