using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class GetReadyUI : MonoBehaviour {

	private GameObject imageTarget;

	public static bool IsOn = false;
	public bool isReady = false;

	void Start() {
		IsOn = false;
		isReady = false;
		imageTarget = GameObject.FindGameObjectWithTag("ImageTarget");
	}

	void Update() {
		if (isReady) {
			return;
		}
		if (imageTarget.GetComponent<DefaultTrackableEventHandler>().headTargetFound) {
			isReady = true;
		}
	}

}
