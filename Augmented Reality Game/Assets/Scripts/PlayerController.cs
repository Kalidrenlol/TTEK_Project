using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] private float speed = 5f;

	void Start() {

	}

	void Update() {
		if (PauseMenu.IsOn) {
			return;
		}

		float _xMov = Input.GetAxis("Horizontal");
		float _zMov = Input.GetAxis("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

	}
}
