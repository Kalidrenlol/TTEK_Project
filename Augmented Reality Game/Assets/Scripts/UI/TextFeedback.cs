using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextFeedback : MonoBehaviour {

	[SerializeField] Text txt;

	void Update() {
		if (txt.fontSize < 300) {
			txt.fontSize += 20;
		}
	}

	public void ShowText(string _txt, int _fontsize) {
		SetText("");
		txt.CrossFadeAlpha(1, 0f, true);
		StopCoroutine("HideText");

		txt.fontSize = _fontsize;
		SetText(_txt);
		StartCoroutine(HideText());

	}

	void SetText(string _txt) {
		txt.text = _txt;
	}

	private IEnumerator HideText() {
		yield return new WaitForSeconds(1f);
		txt.CrossFadeAlpha(0, 0.5f, true);
	}

}
