using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour {
	Vector3 direction = Vector3.left;
	Vector3 start = new Vector3((float)2.5, (float)2.5, (float)4);
	Vector3 end = new Vector3((float)-10.5,(float)2.5,(float)4);

	void Start() {
		transform.localPosition = start;
	}

	void Update() {
		if (transform.position.x < end.x) {
			direction = Vector3.right;
		}
		if (transform.position.x > start.x) {
			direction = Vector3.left;
		}
		transform.Translate (direction * Time.deltaTime);
	}
}