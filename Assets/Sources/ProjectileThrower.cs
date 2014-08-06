using UnityEngine;
using System.Collections;

public class ProjectileThrower : MonoBehaviour 
{
	private const string THROWING_POINT_NAME = "Point_Thrower";

	public Transform shooter;
	
	public AudioClip throwAudio;
	public Transform throwEffect;

	public int powerY;
	public float throwRate;
	
	public float waitTime;
	public int [] fireArray;
	public int k = 0;
	
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

		Vector3[] sp = new Vector3[6];
		sp[0] = new Vector3 (startPoint.position.x+8.0f, startPoint.position.y, startPoint.position.z);
		sp[1] = new Vector3 (startPoint.position.x-8.0f, startPoint.position.y, startPoint.position.z);
		sp[2] = new Vector3 (startPoint.position.x+8.0f, startPoint.position.y-5.0f, startPoint.position.z);
		sp[3] = new Vector3 (startPoint.position.x-8.0f, startPoint.position.y-5.0f, startPoint.position.z);
		sp[4] = new Vector3 (startPoint.position.x, startPoint.position.y-5.0f, startPoint.position.z);
		sp[5] = startPoint.position;

		shootPoints = sp;

		while(true){
			yield return new WaitForSeconds(waitTime);

			if(GameManager.getInstance().isGameRunning()==true)
				FireBullet ();
		}
	}
	public void InitK()
	{
		k = 0;
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

    void FireBullet()
    {
		fireArray = GameManager.getInstance ().foodIdArray; 
		//Transform[] projectiles = GameManager.getInstance().projectiles;

		const int MIN_TORQUE = 0;
		const int MAX_TORQUE = 8;

		int powerZ = Random.Range (2600, 3100);
		//int powerX = Random.Range (-300, 300);
		int powerX = 0;
		//int index = (int)Random.Range(0, projectiles.Length-1);
		int index = fireArray[k];

		currentPoint = nextPoint;
		nextPoint = (int)Random.Range (0, 5);

		if (currentPoint == -1)
			currentPoint = nextPoint;

		startShooterMoving ();

		Vector3 forceVector = new Vector3(powerX, powerY*0.1f, powerZ * -1);
		Vector3 torqueVector = new Vector3();
		for (int i = 0; i < 3; ++i)
			torqueVector[i] = Random.Range(MIN_TORQUE, MAX_TORQUE);

		Fire (currentPoint, index, forceVector, torqueVector);

		AudioSource.PlayClipAtPoint(throwAudio, startPoint.position, 1.0f);

		k++;
	}

	void Fire(int shootPoint, int projectileIndex, Vector3 forceVector, Vector3 torqueVector)
	{
		Transform obj;
		Transform[] projectiles = GameManager.getInstance().projectiles;

		obj = Instantiate (projectiles [projectileIndex], shootPoints[shootPoint], Quaternion.identity) as Transform;
		obj.rigidbody.mass = 1.2f;
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
			shooter.transform.position = Vector3.Slerp (shootPoints [currentPoint], shootPoints [nextPoint], fracComplete);
		}
	}
}