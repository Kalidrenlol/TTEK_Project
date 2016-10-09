using UnityEngine;
using System.Collections;

public class WeaponBox : MonoBehaviour {

	[SerializeField] GameObject[] weapons;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			Destroy(transform.parent.gameObject);
		}
	}

}
