using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

public class GameManager : NetworkBehaviour {

	[SerializeField] Color[] playerColors;

	public static GameManager instance;
	public MatchSettings matchSettings;
	public PowerUps powerUps;

	void Awake() {
		if (instance != null) {
			Debug.LogError("More than one GameManager in scene.");
		} else {
			instance = this;
		}
	}

	public Color GetPlayerColor(int _index) {
		return playerColors[_index];
	}

		
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
