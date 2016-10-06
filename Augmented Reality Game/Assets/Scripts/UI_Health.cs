using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class UI_Health : MonoBehaviour {

	private Text healthTxt;

	// Use this for initialization
	void Start () {
		healthTxt = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetText(int _health) {
		healthTxt.text = _health.ToString();
	}
}
