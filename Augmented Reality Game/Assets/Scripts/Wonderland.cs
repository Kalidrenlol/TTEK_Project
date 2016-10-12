using UnityEngine;
using System.Collections;

public class Wonderland : MonoBehaviour {

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<Player>().SetScore(1);
		}
	}

}
