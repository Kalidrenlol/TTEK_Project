using UnityEngine;
using System.Collections;

public class WeaponBox : MonoBehaviour {

	[SerializeField] GameObject[] weapons;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<Player>().SetScore(1);
            other.gameObject.GetComponent<Player>().CollectPowerup();
			Destroy(transform.parent.gameObject);
		}
	}

}
