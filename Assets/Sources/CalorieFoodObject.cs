using UnityEngine;
using System.Collections;

public class CalorieFoodObject : MonoBehaviour
{
	public AudioClip consumeAudio;
	public Transform consumeParticle;
	public int calorie;


	void Update() 
	{
		if (transform.position.y <= 0)
			Destroy(gameObject);
	}


	void OnCollisionEnter (Collision collision)
	{
		if (!collision.collider.tag.Equals("Non-Explosion"))
		{
			Transform obj = (Transform)Instantiate (consumeParticle, transform.position, Quaternion.identity);
			Destroy(obj.gameObject, 1);

			AudioSource.PlayClipAtPoint(consumeAudio, transform.position, 1.0f);

			consumeFood();
		}
		Destroy (gameObject);
	}


	void consumeFood()
	{
		GameManager.getInstance().addCalorie(calorie);
	}
}