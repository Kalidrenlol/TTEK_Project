using UnityEngine;
using UnityEngine.UI;

public class ScoreboardItem : MonoBehaviour {

	[SerializeField] Text usernameText;
	[SerializeField] Text scoreText;

	public void Setup(string _username, int _score) {
		usernameText.text = _username;
		scoreText.text = _score.ToString();
	}


}
