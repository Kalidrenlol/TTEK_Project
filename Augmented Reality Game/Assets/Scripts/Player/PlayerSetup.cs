using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField] Behaviour[] componentsToDisable;
	[SerializeField] string remoteLayerName = "RemotePlayer";
//	[SerializeField] string dontDrawLayerName = "DontDraw";
	[SerializeField] GameObject playerGraphics;
	[SerializeField] GameObject playerUIPrefab;

	public GameObject scoreboard;
	private GameObject playerUIInstance;
	private GameObject gameManager;
	//Camera sceneCamera;

	void Start() {
		gameManager = GetComponent<Player>().gameManager;
		string _username = "Loading...";

		/*if () {
			Hvis username er indtastet..	
		} else {
			
		}*/

		_username = transform.name;
		CmdSetUsername(transform.name, _username);

		if (!isLocalPlayer) {
			SetComponents(false);
			AssignRemoteLayer();
		} else {
			/*sceneCamera = Camera.main;
			if (sceneCamera != null) {
				sceneCamera.gameObject.SetActive(false);
			}*/

			playerUIInstance = Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;

			PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
			if (ui == null) {
				Debug.LogError("No playerUI on PlayerUI Prefab");
			}

			if (!gameManager.GetComponent<GameManager>().gameStarted) {
				Debug.Log("Spil ikke startet:");
				playerUIInstance.SetActive(false);
				SetComponents(false);
			} else {
				Debug.Log("Spil startet");
			}


		}
		playerGraphics.GetComponent<Renderer>().material.color = GetComponent<Player>().color;

		GetComponent<Player>().Setup();

	}

	public void StartGame() {
		playerUIInstance.SetActive(true);
		SetComponents(true);
		scoreboard.GetComponent<Scoreboard>().RefreshScoreboard();
	}

	public override void OnStartClient() {
		base.OnStartClient();
		string _netID = GetComponent<NetworkIdentity>().netId.ToString();
		Player _player = GetComponent<Player>();
		GameManager.RegisterPlayer(_netID, _player);
		GetComponent<Player>().SetPlayerIndex();
	}

	void RegisterPlayer() {
		string _ID = "Player " + GetComponent<NetworkIdentity>().netId;
		transform.name = _ID;
	}

	void SetComponents(bool _bool) {
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable[i].enabled = _bool;
		}
	}

	void AssignRemoteLayer() {
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
	}

	[Command]
	void CmdSetUsername (string _playerID, string _username) {
		Player player = GameManager.GetPlayer(_playerID);
		if (player != null) {
			player.username = _username;
		}
	}

	void OnDisable() {
		/*if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive(true);
		}*/

		GameManager.UnregisterPlayer(transform.name);
		Destroy(playerUIInstance);
	}
}
