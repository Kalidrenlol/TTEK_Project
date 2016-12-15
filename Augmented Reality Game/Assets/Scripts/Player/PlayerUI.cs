using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	[SerializeField] GameObject pauseMenu;

	private Player playerScript;
	private PlayerController controller;

	[SerializeField] private Text txtMana;
	[SerializeField] private GameObject manaSlider;

	public void SetPlayerScript(Player _script) {
		playerScript = _script;
	}

	public void SetController(PlayerController _controller) {
		controller = _controller;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			TogglePauseMenu();
		}

		// Update ManaSlider //
		if (manaSlider != null) {
			manaSlider.transform.FindDeepChild("TempBar").GetComponent<Slider>().value = playerScript.mana + playerScript.savedMana + playerScript.tempMana;
			manaSlider.transform.FindDeepChild("SavedBar").GetComponent<Slider>().value = playerScript.mana;
			txtMana.text = Mathf.Floor(playerScript.mana).ToString();
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
