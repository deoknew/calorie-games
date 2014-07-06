using UnityEngine;
using System.Collections;

public class ProjectileThrower : MonoBehaviour 
{
	private const string THROWING_POINT_NAME = "Point_Thrower";
	
	public AudioClip throwAudio;
	public int powerY;
	public float throwRate;

	private Transform startPoint;


	void Start () 
	{
		startPoint = GameObject.Find (THROWING_POINT_NAME).transform;
	}


	void Update ()
	{
		if (false == GameManager.getInstance().isGameRunning())
			return;
		
		//matKind = (int)Random.Range (1, 9);
		if (Random.Range (0, 1000) < (int)(throwRate * 30.0f))
			FireBullet ();
	}


    void FireBullet()
    {
		Transform[] projectiles = GameManager.getInstance().projectiles;

		const int MIN_TORQUE = 0;
		const int MAX_TORQUE = 8;

		int powerZ = Random.Range (2600, 3100);
		int powerX = Random.Range (-420, 420);

		int index = (int)Random.Range(0, projectiles.Length-1);

		Vector3 forceVector = new Vector3(powerX, 0.3f * powerY, powerZ * -1);
		Vector3 torqueVector = new Vector3();
		for (int i = 0; i < 3; ++i)
			torqueVector[i] = Random.Range(MIN_TORQUE, MAX_TORQUE);

		Transform obj = Instantiate (projectiles[index], startPoint.position, Quaternion.identity) as Transform; 

		obj.rigidbody.AddForce (forceVector);
		obj.rigidbody.AddTorque (torqueVector);

		AudioSource.PlayClipAtPoint(throwAudio, startPoint.position, 1.0f);
	}
}