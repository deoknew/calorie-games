using UnityEngine;
using System.Collections;
using EVGame.Action;

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
		if (transform.position.z <= 2.5 || GameManager.Instance.isGameFinished())
		{
			RunningGameModule.Instance.resetCombo();
			Destroy (gameObject);
		}
	}


	void OnCollisionEnter (Collision collision)
	{
		if (!collision.collider.tag.Equals("Non-Explosion"))
		{
			if (foodId == 12) // FeverTime
				RunningGameModule.Instance.startFeverTime();

			if (foodId == 11) // Bomb
				GameAction.invoke(Camera.main.gameObject);

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
		RunningGameModule.Instance.addCombo ();
	}


	void showFoodImage()
	{
		RunningGameModule.Instance.showFoodImage (imageName);
	}


	void showFoodCalorie()
	{
		RunningGameModule.Instance.showCalorie (calorie);
	}


	void consumeFood()
	{
		RunningGameModule.Instance.addScore(calorie);
	}
	void CollsionFoodID()
	{
		RunningGameModule.Instance.CheckFoodCrash (foodId);

	}


}