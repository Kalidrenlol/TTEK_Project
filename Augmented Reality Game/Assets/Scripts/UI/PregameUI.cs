using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Linq;

public class PregameUI : MonoBehaviour {

	[SerializeField] GameObject pregamePlayerItem;
	[SerializeField] float secondsBetweenUpdate;
	[SerializeField] Transform prePlayerList;
	[SerializeField] GameObject waitingRoom;
	public Button btnGetReady;
	public Button btnDebug;

	[SerializeField] GameObject scoreRoom;
	[SerializeField] GameObject endgamePlayerItem;
	[SerializeField] Transform endPlayerList;


	void Start() {
		RefreshPlayerlist();
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
			GameObject itemGO = Instantiate(pregamePlayerItem, prePlayerList, false) as GameObject;
			itemGO.transform.localScale = prePlayerList.transform.localScale;
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
			GameObject itemGO = Instantiate(pregamePlayerItem, prePlayerList, false) as GameObject;
			itemGO.transform.localScale = prePlayerList.transform.localScale;
			PregamePlayerItem item = itemGO.GetComponent<PregamePlayerItem>();

			if (item != null) {
				numPlayers++;
				item.NoPlayer();
			}

		}
	}

	void RemovePlayers() {
		foreach(Transform child in prePlayerList) {
			Destroy(child.gameObject);
		}
	}

	public void EndGameUI() {
		waitingRoom.SetActive(false);
		scoreRoom.SetActive(true);
		CreateFinalScore();
	}

	void CreateFinalScore() {
		Player[] players = GameManager.GetPlayers();
		players = players.OrderByDescending(x => x.score).ToArray();

		int numPlayers = 0;
		foreach (Player player in players) {
			GameObject itemGO = Instantiate(endgamePlayerItem, endPlayerList, false) as GameObject;
			itemGO.transform.localScale = endPlayerList.transform.localScale;
			EndgamePlayerItem item = itemGO.GetComponent<EndgamePlayerItem>();

			if (item != null) {
				numPlayers++;
				bool _isLocal = player.GetComponent<Player>().isLocalPlayer;
				item.Setup(player.username, numPlayers, player.color, _isLocal, player.score);
			}
		}

		while(numPlayers < 4) {
			GameObject itemGO = Instantiate(pregamePlayerItem, endPlayerList, false) as GameObject;
			itemGO.transform.localScale = endPlayerList.transform.localScale;
			PregamePlayerItem item = itemGO.GetComponent<PregamePlayerItem>();

			if (item != null) {
				numPlayers++;
				item.NoPlayer();
			}

		}
	}

}
