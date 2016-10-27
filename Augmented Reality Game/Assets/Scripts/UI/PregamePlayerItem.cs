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

		Image backgroundImg = background.GetComponent<Image>();
		Color _newColor = new Color(_color.r,_color.g, _color.b, 0.4f);
		backgroundImg.color = _newColor;

	}

}
