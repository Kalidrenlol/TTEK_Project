using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField] public float speed;
    private Quaternion facinng;

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
			rb.MovePosition(rb.position + velocity * speed * Time.fixedDeltaTime);
            Transform playerGraphics = rb.transform.GetChild(0); // Gets the "Graphic" child of the player object
            playerGraphics.rotation = Quaternion.LookRotation(velocity*-1); // Rotates the graphic in the velocity direction, reverse the vel to turn the right direction
		}
	}


}
