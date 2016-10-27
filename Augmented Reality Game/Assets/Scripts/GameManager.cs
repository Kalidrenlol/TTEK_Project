using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

public class GameManager : NetworkBehaviour {

	[SerializeField] Color[] playerColors;

	public static GameManager instance;

	[SyncVar] public bool gameStarted = false;
	public MatchSettings matchSettings;
	public GameObject WeaponBoxPrefab;

	private List<GameObject> WeaponsSpawn = new List<GameObject>();


	void Awake() {
		if (instance != null) {
			Debug.LogError("More than one GameManager in scene.");
		} else {
			instance = this;
		}
	}

	void Start() {
		gameStarted = false;

	}


	public void IsAllReady() {
		if (isServer) {
			Debug.Log("Is server");
		} else if (isClient) {
			Debug.Log("Is Client");
		} else {
			Debug.Log("Nothign");
		}
		Debug.Log("IsAllReady Called");	
		foreach (Player _player in GetPlayers()) {
			if (!_player.GetComponent<PlayerSetup>().isReady) {
				Debug.Log(_player+" is not ready.");
				return;
			} else {
				Debug.Log(_player.name + " is " + _player.GetComponent<PlayerSetup>().isReady);
			}
		}
		CmdStartGame();
	}

	[Command]
	void CmdStartGame() {
		gameStarted = true;

		foreach (Player _player in GetPlayers()) {
			_player.GetComponent<PlayerSetup>().RpcStartGame();
		}

		SpawnBoxAuto();
	}

	public Color GetPlayerColor(int _index) {
		return playerColors[_index];
	}

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


	public static void RegisterPlayer(string _netID, Player _player) {
		string _playerID = PLAYER_ID_PREFIX + _netID;
		players.Add(_playerID, _player);
		_player.transform.name = _playerID;
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

	#endregion


}
