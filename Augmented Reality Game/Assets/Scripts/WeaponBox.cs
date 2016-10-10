using UnityEngine;
using System.Collections;

public class WeaponBox : MonoBehaviour {

	[SerializeField] GameObject[] weapons;



	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<Player>().SetScore(1);
			Destroy(transform.parent.gameObject);
		}
	}

}
