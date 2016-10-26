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


	void Start() {
		SetComponents(false);
		AssignRemoteLayer();
		playerGraphics.GetComponent<Renderer>().material.color = GetComponent<Player>().color;

	}

	[ClientRpc]
	public void RpcStartGame() {
		if (isLocalPlayer) {
			SetComponents(true);
			GetComponent<Player>().StartParticle();


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
			scoreboard.GetComponent<Scoreboard>().RefreshScoreboard();

		}

		GetComponent<Player>().Setup();
				
	}

	void SetComponents(bool _bool) {
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable[i].enabled = _bool;
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
		
	void OnDisable() {
		/*if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive(true);
		}*/

		GameManager.UnregisterPlayer(transform.name);
		Destroy(playerUIInstance);
	}
}
