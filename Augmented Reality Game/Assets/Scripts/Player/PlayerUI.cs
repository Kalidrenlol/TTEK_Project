using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	public static PlayerUI instance;
	[SerializeField] GameObject pauseMenu;

	private Player playerScript;
	private PlayerController controller;

	public GameObject textFeedback;

	[Header("RightHandController")]
	public Button btnPush;
	public Button btnPowerUp;
	[SerializeField] GameObject manaSlider;
	[SerializeField] Text txtManaCount;


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
			txtManaCount.text = Mathf.Floor(playerScript.mana).ToString();
		}

		if (Input.GetKeyDown(KeyCode.C)) {
			ShowFeedbackText("Kill");
		}

	}

	void Start() {
		PauseMenu.IsOn = false;
	}

	public void ShowFeedbackText(string _txt) {
		textFeedback.GetComponent<TextFeedback>().ShowText(_txt);
	}

	public void TogglePauseMenu() {
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		PauseMenu.IsOn = pauseMenu.activeSelf;
	}
}
