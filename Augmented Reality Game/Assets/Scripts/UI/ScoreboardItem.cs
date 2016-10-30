using UnityEngine;
using UnityEngine.UI;

public class ScoreboardItem : MonoBehaviour {

	[SerializeField] Text usernameText;
	[SerializeField] Text scoreText;

	public void Setup(string _username, int _score, Color _color) {
		usernameText.text = _username;
		scoreText.text = _score.ToString();

		Image backgroundImg = GetComponent<Image>();
		Color _newColor = new Color(_color.r,_color.g, _color.b, 0.4f);
		backgroundImg.color = _newColor;

	}


}
