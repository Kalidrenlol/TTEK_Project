using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	//[SerializeField] private float lookSensitivity = 3f;

	private string virtualJoystickTag = "VirtualJoystick";
	private GameObject joystick;
	private PlayerMotor motor;
	Animator playerAnimator;
    float lastCamAngle = 180;

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
            cam.Rotate(Vector3.down, 100 * Time.deltaTime, Space.World);
            
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            cam.Rotate(Vector3.up, 100 * Time.deltaTime, Space.World);
        }
        //float camAngle  = Vector3.Angle(camBase, camDir);
        float camAngle = Mathf.Atan2(camDir.y-camBase.y, camDir.x-camBase.x)*180 / Mathf.PI;
        if (camAngle != lastCamAngle)
        {
            Debug.Log("Angle: " + camAngle);
            lastCamAngle = camAngle;
        }

         Vector3 pos = camBase;
         Vector3 dir = (camBase - camDir).normalized;
         Debug.Log("Dir: "+dir);
         Debug.DrawLine(pos, pos + dir * -20, Color.red, 5);

        

        // Rotatet _velocity to align with camAngle


        Debug.DrawRay(camBase, camDir, Color.green,5,false);
        //_velocity = Quaternion.Euler(0, camAngle+180, 0) * _velocity;
		motor.Move(_velocity, dir);


		/*		float _yRot = Input.GetAxisRaw("Mouse X");
		Vector3 _rotation = new Vector3(0,_yRot, 0) * lookSensitivity;
		motor.Rotate(_rotation);

		float _xRot = Input.GetAxisRaw("Mouse Y");
		float _cameraRotationX = _xRot * lookSensitivity;
		motor.cameraRotate(_cameraRotationX);
*/

	}

	public void PushOpponent() {
		Debug.Log ("Skubber modstander");
		playerAnimator.SetBool ("HasAttacked", true);
	}

	void OnCollisionStay(Collision collider) {
		

		if (collider.gameObject.tag == "Player") {
			if (playerAnimator.GetBool ("HasAttacked") == true) {
				Vector3 dir = (transform.position - collider.transform.position).normalized;
				collider.gameObject.GetComponent<Rigidbody> ().AddForce (-dir * 500f);
				Debug.Log ("Force added");
			}
		}
	}

}
