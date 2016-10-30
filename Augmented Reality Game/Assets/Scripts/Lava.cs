using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			other.GetComponent<Player>().HitWater();
		}
	}

}
