using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

public class GameManager : NetworkBehaviour {

	[SerializeField] Color[] playerColors;
	[SerializeField] GameObject presceneUIPrefab;
	[SerializeField] GameObject networkManager;
	[SerializeField] GameObject playerPrefab;

	public bool gameStarted = false;
	[SyncVar] public string roomName;
	public static GameManager instance;

	public MatchSettings matchSettings;
	public GameObject WeaponBoxPrefab;

	private GameObject presceneUI;
	private List<GameObject> WeaponsSpawn = new List<GameObject>();


	void Awake() {
		if (instance != null) {
			Debug.LogError("More than one GameManager in scene.");
		} else {
			instance = this;
		}
	}

	void Start() {
		presceneUI = Instantiate(presceneUIPrefab);
		if (GetComponent<NetworkIdentity>().isServer) {
			Debug.Log("Roomname set");
			roomName = networkManager.GetComponent<HostGame>().GetRoomName();
		} else {
			Debug.Log("Roomname not set due to client");
		}
	}


	#region Game Control
	//[Command]
	public void StartGame(int _index) {
		if (!gameStarted) {
			Debug.Log("Starting game");
			gameStarted = true;

			//Deactivate Prescene UI
			presceneUI.SetActive(false);

			// Spawn Players
			foreach (Player _player in GetPlayers()) {
				Debug.Log("Spawning "+_player.name);

				_player.GetComponent<PlayerSetup>().RpcStartGame();

			}

			// Spawn boxe
			//SpawnBoxAuto();

		}
	}

	public Color GetPlayerColor(int _index) {
		return playerColors[_index];
	}

	public bool IsGameStarted() {
		return gameStarted;
	}

	#endregion

	#region Weapon Drop

	void SpawnBoxAuto() {
		CmdSpawnBox();
		Invoke("SpawnBoxAuto", instance.matchSettings.spawnNextWeapon);
	}

	[Command]
	public void CmdSpawnBox() {
		GameObject box = (GameObject) Instantiate(WeaponBoxPrefab);
		box.transform.position = GetWeaponSpawn().transform.position;
		NetworkServer.Spawn(box);
	}

	GameObject GetWeaponSpawn() {
		WeaponsSpawn.Clear();
		GameObject[] SpawnArray = GameObject.FindGameObjectsWithTag("WeaponDrop");
		foreach (GameObject Spawn in SpawnArray) {
			WeaponsSpawn.Add(Spawn);
		}

		return WeaponsSpawn[Random.Range(0,WeaponsSpawn.Count())];
	}
		
	#endregion
		
	#region Player tracking

	private const string PLAYER_ID_PREFIX = "Player ";

	private static Dictionary<string, Player> players = new Dictionary<string, Player>();
	private static Dictionary<string, PrescenePlayer> prescenePlayers = new Dictionary<string, PrescenePlayer>();


	public static void RegisterPlayer(string _netID, Player _player) {
		string _playerID = PLAYER_ID_PREFIX + _netID;
		players.Add(_playerID, _player);
		_player.transform.name = _playerID;
		Debug.Log(_player + " registreret.");
	}

	public static void UnregisterPlayer(string _playerID) {
		players.Remove(_playerID);
	}

	public static Player GetPlayer(string _playerID) {
		return players[_playerID];
	}

	public static Player[] GetPlayers() {
		return players.Values.ToArray();
	}

	// PRESCENE PLAYERS //

	public static void RegisterPrescenePlayer(string _netID, PrescenePlayer _player) {
		string _playerID = PLAYER_ID_PREFIX + _netID;
		prescenePlayers.Add(_playerID, _player);
		_player.transform.name = _playerID;
		Debug.Log("Prescene Player registreret.");
	}

	public static void UnregisterPrescenePlayer(string _playerID) {
		prescenePlayers.Remove(_playerID);
	}

	public static PrescenePlayer GetPrescenePlayer(string _playerID) {
		return prescenePlayers[_playerID];
	}

	public static PrescenePlayer[] GetPrescenePlayers() {
		return prescenePlayers.Values.ToArray();
	}

	#endregion


}
