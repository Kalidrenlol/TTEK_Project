using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;

public class PresceneManager : NetworkBehaviour {

	[SerializeField] GameObject gameManager;
	[SerializeField] GameObject networkManager;
	[SerializeField] GameObject playerList;
	[SerializeField] Text roomNameText;
	[SerializeField] Text statusText;
	[SerializeField] Text playerTitle;

	private string roomName;

	void Start() {
		roomName = networkManager.GetComponent<HostGame>().GetRoomName();
		if (roomName == "") {
			roomName = "Not Available";
		}

		roomNameText.text = "Room: " + roomName;
		RefreshPlayerList();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.E)) {
			RefreshPlayerList();
			Debug.Log("PlayerList Opdateret");
		}
	}

	public void RefreshPlayerList() {
		playerList.GetComponent<PrescenePlayerList>().RefreshPlayerlist();
		int _playerNo = GameManager.GetPrescenePlayers().Count();
		playerTitle.text = "Players ("+_playerNo+"/4)";

	}

	public void StatusText(string _text) {
		statusText.text = _text;
	}


}
