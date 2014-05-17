using UnityEngine;
using System.Collections;

public class CollisionEvent : MonoBehaviour {
	public int power=1000;
	public Transform explode;
	// Use this for initialization

	void Update() {
		if (transform.position.x <= 24)
						Destroy (gameObject);
	}
	void OnCollisionEnter (Collision Collsion) {
		if (!Collsion.collider.tag.Equals ("BALL")) 
		{

						Transform obj = (Transform)Instantiate (explode, transform.position, Quaternion.identity);
						Destroy (gameObject, 1);
						Destroy(obj.gameObject,1);
		}
	}
}
