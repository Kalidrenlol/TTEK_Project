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
	[SerializeField] GameObject waitingUIPrefab;

	public GameObject scoreboard;
	private GameObject playerUIInstance;
	private GameObject gameManager;

	private GameObject waitingUI;

	[SyncVar] public bool isReady = false;


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

			isReady = false;
			playerUIInstance = Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;

			PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
			if (ui == null) {
				Debug.LogError("No playerUI on PlayerUI Prefab");
			}

			if (!GetComponent<GameController>().gameStarted) {
				playerUIInstance.SetActive(false);
				SetComponents(false);
			} else {
				Debug.Log("Spil startet" + GetComponent<GameController>().gameStarted);
			}

			waitingUI = Instantiate(waitingUIPrefab);
			waitingUI.name = waitingUIPrefab.name;


		}
		playerGraphics.GetComponent<Renderer>().material.color = GetComponent<Player>().color;

		GetComponent<Player>().Setup();
	}



	void Update() {
		if (Input.GetKeyDown(KeyCode.O)) {
			if (!isLocalPlayer) {
				return;
			}
			CmdSetReady();
		}
	}

	#region PREGAME

	[Command]
	public void CmdSetReady() {
			isReady = true;
			GetComponent<GameController>().IsAllReady();
	}



	#endregion

	[ClientRpc]
	public void RpcStartGame() {
		if (!isLocalPlayer) {
			return;
		}
		playerUIInstance.SetActive(true);
		SetComponents(true);
		scoreboard.GetComponent<Scoreboard>().RefreshScoreboard();

		if (waitingUI.activeSelf)  {
			waitingUI.SetActive(false);
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


