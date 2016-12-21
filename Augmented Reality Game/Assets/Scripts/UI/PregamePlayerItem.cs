using UnityEngine;
using UnityEngine.UI;

public class PregamePlayerItem : MonoBehaviour {

	[SerializeField] Text usernameText;
	[SerializeField] Text statusText;
	[SerializeField] GameObject background;
	[SerializeField] Image silhouette;

	[SerializeField] Sprite[] playerImageArray;

	public void Setup(string _username, bool _ready, Color _color, bool _isLocal, int _no) {

		if (_isLocal) {
			usernameText.color = Color.black;
			statusText.color = Color.black;
		} else {
			usernameText.color = Color.white;
			statusText.color = Color.white;
		}
			
		usernameText.text = _username;

		if (_ready) {
			statusText.text = "Ready";
		} else {
			statusText.text = "Not Ready";
		}

		Image backgroundImg = background.GetComponent<Image>();
		Color _newColor = new Color(_color.r,_color.g, _color.b, 0.4f);
		backgroundImg.color = _newColor;

		int _noIndex = _no - 1;
		silhouette.sprite = playerImageArray[_noIndex];
	}

	public void NoPlayer() {
		usernameText.text = "Ledig plads";
		usernameText.fontStyle = FontStyle.Italic;
		usernameText.alignment = TextAnchor.MiddleCenter;
		statusText.gameObject.SetActive(false);
		silhouette.gameObject.SetActive(false);

		Image backgroundImg = background.GetComponent<Image>();
		Color _newColor = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.4f);
		backgroundImg.color = _newColor;

	}

}
