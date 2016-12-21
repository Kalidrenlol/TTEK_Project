using UnityEngine;
using System.Collections;

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

		if (_xMov != 0 || _zMov != 0) {
			bool isWalkingPressed = true;
			playerAnimator.SetBool ("IsWalking", isWalkingPressed);
		} else {
			bool isWalkingPressed = false;
			playerAnimator.SetBool ("IsWalking", isWalkingPressed);
		}

		if (Input.GetKeyDown ("f")) {
			PushOpponent();
            //Push sound
            GetComponent<Player>().PushSound();
		}

        if (Input.GetKeyDown("x"))
        {

            GetComponent<Player>().PushNew(this.gameObject);
            GetComponent<Player>().isAttacking = true;
            playerAnimator.SetBool("HasAttacked", true);
            StartCoroutine(StopPush());
            //Push sound
            GetComponent<Player>().PushSound();
        }

		Vector3 _velocity = new Vector3 (_xMov, 0, _zMov);
		GameObject cam = Camera.main.gameObject;
		motor.Move(_velocity);

        if (Input.GetKeyDown("g"))
        {
            Debug.Log("Use powerup");
            GetComponent<Player>().ActivatePowerup();
        }

        if (Input.GetKeyDown("r"))
        {
            Debug.Log("Use powerup");
            GetComponent<Player>().PU_ThrowExplosive();
        }

        if (Input.GetKeyDown("t"))
        {
            Debug.Log("Use powerup");
            GetComponent<Player>().PU_PlaceMine();
        }

        if (Input.GetKeyDown("q"))
        {
            //Camera camMain = Camera.main;
            cam.GetComponent<ScreenShake>().InitScreenShake(1,1);
        }

		isOutsideMap ();
	}
		
	public void PushOpponent() {
		GetComponent<Player>().isAttacking = true;
		playerAnimator.SetBool ("HasAttacked", true);
		StartCoroutine(StopPush());
	}


	private IEnumerator StopPush() {
		yield return new WaitForSeconds(0.5f);
		GetComponent<Player>().isAttacking = false;
	}

	void OnCollisionStay(Collision collider) {
		if (collider.gameObject.tag == "Player") {
			//GetComponent<Player>().PushOpponent(collider);
		}
	}

	public void isOutsideMap() {
		if (GetComponent<Player> ().transform.position.y < -0.1) {
			GetComponent<Player> ().Die ();
		}
	}

}