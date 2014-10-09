using UnityEngine;
using System.Collections;

public class CalorieFoodObject : MonoBehaviour
{
	public AudioClip consumeAudio;
	public GameObject consumeParticle;

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
		if (transform.position.z <= 2.5 || GameManager.getInstance().isGameFinished())
		{
			GameManager.getInstance().resetCombo();
			Destroy (gameObject);
		}
	}


	void OnCollisionEnter (Collision collision)
	{
		if (!collision.collider.tag.Equals("Non-Explosion"))
		{
			if (foodId == 12) // FeverTime
				GameManager.getInstance().startFeverTime();

			if (foodId == 11) // Bomb
				EVAction.run(Camera.main.gameObject);

			GameObject obj = (GameObject)Instantiate (consumeParticle, transform.position, Quaternion.identity);
			Destroy(obj, 3);

			AudioSource.PlayClipAtPoint(consumeAudio, transform.position, 1.0f);
			showFoodCalorie();
			showFoodImage();

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