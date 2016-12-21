using UnityEngine;
using System.Collections;

public class SceneCamera : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] float speed;

	void Update(){
		transform.LookAt(target);
		transform.Translate(Vector3.right * speed * Time.deltaTime);
	}
}
