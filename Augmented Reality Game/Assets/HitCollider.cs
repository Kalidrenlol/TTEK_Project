using UnityEngine;
using System.Collections;

public class HitCollider : MonoBehaviour {

	public bool isInRange;

	// Use this for initialization
	void Start () {
		isInRange = false;
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "Player") {
			Debug.Log(coll.name +" er inden for rækkevidde");
			isInRange = true;
		}

		Debug.Log(this);
		Debug.Log(coll.gameObject);

		if (coll.gameObject == this) {
			Debug.Log("Sig selv");
		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.gameObject.tag == "Player") {
			Debug.Log(coll.name + " er uden for rækkevidde");
			isInRange = false;
		}
	}

}
