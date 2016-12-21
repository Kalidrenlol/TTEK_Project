using UnityEngine;
using System.Collections;

public class NoMansLand : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision) {


        if (collision.collider.tag == "Player")
        {
			if (GameManager.instance.matchSettings.showDebug) {
				Debug.Log ("Player \"" + collision.collider.name + "\" hit outside");
			}
        }

    }

    void OnCollisionExit(Collision collision)
    {


        if (collision.collider.tag == "Player")
        {
			if (GameManager.instance.matchSettings.showDebug) {
				Debug.Log ("Player \"" + collision.collider.name + "\" left outside");
			}
        }

    }

}
