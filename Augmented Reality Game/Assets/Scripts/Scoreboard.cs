using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

	[SerializeField] GameObject scoreboardItem;
	[SerializeField] Transform playerList;

	public void RefreshScoreboard () {
		//RemovePlayers();
		AddPlayers();
	}


	void AddPlayers() {
		Player[] players = GameManager.GetPlayers();

		foreach (Player player in players) {
			GameObject itemGO = (GameObject) Instantiate(scoreboardItem);
			itemGO.transform.SetParent(playerList);
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
