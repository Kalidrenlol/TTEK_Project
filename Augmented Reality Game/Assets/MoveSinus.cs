using UnityEngine;
using System.Collections;

public class MoveSinus : MonoBehaviour {

    Vector3 _startPosition;

	// Use this for initialization
	void Start () {
        _startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = _startPosition + new Vector3(0.0f, Mathf.Sin(Time.time*5)*3, 0.0f);
	}
}
