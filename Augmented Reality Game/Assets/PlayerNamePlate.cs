using UnityEngine;
using UnityEngine.UI;

public class PlayerNamePlate : MonoBehaviour {

	[SerializeField] private Text usernameText;
	[SerializeField] private Player player;

	void Update() {
		usernameText.text = player.username;

		Camera cam = Camera.main;
		if (cam != null) {
			transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
		}
		
	}

}
