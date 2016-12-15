using UnityEngine;
using System.Collections;

public class Wonderland : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<Player>().isOnWonderland = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<Player>().isOnWonderland = false;
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<Player>().TempMana(GameManager.instance.matchSettings.getManaPrSecond * Time.deltaTime);
		}
	}

}
