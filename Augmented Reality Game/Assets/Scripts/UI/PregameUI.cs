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
	[SerializeField] Text txtStatus;
	
	public Button btnGetReady;
	public Button btnDebug;

	public GameObject uiScoreRoom;

	void Start() {
		StartCoroutine(StartUpRefresh());
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

		switch (numPlayers) {
		case 1: SetStatusText("Let's wait for some players");
		break;
		case 2: SetStatusText("2 players is enough for a game?");
		break;
		case 3: SetStatusText("This gonna be fun!");
		break;
		case 4: SetStatusText("Are You Ready?");
		break;
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
		uiScoreRoom.SetActive(true);
		uiScoreRoom.GetComponent<UIScoreRoom>().CreateFinalScore();
	}

	public void SetStatusText(string _text) {
		txtStatus.text = _text;
	}

	IEnumerator StartUpRefresh() {
		yield return new WaitForSeconds(3f);

		RefreshPlayerlist();
	}


}
