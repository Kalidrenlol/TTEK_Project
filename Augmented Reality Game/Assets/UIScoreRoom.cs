using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

public class UIScoreRoom : MonoBehaviour {

	[SerializeField] GameObject endgamePlayerItem;
	[SerializeField] Transform endPlayerList;
	[SerializeField] Text txtHeader;
	public Button btnGoToMain;

	void Start() {
		btnGoToMain.onClick.AddListener(GoToMain);
	}

	public void CreateFinalScore() {
		Player[] players = GameManager.GetPlayers();
		players = players.OrderByDescending(x => x.score).ToArray();

		//txtHeader.text = players[0].GetComponent<Player>().username + " er den store vinder!";

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
			GameObject itemGO = Instantiate(endgamePlayerItem, endPlayerList, false) as GameObject;
			itemGO.transform.localScale = endPlayerList.transform.localScale;
			EndgamePlayerItem item = itemGO.GetComponent<EndgamePlayerItem>();

			if (item != null) {
				numPlayers++;
				item.NoPlayer();
			}

		}
	}

	void GoToMain() {
		SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
	}
}
