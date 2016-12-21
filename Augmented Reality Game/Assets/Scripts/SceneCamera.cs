using UnityEngine;
using System.Collections;

public class SceneCamera : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] float rSpeed;
	[SerializeField] float zSpeed;
	[SerializeField] float zrSpeed;

	public GameObject mainCamera;
	public bool rotateCamera;
	public bool zoomToMain;
	public bool doneZooming;

	void Start() {
		doneZooming = false;
	}

	void Update(){
		if (rotateCamera) {
			transform.LookAt(target);
			transform.Translate(Vector3.right * rSpeed * Time.deltaTime);
		}

		if (zoomToMain) {
			transform.position = Vector3.MoveTowards(transform.position, mainCamera.transform.position, zSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Lerp (transform.rotation,mainCamera.transform.rotation,Time.deltaTime * zrSpeed);

			if (transform.position == mainCamera.transform.position && transform.rotation == mainCamera.transform.rotation && doneZooming) {

				doneZooming = true;
			}
		}
	}
}
