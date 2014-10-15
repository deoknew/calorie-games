using UnityEngine;
using System.Collections;

public class ProjectileThrower : MonoBehaviour 
{
	private const string THROWING_POINT_NAME = "Point_Thrower";
	private const int FIRST_SHOOT_POINT = 12;

	public Transform shooter;
	
	public AudioClip throwAudio;
	public Transform throwEffect;
	
	public float throwRate;
	
	private float waitTime = 0.4f;
	public void setWaitTime (float wtime)
	{
		waitTime = wtime;
	}

	private int _currentProjectileIndex = 0;
	
	private int currentPoint;
	private int nextPoint;
	private float startTime;
	private bool shooterMoving;

	private Transform startPoint;
	private Vector3[] shootPoints;
	
	private static ProjectileThrower instance;
	
	public static ProjectileThrower getInstance() {
		return instance;
	}

	IEnumerator Start()
	{
		instance = this;
		startPoint = GameObject.Find (THROWING_POINT_NAME).transform;

		currentPoint = -1;
		nextPoint = -1;

		while(true){
			yield return new WaitForSeconds(waitTime);

			if(GameManager.getInstance().isGameRunning()) {
				if (shootPoints == null)
					shootPoints = generateShootPoints();

				fireBullet ();
			}
		}
	}


	public void reset()
	{
		_currentProjectileIndex = 0;
	}


	private Vector3[] generateShootPoints()
	{
		Vector3[] sp = new Vector3[26];
		
		for(int i=0,x=-50; i<5; i++)
		{
			for(int j=0,y=25; j<5; j++)
			{
				sp[i*5+j]= new Vector3 (startPoint.position.x+x,startPoint.position.y+y,startPoint.position.z);
				y-=8;
			}
			x+=25;
		}
		return sp;
	}

	/*void Start () 
	{


	}
	/*

	void Update ()
	{
		if (false == GameManager.getInstance().isGameRunning())
			return;
		
		//matKind = (int)Random.Range (1, 9);
		//if (Random.Range (0, 1000) < (int)(throwRate * 30.0f))
			//FireBullet ();
	}
*/

    void fireBullet()
    {
		int[] bulletPool = GameManager.getInstance ().foodIdArray; 
		//Transform[] projectiles = GameManager.getInstance().projectiles;

		const int MIN_TORQUE = 2;
		const int MAX_TORQUE = 10;

		int index = bulletPool[_currentProjectileIndex];

		currentPoint = nextPoint;
		nextPoint = (int)Random.Range (0, 25);
		
		if (currentPoint == -1)
			currentPoint = FIRST_SHOOT_POINT;
		
		startShooterMoving ();

		/*
		//int powerZ = Random.Range (2600, 3100);
		int powerZ = 5000;
		int powerX = 0;
		int powerY = 0;

		//int index = (int)Random.Range(0, projectiles.Length-1);

		int InsertPower = 500;
		Vector3 forceVector = new Vector3(powerX, powerY, powerZ * -1);
		//
		if (currentPoint >= 0 && currentPoint <= 4) {
			if(currentPoint == 0)
				forceVector = new Vector3 (1300,-InsertPower, powerZ * -1);
			else if(currentPoint == 4)
				forceVector = new Vector3 (1300, InsertPower, powerZ * -1);
			else
				forceVector = new Vector3 (1300, powerY * 0.1f, powerZ * -1);
				}

		if (currentPoint >= 5 && currentPoint <= 9) {
			if(currentPoint == 5)
				forceVector = new Vector3 (500, -InsertPower, powerZ * -1);
			else if(currentPoint == 9)
				forceVector = new Vector3 (500, InsertPower, powerZ * -1);
			else	
				forceVector = new Vector3 (500, powerY * 0.1f, powerZ * -1);
				}

		if(currentPoint ==10)
			forceVector = new Vector3(powerX, -InsertPower, powerZ * -1);

		if (currentPoint >= 15 && currentPoint <= 19) {
			if(currentPoint == 15)
				forceVector = new Vector3 (-800,-InsertPower, powerZ * -1);
			else if(currentPoint == 19)
				forceVector = new Vector3 (-800, InsertPower, powerZ * -1);
			else
				forceVector = new Vector3 (-800, powerY * 0.1f, powerZ * -1);
				}

		if (currentPoint >= 20 && currentPoint <= 24) {
			if(currentPoint == 20)
				forceVector = new Vector3 (-1500, -InsertPower , powerZ * -1);
			else if(currentPoint == 24)
				forceVector = new Vector3 (-1500, InsertPower, powerZ * -1);
			else
				forceVector = new Vector3 (-1500, powerY * 0.1f, powerZ * -1);
		}
		*/

		Vector3 torqueVector = new Vector3();
		for (int i = 0; i < 3; ++i)
			torqueVector[i] = Random.Range(MIN_TORQUE, MAX_TORQUE);

		if (GameManager.getInstance ().IsFeverTime) {
			index = 13;
			_currentProjectileIndex--;
		}

		Transform projectile = GameManager.getInstance().projectiles[index];
		Vector3 shootPosition = shootPoints[currentPoint];
		Vector3 targetPosition = Camera.main.transform.position;

		targetPosition.x += Random.Range (-5.0f, 5.0f);
		targetPosition.y += 8.0f;
		float power = Random.Range (40, 50);

		fireToTarget (shootPosition, targetPosition, torqueVector, power, projectile);

		AudioSource.PlayClipAtPoint(throwAudio, startPoint.position, 1.0f);
	}


	void fireToTarget(Vector3 shootPosition, Vector3 targetPosition, Vector3 torqueVector, float power, Transform projectile)
	{
		_currentProjectileIndex++;

		Transform obj = Instantiate (projectile, shootPosition, Quaternion.identity) as Transform;
		obj.rigidbody.mass = 1.0f;
		obj.rigidbody.useGravity = true;

		float dist = Vector3.Distance(shootPosition, targetPosition);
		obj.transform.LookAt (targetPosition);

		Vector3 forceVector = obj.transform.forward * (power * dist);
		obj.rigidbody.AddForce (forceVector);
		obj.rigidbody.AddTorque (torqueVector);

		obj.transform.localScale *= 2;
	}


	void startShooterMoving()
	{
		startTime = Time.time;
		shooterMoving = true;

		if (throwEffect != null) {
			Transform obj = (Transform)Instantiate (throwEffect, shootPoints [currentPoint], Quaternion.LookRotation (shooter.up));
			Destroy (obj.gameObject, 0.5f);
		}
	}


	void Update()
	{
		if (shooterMoving) {
			float fracComplete = (Time.time - startTime) / waitTime;
			shooter.transform.position = Vector3.Slerp (shooter.transform.position, shootPoints [nextPoint], fracComplete);
		}
	}
}