using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PrescenePlayer : NetworkBehaviour {

	[SerializeField] GameObject playerPrefab;
	[SerializeField] GameObject gameManager;
	[SerializeField] GameObject presceneUI;

	public string username;
	public bool isReady = false;
	public string index;

	void Start() {
		RegisterPlayer();
		presceneUI.GetComponent<PresceneManager>().RefreshPlayerList();

		string _username = "Loading..."; // Imens username bliver hentet til klienter

		/*if () {
			Hvis username er indtastet..	
		} else {
			
		}*/

		_username = transform.name;
		username = _username;
		CmdSetUsername(transform.name, _username);
	}

	public void SetReady(bool _isReady) {
		isReady = _isReady;
	}

	public void SetIndex(string _index) {
		index = _index;
	}

	void RegisterPlayer() {
		PrescenePlayer _player = GetComponent<PrescenePlayer>();
		GameManager.RegisterPrescenePlayer(index, _player);

	}

	[Command]
	void CmdSetUsername (string _playerID, string _username) {
		PrescenePlayer player = GameManager.GetPrescenePlayer(_playerID);
		if (player != null) {
			player.username = _username;
		}
	}

}
