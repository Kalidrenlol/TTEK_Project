using UnityEngine;
using UnityEngine.UI;

public class EndgamePlayerItem : MonoBehaviour {

	[SerializeField] GameObject background;
	[SerializeField] Image silhouette;
	[SerializeField] Text usernameText;
	[SerializeField] Text posText;
	[SerializeField] Text pointText;

	[SerializeField] Sprite[] playerImageArray;

	public void Setup(string _username, int _pos, Color _color, bool _isLocal, int _score) {

		if (_isLocal) {
			usernameText.color = Color.black;
			pointText.color = Color.black;
		} else {
			usernameText.color = Color.white;
			pointText.color = Color.white;
		}

		usernameText.text = _username;
		pointText.text = _score.ToString();
		posText.text = _pos.ToString();

		Image backgroundImg = background.GetComponent<Image>();
		Color _newColor = new Color(_color.r,_color.g, _color.b, 0.4f);
		backgroundImg.color = _newColor;

	}

	public void NoPlayer() {
		usernameText.text = "";
		pointText.gameObject.SetActive(false);
		silhouette.gameObject.SetActive(false);
		posText.text = "";

		Image backgroundImg = background.GetComponent<Image>();
		Color _newColor = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.4f);
		backgroundImg.color = _newColor;

	}

}
