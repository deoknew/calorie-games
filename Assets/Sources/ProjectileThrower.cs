using UnityEngine;
using System.Collections;

public class ProjectileThrower : MonoBehaviour 
{
	private const string THROWING_POINT_NAME = "Point_Thrower";
	
	public AudioClip throwAudio;
	public int powerY;
	public float throwRate;

	private Transform startPoint;

	public float waitTime=0.4f;
	public int [] fireArray;
	public int k = 0;

	private static ProjectileThrower instance;
	
	public static ProjectileThrower getInstance() {
		return instance;
	}

	IEnumerator Start()
	{
		instance = this;
		startPoint = GameObject.Find (THROWING_POINT_NAME).transform;

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

		//******************************************
		int spPosition = (int)Random.Range (0, 5);
		Vector3 [] sp = new Vector3[6];
		sp[0] = new Vector3 (startPoint.position.x+8.0f, startPoint.position.y, startPoint.position.z);
		sp[1] = new Vector3 (startPoint.position.x-8.0f, startPoint.position.y, startPoint.position.z);
		sp[2] = new Vector3 (startPoint.position.x+8.0f, startPoint.position.y-5.0f, startPoint.position.z);
		sp[3] = new Vector3 (startPoint.position.x-8.0f, startPoint.position.y-5.0f, startPoint.position.z);
		sp[4] = new Vector3 (startPoint.position.x, startPoint.position.y-5.0f, startPoint.position.z);
		sp[5] = startPoint.position;
		//******************************************

		Vector3 forceVector = new Vector3(powerX, powerY*0.1f, powerZ * -1);
		Vector3 torqueVector = new Vector3();
		for (int i = 0; i < 3; ++i)
			torqueVector[i] = Random.Range(MIN_TORQUE, MAX_TORQUE);

		Fire (spPosition, index, sp, forceVector, torqueVector);

		AudioSource.PlayClipAtPoint(throwAudio, startPoint.position, 1.0f);

		k++;
	}

	void Fire(int spPosition, int index, Vector3 []sp, Vector3 forceVector, Vector3 torqueVector)
	{
		Transform obj;
		Transform[] projectiles = GameManager.getInstance().projectiles;
		if (spPosition == 0) 
		{
			obj = Instantiate (projectiles [index], sp[spPosition], Quaternion.identity) as Transform;
			obj.rigidbody.mass = 1.2f;
			obj.rigidbody.AddForce (forceVector);
			obj.rigidbody.AddTorque (torqueVector);
		}
		if (spPosition == 1)
		{
			obj = Instantiate (projectiles [index], sp[spPosition], Quaternion.identity) as Transform;
			obj.rigidbody.mass = 1.2f;
			obj.rigidbody.AddForce (forceVector);
			obj.rigidbody.AddTorque (torqueVector);
		}
		if (spPosition == 2) 
		{
			obj = Instantiate (projectiles [index], sp[spPosition], Quaternion.identity) as Transform;
			obj.rigidbody.mass = 1.2f;
			obj.rigidbody.AddForce (forceVector);
			obj.rigidbody.AddTorque (torqueVector);
		}
		if (spPosition == 3) 
		{
			obj = Instantiate (projectiles [index], sp[spPosition], Quaternion.identity) as Transform;
			obj.rigidbody.mass = 1.2f;
			obj.rigidbody.AddForce (forceVector);
			obj.rigidbody.AddTorque (torqueVector);
		}
		if (spPosition == 4) 
		{
			obj = Instantiate (projectiles [index], sp[spPosition], Quaternion.identity) as Transform;
			obj.rigidbody.mass = 1.2f;
			obj.rigidbody.AddForce (forceVector);
			obj.rigidbody.AddTorque (torqueVector);
		}
		if (spPosition == 5) 
		{
			obj = Instantiate (projectiles [index], sp[spPosition], Quaternion.identity) as Transform;
			obj.rigidbody.mass = 1.2f;
			obj.rigidbody.AddForce (forceVector);
			obj.rigidbody.AddTorque (torqueVector);
		}

	}
}