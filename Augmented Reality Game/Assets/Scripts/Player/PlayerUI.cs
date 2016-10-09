using UnityEngine;

public class PlayerUI : MonoBehaviour {

	[SerializeField] GameObject pauseMenu;
	private PlayerController controller;

	public void SetController(PlayerController _controller) {
		controller = _controller;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			TogglePauseMenu();
		}
	}

	void Start() {
		PauseMenu.IsOn = false;
	}

	void TogglePauseMenu() {
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		PauseMenu.IsOn = pauseMenu.activeSelf;
	}
}
