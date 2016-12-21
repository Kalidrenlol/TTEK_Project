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
	public Button btnGetReady;
	public Button btnDebug;

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

		int numPlayers = 0;
		foreach (Player player in players) {
			GameObject itemGO = Instantiate(pregamePlayerItem, playerList, false) as GameObject;
			itemGO.transform.localScale = playerList.transform.localScale;
			PregamePlayerItem item = itemGO.GetComponent<PregamePlayerItem>();

			if (item != null) {
				numPlayers++;
				bool _isReady = player.GetComponent<PlayerSetup>().isReady;
				Color _color = player.color;
				bool _isLocal = player.GetComponent<Player>().isLocalPlayer;
				item.Setup(player.username, _isReady, _color, _isLocal, numPlayers);
			}
		}

		while(numPlayers < 4) {
			GameObject itemGO = Instantiate(pregamePlayerItem, playerList, false) as GameObject;
			itemGO.transform.localScale = playerList.transform.localScale;
			PregamePlayerItem item = itemGO.GetComponent<PregamePlayerItem>();

			if (item != null) {
				numPlayers++;
				item.NoPlayer();
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
