using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	//[SerializeField] private float lookSensitivity = 3f;

	private string virtualJoystickTag = "VirtualJoystick";
	private GameObject joystick;
	private PlayerMotor motor;
	Animator playerAnimator;

	void Start() {
		playerAnimator = transform.FindDeepChild("Character").GetComponent<Animator> ();
		motor = GetComponent<PlayerMotor>();
		joystick = GameObject.FindGameObjectWithTag(virtualJoystickTag);
	}

	void Update() {

		if (PauseMenu.IsOn) {
			return;
		}

		float _xMov = joystick.GetComponent<VirtualJoystick>().Horizontal();
		float _zMov = joystick.GetComponent<VirtualJoystick>().Vertical();

		if (_xMov > 0.1 && _zMov > 0.1 || _xMov < -0.1 && _zMov > 0.1 || _xMov > 0.1 && _zMov < -0.1 || _xMov < -0.1 && _zMov < -0.1) {
			bool isWalkingPressed = true;
			playerAnimator.SetBool ("IsWalking", isWalkingPressed);
		} else {
			bool isWalkingPressed = false;
			playerAnimator.SetBool ("IsWalking", isWalkingPressed);
		}

		if (Input.GetKeyDown ("f")) {
			bool hasAttackPressed = true;
			playerAnimator.SetBool ("HasAttacked", hasAttackPressed);
		}

		Vector3 _velocity = new Vector3 (_xMov, 0, _zMov);
        var cam = Camera.main.transform;

		motor.Move(_velocity);
        

	}

	public void PushOpponent() {
		Debug.Log ("Skubber modstander");
		playerAnimator.SetBool ("HasAttacked", true);
	}

	void OnCollisionStay(Collision collider) {
		if (collider.gameObject.tag == "Player") {
			GetComponent<Player>().PushOpponent(collider);
		}
	}

}