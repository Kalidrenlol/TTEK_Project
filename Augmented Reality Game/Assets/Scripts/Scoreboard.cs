using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

	[SerializeField] GameObject scoreboardItem;
	Transform playerList;

	public void RefreshScoreboard () {
		//RemovePlayers();
		AddPlayers();
	}


	void AddPlayers() {
		Player[] players = GameManager.GetPlayers();
		playerList = GameObject.Find("ScoreboardPlayerList").transform;

		foreach (Player player in players) {
			GameObject itemGO = Instantiate(scoreboardItem, playerList) as GameObject;
			ScoreboardItem item = itemGO.GetComponent<ScoreboardItem>();

			if (item != null) {
				item.Setup(player.username, player.score);
			}
		}
	}

	void RemovePlayers() {
		foreach(Transform child in playerList) {
			Destroy(child.gameObject);
		}
	}
}
