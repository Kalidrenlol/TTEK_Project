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

	public GameObject playerUIInstance;
	public GameObject pregameUI;
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
		_username = transform.name;
		CmdSetPlayerID(transform.name, _username);

		isReady = false;

		if (!isLocalPlayer) {
			SetComponents(false);
			AssignRemoteLayer();
		} else {

			sceneCamera = Camera.main;
            sceneCamera.transform.parent = this.transform;
            Vector3 playerPos = this.transform.position;
            //playerPos.x = playerPos.x + 2;
            playerPos.y = 28;
            playerPos.z = -14;
            sceneCamera.transform.position = playerPos;

			playerUIInstance = Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;

			PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
			if (ui == null) {
				Debug.LogError("No playerUI on PlayerUI Prefab");
			}
			ui.SetController(GetComponent<PlayerController>());
			ui.SetPlayerScript(GetComponent<Player>());


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
			playerUIInstance.GetComponent<PlayerUI> ().btnPowerUp.onClick.AddListener (BtnUsePowerUp);
			playerUIInstance.GetComponent<PlayerUI> ().btnPush.onClick.AddListener (BtnPush);		
			gameObject.GetComponent<Rigidbody>().useGravity = false;

			string username = System.Environment.UserName;
			CmdSetUsername(transform.name, username);
		
		}

		Color c = new Color(GetComponent<Player> ().color.r, GetComponent<Player> ().color.g, GetComponent<Player> ().color.b);
		transform.FindDeepChild ("NPC_Hair_009").gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", c);


	}

	void BtnUsePowerUp() {
		GetComponent<Player>().ActivatePowerup();
	}

	void BtnPush() {
		GetComponent<PlayerController> ().PushOpponent ();
	}

	void Update() {
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

		if (isLocalPlayer) {
			playerUIInstance.GetComponent<PlayerUI>().tMana.text = GetComponent<Player>().mana.ToString();
			playerUIInstance.GetComponent<PlayerUI>().tTemp.text = GetComponent<Player>().tempMana.ToString();
			playerUIInstance.GetComponent<PlayerUI>().tSave.text = GetComponent<Player>().savedMana.ToString();
		}
	
	}

	#region PREGAME
	// No Vuforia //
	public void NoVuforia() {
		CmdSetReady();
	}
		
	// Find ImageTarget //
	public void GetReady() {
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
			//
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

		/*if (!GetReadyUI.IsOn) {
			if (isReady) {
				pregameUI.GetComponent<PregameUI>().ShowText(true);
			}
		}*/

	}


	#endregion

	[ClientRpc]
	public void RpcStartGame() {
		if (!isLocalPlayer) {
			GetComponent<Player>().Setup();
			return;
		}
		playerUIInstance.SetActive(true);
		SetComponents(true);
		GetComponent<Player>().Setup();
		gameObject.GetComponent<Rigidbody>().useGravity = true;
		scoreboard.GetComponent<Scoreboard>().RefreshScoreboard();
		if (waitingUI.activeSelf)  {
			waitingUI.SetActive(false);
		}
	}

	[ClientRpc]
	public void RpcEndGame() {
		if (!isLocalPlayer) {
			/*GetComponent<Player>().Setup();*/
			return;
		}
		playerUIInstance.SetActive(false);
		SetComponents(false);
		gameObject.GetComponent<Rigidbody>().useGravity = false;
		scoreboard.GetComponent<Scoreboard>().StopScoreboard();
		if (waitingUI.activeSelf)  {
			waitingUI.SetActive(false);
		}
		pregameUI.GetComponent<PregameUI>().EndGameUI();
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
	public void CmdSetPlayerID(string _playerID, string _username) {
		Player player = GameManager.GetPlayer(_playerID);
		if (player != null) {
			player.playerID = _username;
		}
	}

	[Command]
	public void CmdSetUsername(string _playerID, string _username) {
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