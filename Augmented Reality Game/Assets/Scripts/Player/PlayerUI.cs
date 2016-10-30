using UnityEngine;

public class PlayerUI : MonoBehaviour {

	[SerializeField] GameObject pauseMenu;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			TogglePauseMenu();
		}
	}

	void Start() {
		PauseMenu.IsOn = false;
	}

	public void TogglePauseMenu() {
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		PauseMenu.IsOn = pauseMenu.activeSelf;
	}
}
