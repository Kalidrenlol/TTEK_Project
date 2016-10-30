using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Linq;

public class PregameUI : MonoBehaviour {

	[SerializeField] GameObject pregamePlayerItem;
	[SerializeField] float secondsBetweenUpdate;
	[SerializeField] Transform playerList;
	[SerializeField] GameObject goToGameObj;
	public Button getReadyBtn;

	void Start() {
		RefreshPlayerlist();
		goToGameObj.SetActive(false);
	}

	public void RefreshPlayerlist () {
		RemovePlayers();
		AddPlayers();
		Invoke("RefreshPlayerlist", secondsBetweenUpdate);
	}

	void AddPlayers() {
		Player[] players = GameManager.GetPlayers();

		foreach (Player player in players) {
			GameObject itemGO = Instantiate(pregamePlayerItem, playerList) as GameObject;
			PregamePlayerItem item = itemGO.GetComponent<PregamePlayerItem>();

			if (item != null) {
				bool _isReady = player.GetComponent<PlayerSetup>().isReady;
				Color _color = player.color;
				item.Setup(player.username, _isReady, _color);
			}
		}
	}

	void RemovePlayers() {
		foreach(Transform child in playerList) {
			Destroy(child.gameObject);
		}
	}

	public void ShowText(bool _show) {
		goToGameObj.SetActive(_show);
	}

}
