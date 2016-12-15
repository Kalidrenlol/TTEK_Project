using UnityEngine;
using System.Collections;

public class HitCollider : MonoBehaviour {

	public bool isInRange;
	public GameObject player;

	// Use this for initialization
	void Start () {
		isInRange = false;
	}

	/*void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "Player") {
			Debug.Log(coll.name +" er inden for rækkevidde");
			isInRange = true;
		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.gameObject.tag == "Player") {
			Debug.Log(coll.name + " er uden for rækkevidde");
			isInRange = false;
		}
	}*/

	void OnTriggerStay(Collider coll) {
		if (coll.gameObject.tag == "Player") {
			player.GetComponent<Player>().PushOpponent(coll);
			Debug.Log("..");
		}
	}

}
