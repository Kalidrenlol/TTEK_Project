using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class Scoreboard : MonoBehaviour {

	[SerializeField] GameObject scoreboardItem;
	[SerializeField] int secondsBetweenUpdate;
	Transform playerList;

	public void RefreshScoreboard () {
		playerList = GameObject.Find("ScoreboardPlayerList").transform;
		RemovePlayers();
		AddPlayers1();
		Invoke("RefreshScoreboard", secondsBetweenUpdate);
	}

	void AddPlayers1() {
		Player[] players = GameManager.GetPlayers();

		players = players.OrderByDescending(x => x.score).ToArray();
		Debug.Log(players[0]);

		foreach (Player player in players) {
			GameObject itemGO = Instantiate(scoreboardItem, playerList) as GameObject;
			ScoreboardItem item = itemGO.GetComponent<ScoreboardItem>();

			if (item != null) {
				item.Setup(player.username, player.score);
			}
		}
	}

	void AddPlayers() {
		Player[] players = GameManager.GetPlayers();

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
