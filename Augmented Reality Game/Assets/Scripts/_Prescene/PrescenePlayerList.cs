using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class PrescenePlayerList : MonoBehaviour {

	[SerializeField] GameObject playerItem;

	Transform playerList;

	public void RefreshPlayerlist () {
		playerList = GameObject.Find("PrescenePlayerList").transform;
		RemovePlayers();
		AddPlayers();
	}

	void AddPlayers() {
		PrescenePlayer[] players = GameManager.GetPrescenePlayers();

		foreach (PrescenePlayer player in players) {
			GameObject itemGO = Instantiate(playerItem, playerList) as GameObject;
			PrescenePlayerItem item = itemGO.GetComponent<PrescenePlayerItem>();

			if (item != null) {
				item.Setup(player.name, player.isReady);
			}
		}

	}

	void RemovePlayers() {
		foreach(Transform child in playerList) {
			Destroy(child.gameObject);
		}
	}
}
