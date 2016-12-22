using UnityEngine;
using UnityEngine.UI;

public class PlayerNamePlate : MonoBehaviour {

	//Canvas
	[SerializeField] Text usernameText;
	[SerializeField] Player player;

	void Update () {
		Camera cam = Camera.main;
		if (cam != null) {
			transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
		}

		usernameText.text = player.username;
	}
}
