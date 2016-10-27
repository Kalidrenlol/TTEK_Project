using UnityEngine;
using UnityEngine.UI;

public class PrescenePlayerItem : MonoBehaviour {

	[SerializeField] Text usernameText;
	[SerializeField] Text readyText;

	public void Setup(string _username, bool _isReady) {
		usernameText.text = _username;
		readyText.text = (_isReady) ? "Ready" : "Not Ready";
	}

}
