using UnityEngine;
using System.Collections;

public class Wonderland : MonoBehaviour {

	Renderer rend;

	void Start() {
		rend = GetComponent<Renderer>();
	}

	void OnTriggerEnter(Collider other) {
		//rend.material.SetColor("_color",Color.red);
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<Player>().SetScore(1);
		}
	}

	void OnTriggerExit(Collider other) {
		//rend.material.SetColor("_color",Color.black);
	}

}
