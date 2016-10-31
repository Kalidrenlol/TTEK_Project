using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField] Behaviour[] componentsToDisable;
	[SerializeField] string remoteLayerName = "RemotePlayer";
//	[SerializeField] string dontDrawLayerName = "DontDraw";
	[SerializeField] GameObject playerGraphics;
	[SerializeField] GameObject playerUIPrefab;
	[SerializeField] GameObject pregameUIPrefab;
	[SerializeField] GameObject waitingUIPrefab;
	[SerializeField] GameObject getReadyUIPrefab;
	[SerializeField] GameObject aRCameraPrefab;


	public GameObject scoreboard;

	private GameObject playerUIInstance;
	private GameObject pregameUI;
	private GameObject waitingUI;
	private GameObject getReadyUI;

	private Camera sceneCamera;
	private GameObject aRCamera;

	[SyncVar] public bool isReady = false;


	void Start() {
		GameObject _imageTarget = GameObject.FindGameObjectWithTag("GameWorld");
		Transform _playerFolder = _imageTarget.transform.FindDeepChild("Players");
		gameObject.transform.SetParent(_playerFolder);


		string _username = "Loading...";

		/*if () {
			Hvis username er indtastet..	
		} else {
			
		}*/

		_username = transform.name;
		CmdSetUsername(transform.name, _username);

		isReady = false;

		if (!isLocalPlayer) {
			SetComponents(false);
			AssignRemoteLayer();
		} else {

			sceneCamera = Camera.main;

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

			pregameUI = Instantiate(pregameUIPrefab);
			pregameUI.name = pregameUIPrefab.name;
			waitingUI = pregameUI.transform.Find("WaitingRoom").gameObject;
			waitingUI.SetActive(true);
			pregameUI.GetComponent<PregameUI>().btnGetReady.onClick.AddListener(GetReady);
			pregameUI.GetComponent<PregameUI>().btnDebug.onClick.AddListener(NoVuforia);
			gameObject.GetComponent<Rigidbody>().useGravity = false;

		}

		playerGraphics.GetComponent<Renderer>().material.color = GetComponent<Player>().color;

		GetComponent<Player>().Setup();
	}



	void Update() {
		if (Input.GetKeyDown(KeyCode.I)) {
			if (!isLocalPlayer) {
				return;
			}
			GetReady();
		}

		if (Input.GetKeyDown(KeyCode.O)) {
			if (!isLocalPlayer) {
				return;
			}
			CmdSetReady();
		}

		// Set isReady ved fangst af ImageTarget
		if (getReadyUI) {
			if (getReadyUI.GetComponent<GetReadyUI>().isReady && isReady == false) {
				CmdSetReady();
				getReadyUI.SetActive(false);
			}
		}
	
	}

	#region PREGAME
	// No Vuforia //
	public void NoVuforia() {
		CmdSetReady();
	}



	// Find ImageTarget //
	public void GetReady() {
		Debug.Log("GetReady");
		if (sceneCamera != null) {
			GameObject _world = GameObject.FindGameObjectWithTag("GameWorld");
			_world.transform.SetParent(GameObject.Find("ImageTarget").transform);
			sceneCamera.gameObject.SetActive(false);
			aRCamera = Instantiate(aRCameraPrefab);
			aRCamera.name = aRCameraPrefab.name;
		}
		if (waitingUI.activeSelf)  {
			waitingUI.SetActive(false);
		}
		if (getReadyUI == null) {
			getReadyUI = Instantiate(getReadyUIPrefab);
			getReadyUI.name = getReadyUIPrefab.name;
			getReadyUI.SetActive(true);
			getReadyUI.transform.SetParent(pregameUI.transform);
		}
	}


	[Command]
	public void CmdSetReady() {
		if (!isLocalPlayer) {
			Debug.Log("Not Local");
		}
		isReady = true;
		GetComponent<GameController>().IsAllReady();
	}


	// Toggle UI /
	public void TogglePregameUI() {
		Debug.Log("TogglePregame");
		if (getReadyUI == null) {
			getReadyUI = GameObject.Find("GetReadyUI");
		}
		if (waitingUI == null) {
			waitingUI = GameObject.FindGameObjectWithTag("WaitingRoomUI");
		}
		getReadyUI.SetActive(!getReadyUI.activeSelf);
		waitingUI.SetActive(true);
		GetReadyUI.IsOn = getReadyUI.activeSelf;

		if (!GetReadyUI.IsOn) {
			if (isReady) {
				pregameUI.GetComponent<PregameUI>().ShowText(true);
			}
		}

	}


	#endregion

	[ClientRpc]
	public void RpcStartGame() {
		if (!isLocalPlayer) {
			return;
		}

		playerUIInstance.SetActive(true);
		SetComponents(true);
		gameObject.GetComponent<Rigidbody>().useGravity = true;
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


