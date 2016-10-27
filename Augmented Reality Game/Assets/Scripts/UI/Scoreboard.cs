using UnityEngine;
using System.Collections;
using System.Linq;

public class Scoreboard : MonoBehaviour {

	[SerializeField] GameObject scoreboardItem;
	[SerializeField] float secondsBetweenUpdate;
	Transform playerList;

	public void RefreshScoreboard () {
		playerList = GameObject.Find("ScoreboardPlayerList").transform;
		RemovePlayers();
		AddPlayers();
		Invoke("RefreshScoreboard", secondsBetweenUpdate);
	}

	void AddPlayers() {
		Player[] players = GameManager.GetPlayers();
		players = players.OrderByDescending(x => x.score).ToArray();

		foreach (Player player in players) {
			GameObject itemGO = Instantiate(scoreboardItem, playerList) as GameObject;
			ScoreboardItem item = itemGO.GetComponent<ScoreboardItem>();

			if (item != null) {
				item.Setup(player.username, player.score, player.color);
			}
		}
	}

	void RemovePlayers() {
		foreach(Transform child in playerList) {
			Destroy(child.gameObject);
		}
	}
}
