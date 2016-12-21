using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

public class GameController : NetworkBehaviour {

	[SerializeField] GameObject gameManager;
	[SerializeField] GameObject WeaponBoxPrefab;
	[SerializeField] int _playersBeforeGo;
	public GameObject Fireworks;
	bool startCountdown = false;
	bool countdownTriggered = false;
	float countdown = 5f;

	[SyncVar] public bool gameStarted = false;
	[SyncVar(hook="EndGame")] public bool gameEnded = false;

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

		startCountdown = true;
		GetComponent<PlayerSetup>().pregameUI.SetActive(false);
		GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
	}

	void Update() {
		if (startCountdown && !countdownTriggered) {
			countdown -= Time.deltaTime;
			GetComponent<PlayerSetup>().playerUIInstance.GetComponent<PlayerUI>().ShowFeedbackText(Mathf.Ceil(countdown).ToString(), 400);
		}
		if (!countdownTriggered && countdown < 0f) {
			CmdStartGame();
			countdownTriggered = true;
		}
	}

	public void IsGameFinish() {
		foreach (Player _player in GameManager.GetPlayers()) {
			if (_player.GetComponent<Player>().score >= GameManager.instance.matchSettings.killToWin) {
				gameEnded = true;
			}
		}

	}

	[Command]
	void CmdStartGame() {
		gameStarted = true;

		foreach (Player _player in GameManager.GetPlayers()) {
			_player.GetComponent<PlayerSetup>().RpcStartGame();
		}

		SpawnBoxAuto();
	}

	void EndGame(bool _gameEnded) {
		if (!isLocalPlayer) {
			return;
		}
		if (_gameEnded) {
			CmdEndGame();
		}
	}

	[Command]
	void CmdEndGame() {
		foreach (Player _player in GameManager.GetPlayers()) {
			_player.GetComponent<PlayerSetup>().RpcEndGame();
		}
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

	public void StartFireworks() {
		Instantiate(Fireworks);
	}

}
