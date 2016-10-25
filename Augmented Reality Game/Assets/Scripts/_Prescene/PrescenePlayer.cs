using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PrescenePlayer : NetworkBehaviour {

	[SerializeField] GameObject playerPrefab;
	[SerializeField] GameObject gameManager;
	[SerializeField] GameObject presceneUI;

	public string username;
	public bool isReady = false;

	void Start() {
		base.OnStartClient();
		presceneUI.GetComponent<PresceneManager>().RefreshPlayerList();

		string _username = "Loading...";

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

	public void SpawnPlayer(Transform _transform) {
		GameObject _newPlayer = (GameObject) Instantiate(playerPrefab);
		_newPlayer.transform.position = _transform.position;

		_newPlayer.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);

		Debug.Log(_newPlayer.GetComponent<NetworkIdentity>().clientAuthorityOwner);
	
	}

	public override void OnStartClient() {
		base.OnStartClient();
		string _netID = GetComponent<NetworkIdentity>().netId.ToString();
		PrescenePlayer _player = GetComponent<PrescenePlayer>();
		GameManager.RegisterPrescenePlayer(_netID, _player);
		//GetComponent<Player>().SetPlayerIndex();
	}

	void RegisterPlayer() {
		string _ID = "PrePlayer " + GetComponent<NetworkIdentity>().netId;
		transform.name = _ID;
	}

	[Command]
	void CmdSetUsername (string _playerID, string _username) {
		PrescenePlayer player = GameManager.GetPrescenePlayer(_playerID);
		if (player != null) {
			player.username = _username;
		}
	}

}
