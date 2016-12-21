using UnityEngine;
using System.Collections;

public class DestroyThis : MonoBehaviour {

    public float DestroyInSeconds = 3f;
 
	// Use this for initialization
	void Awake () {
        Invoke("Destroy", DestroyInSeconds);
	}

    void Destroy()
    {
        //Destroy();
    }
}
