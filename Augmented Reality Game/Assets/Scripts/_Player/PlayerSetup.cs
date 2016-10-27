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
<<<<<<< HEAD:Augmented Reality Game/Assets/Scripts/Player/PlayerSetup.cs
	private GameObject gameManager;

	private GameObject waitingUI;

	[SyncVar] public bool isReady = false;


	void Start() {
		gameManager = GetComponent<Player>().gameManager;
		string _username = "Loading...";
=======


	void Start() {
		SetComponents(false);
		AssignRemoteLayer();
		playerGraphics.GetComponent<Renderer>().material.color = GetComponent<Player>().color;
>>>>>>> master:Augmented Reality Game/Assets/Scripts/_Player/PlayerSetup.cs

	}

	[ClientRpc]
	public void RpcStartGame() {
		if (isLocalPlayer) {
			SetComponents(true);
			GetComponent<Player>().StartParticle();


<<<<<<< HEAD:Augmented Reality Game/Assets/Scripts/Player/PlayerSetup.cs
		if (!isLocalPlayer) {
			SetComponents(false);
			AssignRemoteLayer();
		} else {
=======
>>>>>>> master:Augmented Reality Game/Assets/Scripts/_Player/PlayerSetup.cs
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
<<<<<<< HEAD:Augmented Reality Game/Assets/Scripts/Player/PlayerSetup.cs

			if (!GetComponent<GameController>().gameStarted) {
				playerUIInstance.SetActive(false);
				SetComponents(false);
			} else {
				Debug.Log("Spil startet" + GetComponent<GameController>().gameStarted);
			}

			waitingUI = Instantiate(waitingUIPrefab);
			waitingUI.name = waitingUIPrefab.name;

=======
			scoreboard.GetComponent<Scoreboard>().RefreshScoreboard();
>>>>>>> master:Augmented Reality Game/Assets/Scripts/_Player/PlayerSetup.cs

		}

		GetComponent<Player>().Setup();
<<<<<<< HEAD:Augmented Reality Game/Assets/Scripts/Player/PlayerSetup.cs
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
=======
				
	}

	void SetComponents(bool _bool) {
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable[i].enabled = _bool;
>>>>>>> master:Augmented Reality Game/Assets/Scripts/_Player/PlayerSetup.cs
		}
	}

	void AssignRemoteLayer() {
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
	}

	public override void OnStartClient() {
		base.OnStartClient();
		string _netID = GetComponent<NetworkIdentity>().netId.ToString();
		Player _player = GetComponent<Player>();
		GameManager.RegisterPlayer(_netID, _player);

		//GetComponent<Player>().SetPlayerIndex();
	}
<<<<<<< HEAD:Augmented Reality Game/Assets/Scripts/Player/PlayerSetup.cs

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

=======
		
>>>>>>> master:Augmented Reality Game/Assets/Scripts/_Player/PlayerSetup.cs
	void OnDisable() {
		/*if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive(true);
		}*/

		GameManager.UnregisterPlayer(transform.name);
		Destroy(playerUIInstance);
	}
}


