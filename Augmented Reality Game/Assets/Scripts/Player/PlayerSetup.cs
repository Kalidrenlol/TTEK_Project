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
	//Camera sceneCamera;

	void Start() {
		string _username = "Loading...";

		/*if () {
			Hvis username er indtastet..	
		} else {
			
		}*/

		_username = transform.name;
		CmdSetUsername(transform.name, _username);

		if (!isLocalPlayer) {
			DisableComponents();
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
			ui.SetController(GetComponent<PlayerController>());
			scoreboard.GetComponent<Scoreboard>().RefreshScoreboard();

		}
		playerGraphics.GetComponent<Renderer>().material.color = GetComponent<Player>().color;

		GetComponent<Player>().Setup();

	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.E)) {
			scoreboard.GetComponent<Scoreboard>().RefreshScoreboard();
		}
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

	void DisableComponents() {
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable[i].enabled = false;
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
