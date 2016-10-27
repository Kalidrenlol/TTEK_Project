using UnityEngine;
using UnityEngine.Networking;

public class HostGame : NetworkBehaviour {

	[SerializeField] private uint roomSize = 4;

<<<<<<< HEAD:Augmented Reality Game/Assets/Scripts/Lobby/HostGame.cs
	private string roomName;
=======
	private static string roomName;
>>>>>>> master:Augmented Reality Game/Assets/Scripts/_Lobby/HostGame.cs
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

<<<<<<< HEAD:Augmented Reality Game/Assets/Scripts/Lobby/HostGame.cs
=======
	public string GetRoomName() {
		return roomName;
	}

>>>>>>> master:Augmented Reality Game/Assets/Scripts/_Lobby/HostGame.cs
	public void CreateRoom() {
		if (roomName != null && roomName != "") {
			networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
		}
	}
}
