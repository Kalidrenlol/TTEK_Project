using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

	[SerializeField] private uint roomSize = 4;

	private string roomName;
	private NetworkManager networkManager;

	void Start() {
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null) {
			networkManager.StartMatchMaker();
		}
	}

	public void SetRoomName(string _name) {
		roomName = _name;
	}

	public void CreateRoom() {
		if (roomName != null && roomName != "") {
			networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
		}
	}
}
