using UnityEngine;
using System.Collections;

public class HitCollider : MonoBehaviour {

	public GameObject player;

	void OnTriggerStay(Collider coll) {
		if (coll.gameObject.tag == "Player") {
			//player.GetComponent<Player>().PushOpponent(coll);
		}
	}

}
