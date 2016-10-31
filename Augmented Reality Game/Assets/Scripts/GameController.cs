using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

public class GameController : NetworkBehaviour {

	[SerializeField] GameObject gameManager;
	[SerializeField] GameObject WeaponBoxPrefab;
	[SerializeField] int _playersBeforeGo;

	[SyncVar] public bool gameStarted = false;

	private List<GameObject> WeaponsSpawn = new List<GameObject>();

	void Start() {
		gameStarted = false;
	}

	public void IsAllReady() {
		if (!isServer) {
			return;
		}

		/*if (GameManager.GetPlayers() < _playersBeforeGo) {
			return;
		}*/

		foreach (Player _player in GameManager.GetPlayers()) {
			if (!_player.GetComponent<PlayerSetup>().isReady) {
				Debug.Log(_player.name + " is not ready.");
				return;
			} else {
				Debug.Log(_player.name + " is ready.");
			}
		}
		CmdStartGame();
	}

	[Command]
	void CmdStartGame() {
		gameStarted = true;

		foreach (Player _player in GameManager.GetPlayers()) {
			_player.GetComponent<PlayerSetup>().RpcStartGame();
		}

		SpawnBoxAuto();
	}

	#region Weapon Drop

	void SpawnBoxAuto() {
		CmdSpawnBox();
		Invoke("SpawnBoxAuto", GameManager.instance.matchSettings.spawnNextWeapon);
	}

	[Command]
	public void CmdSpawnBox() {
		GameObject _box = (GameObject) Instantiate(WeaponBoxPrefab);
		_box.transform.position = GetWeaponSpawn().transform.position;
		Transform _folder = GameObject.FindGameObjectWithTag("GameWorld").transform.FindDeepChild("Weapons");
		_box.transform.SetParent(_folder);
		NetworkServer.Spawn(_box);
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

}
