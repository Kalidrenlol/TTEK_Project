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
				return;
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

}
