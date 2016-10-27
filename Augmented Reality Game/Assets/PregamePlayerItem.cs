using UnityEngine;
using UnityEngine.UI;

public class PregamePlayerItem : MonoBehaviour {

	[SerializeField] Text usernameText;
	[SerializeField] Text statusText;
	[SerializeField] GameObject background;

	public void Setup(string _username, bool _ready, Color _color) {
		usernameText.text = _username;
		if (_ready) {
			statusText.text = "Ready";
		} else {
			statusText.text = "Not Ready";
		}
		background.GetComponent<Image>().material.color = _color;
	}
}
