using UnityEngine;
using System.Collections;

public class CalorieFoodObject : MonoBehaviour
{
	public AudioClip consumeAudio;
	public Transform consumeParticle;

	public int calorie;
	public int foodId;
	public string imageName;
	
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
		if (transform.position.z <= 14 || GameManager.getInstance().isGameFinished())
		{
			GameManager.getInstance().resetCombo();
			Destroy (gameObject);
		}
	}


	void OnCollisionEnter (Collision collision)
	{
		if (!collision.collider.tag.Equals("Non-Explosion"))
		{
			if(foodId==12)
				GameManager.getInstance().setFeverTime(true);

			Transform obj = (Transform)Instantiate (consumeParticle, transform.position, Quaternion.identity);
			Destroy(obj.gameObject, 1);

			AudioSource.PlayClipAtPoint(consumeAudio, transform.position, 1.0f);
			showFoodCalorie();
			showFoodImage();

			show_Text (transform);
			CollsionFoodID();
			consumeFood();

			Combo();
		
		}
		Destroy (gameObject);
	}

	public void Combo()
	{
		GameManager.getInstance ().addCombo ();
	}
	void show_Text(Transform tr)
	{
		GameManager.getInstance ().showText (tr,calorie);
	}


	void showFoodImage()
	{
		GameManager.getInstance ().showFoodImage (imageName);
	}


	void showFoodCalorie()
	{
		GameManager.getInstance ().showCalorie (calorie);

	}


	void consumeFood()
	{
		GameManager.getInstance().addScore(calorie);
	}
	void CollsionFoodID()
	{
		GameManager.getInstance ().CheckFoodCrash (foodId);

	}


}