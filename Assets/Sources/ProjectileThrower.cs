using UnityEngine;
using System.Collections;

public class ProjectileThrower : MonoBehaviour 
{
	private const string THROWING_POINT_NAME = "Point_Thrower";

	public Transform shooter;
	
	public AudioClip throwAudio;
	public Transform throwEffect;
	
	public float throwRate;
	
	private float waitTime=0.4f;
	public void setWaitTime (float wtime)
	{
		waitTime = wtime;
	}
	private int [] fireArray;
	private int k = 0;

	private int fp = 0;

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

		Vector3[] sp = new Vector3[26];

		/*for(int i=0,x=-22; i<5; i++)
		{
			for(int j=0,y=10; j<5; j++)
			{
				sp[i*5+j]= new Vector3 (startPoint.position.x+x,startPoint.position.y+y,startPoint.position.z);
				y-=5;
			}
			x+=10;
		}*/
		////////////
		sp[0] = new Vector3 (startPoint.position.x-80.0f, startPoint.position.y+30.0f, startPoint.position.z);
		sp[1] = new Vector3 (startPoint.position.x-80.0f, startPoint.position.y+20.0f, startPoint.position.z);                                                                                                                                                                                                                                                                                                                                                                                                                  
		sp[2] = new Vector3 (startPoint.position.x-80.0f, startPoint.position.y+10.0f, startPoint.position.z);
		sp[3] = new Vector3 (startPoint.position.x-80.0f, startPoint.position.y, startPoint.position.z);
		sp[4] = new Vector3 (startPoint.position.x-80.0f, startPoint.position.y-10.0f, startPoint.position.z);

		sp[5] = new Vector3 (startPoint.position.x-50.0f, startPoint.position.y+6.0f, startPoint.position.z);
		sp[6] = new Vector3 (startPoint.position.x-50.0f, startPoint.position.y+3.0f, startPoint.position.z);
		sp[7] = new Vector3 (startPoint.position.x-50.0f, startPoint.position.y, startPoint.position.z);
		sp[8] = new Vector3 (startPoint.position.x-50.0f, startPoint.position.y-3.0f, startPoint.position.z);
		sp[9] = new Vector3 (startPoint.position.x-50.0f, startPoint.position.y-6.0f, startPoint.position.z);

		sp[10] = new Vector3 (startPoint.position.x-20.0f, startPoint.position.y+6.0f, startPoint.position.z);
		sp[11] = new Vector3 (startPoint.position.x-20.0f, startPoint.position.y+3.0f, startPoint.position.z);
		sp[12] = new Vector3 (startPoint.position.x-20.0f, startPoint.position.y, startPoint.position.z);
		sp[13] = new Vector3 (startPoint.position.x-20.0f, startPoint.position.y-3.0f, startPoint.position.z);
		sp[14] = new Vector3 (startPoint.position.x-20.0f, startPoint.position.y-6.0f, startPoint.position.z);

		sp[15] = new Vector3 (startPoint.position.x+10.0f, startPoint.position.y+6.0f, startPoint.position.z);
		sp[16] = new Vector3 (startPoint.position.x+10.0f, startPoint.position.y+3.0f, startPoint.position.z);
		sp[17] = new Vector3 (startPoint.position.x+10.0f, startPoint.position.y, startPoint.position.z);
		sp[18] = new Vector3 (startPoint.position.x+10.0f, startPoint.position.y-3.0f, startPoint.position.z);
		sp[19] = new Vector3 (startPoint.position.x+10.0f, startPoint.position.y-6.0f, startPoint.position.z);
	
		sp[20] = new Vector3 (startPoint.position.x+35.0f, startPoint.position.y+6.0f, startPoint.position.z);
		sp[21] = new Vector3 (startPoint.position.x+35.0f, startPoint.position.y+3.0f, startPoint.position.z);
		sp[22] = new Vector3 (startPoint.position.x+35.0f, startPoint.position.y, startPoint.position.z);
		sp[23] = new Vector3 (startPoint.position.x+35.0f, startPoint.position.y-3.0f, startPoint.position.z);
		sp[24] = new Vector3 (startPoint.position.x+35.0f, startPoint.position.y-6.0f, startPoint.position.z);
		/////////////
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

		//int powerZ = Random.Range (2600, 3100);
		int powerZ = 5000;
		int powerX = 0;
		int powerY = 0;

		//int index = (int)Random.Range(0, projectiles.Length-1);
		int index = fireArray[k];

		currentPoint = nextPoint;
		nextPoint = (int)Random.Range (17, 18);
		Debug.Log (nextPoint);
		if (currentPoint == -1)
			currentPoint = nextPoint;

		startShooterMoving ();
		int InsertPower = 500;
		Vector3 forceVector = new Vector3(powerX, powerY, powerZ * -1);
		//

		if (currentPoint >= 0 && currentPoint <= 4) {
			if(currentPoint == 0)
				forceVector = new Vector3 (5500,-1500, powerZ * -1);
			else if(currentPoint == 1)
				forceVector = new Vector3 (5500,-1300, powerZ * -1);
			else if(currentPoint == 4)
				forceVector = new Vector3 (5500, 2000, powerZ * -1);
			else
				forceVector = new Vector3 (5500, powerY * 0.1f, powerZ * -1);
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
		//
		Vector3 torqueVector = new Vector3();
		for (int i = 0; i < 3; ++i)
			torqueVector[i] = Random.Range(MIN_TORQUE, MAX_TORQUE);

		Fire (currentPoint, index, forceVector, torqueVector);

		AudioSource.PlayClipAtPoint(throwAudio, startPoint.position, 1.0f);

		Debug.Log ("delta time = " + Time.deltaTime);
	}

	void Fire(int shootPoint, int projectileIndex, Vector3 forceVector, Vector3 torqueVector)
	{
		Transform obj;
		Transform[] projectiles = GameManager.getInstance().projectiles;
		k++;

		if (GameManager.getInstance ().IsFeverTime == true)
		{		
			Debug.Log ("발사 수: "+fp);
			fp++;
			projectileIndex = 13;
			k--;
		}
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