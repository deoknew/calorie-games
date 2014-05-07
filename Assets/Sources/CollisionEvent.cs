using UnityEngine;
using System.Collections;

public class CollisionEvent : MonoBehaviour {
	public int power=1000;
	public Transform explode;
	// Use this for initialization

	void OnCollisionEnter (Collision Collsion) {
		//this.gameObject.rigidbody.AddForce (0,1 * power,1 * power);
		Object obj = Instantiate (explode, transform.position, Quaternion.identity);
		//Destroy (gameObject);
		Destroy (obj, 2);
	}
}
