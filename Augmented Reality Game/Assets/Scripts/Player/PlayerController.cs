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
        Vector3 camVec = cam.forward;
        //Debug.Log("v: " + _velocity);
        //Debug.Log("c: " + camVec); // Bevæg i forhold til X og Z (X, Y, Z)
        Vector3 camBase = new Vector3(cam.position.x, 0.2f, cam.position.z);
        Vector3 camDir  = new Vector3(camVec.x*100,0.2f,camVec.z*100);
        if (Input.GetKeyDown(KeyCode.M))
        {
            cam.Rotate(Vector3.down, 100 * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            cam.Rotate(Vector3.up, 100 * Time.deltaTime);
        }
        float camAngle  = Vector3.Angle(camBase, camDir);
//        Debug.Log("Angle: " + camAngle);

        // Rotatet _velocity to align with camAngle


        Debug.DrawRay(camBase, camDir, Color.green,5,false);
        _velocity = Quaternion.Euler(0, camAngle+180, 0) * _velocity;
		motor.Move(_velocity);

=======
>>>>>>> 035260f495a9be66c260e4edf0f0d8da8f18f9b7

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