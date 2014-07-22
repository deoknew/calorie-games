using UnityEngine;
using System.Collections;

public class CalorieFoodObject : MonoBehaviour
{
	public AudioClip consumeAudio;
	public Transform consumeParticle;

	public int calorie;
	public int foodId;

	//public Transform Material;
	//public UILabel AddedCalorieText;

	
	private static CalorieFoodObject instance;
	
	public static CalorieFoodObject getInstance() {
		return instance;
	}
	void Start()
	{

		instance = this;		
	}


	void Update() 
	{
		if (transform.position.y <= 3 || GameManager.getInstance().isGameFinished())
		{
			Destroy (gameObject);
		}
	}


	void OnCollisionEnter (Collision collision)
	{
		if (!collision.collider.tag.Equals("Non-Explosion"))
		{
			Transform obj = (Transform)Instantiate (consumeParticle, transform.position, Quaternion.identity);
			Destroy(obj.gameObject, 1);

			AudioSource.PlayClipAtPoint(consumeAudio, transform.position, 1.0f);
			showFoodCalorie();
			showFoodImage();
			show_Text (transform);
			CollsionFoodID();
			consumeFood();
		}
		Destroy (gameObject);
	}


	void show_Text(Transform tr)
	{
		GameManager.getInstance ().showText (tr,calorie);
	}


	void showFoodImage()
	{
		GameManager.getInstance ().showFoodImage (foodId);
	}


	void showFoodCalorie()
	{
		GameManager.getInstance ().showCalorie (calorie);

	}


	void consumeFood()
	{
		GameManager.getInstance().addCalorie(calorie);
	}
	void CollsionFoodID()
	{
		GameManager.getInstance ().CheckFoodCrash (foodId);
		Debug.Log (foodId);
	}


}