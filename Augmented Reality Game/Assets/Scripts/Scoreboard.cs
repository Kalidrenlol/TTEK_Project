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
        GameObject scoreList = GameObject.FindWithTag("ScoreboardContent");

		foreach (Player player in players) {
            GameObject itemGO = Instantiate(scoreboardItem) as GameObject;
			itemGO.transform.SetParent(scoreList.transform);
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
