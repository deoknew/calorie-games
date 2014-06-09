using UnityEngine;
using System.Collections;

public class CollisionEvent : MonoBehaviour 
{
	public Transform collisionParticle;


	void Update() 
	{
		if (transform.position.z <= 12)
			Destroy (gameObject);

	}


	void OnCollisionEnter (Collision collision) 
	{
		if (!collision.collider.tag.Equals("Non-Explosion")) 
		{
			Transform obj = (Transform)Instantiate (collisionParticle, transform.position, Quaternion.identity);
			Destroy(obj.gameObject, 1);
		}
		Destroy (gameObject);
	}
}
