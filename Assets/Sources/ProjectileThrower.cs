﻿using UnityEngine;
using System.Collections;

public class ProjectileThrower : MonoBehaviour 
{
	private const string THROWING_POINT_NAME = "Point_Thrower";
	
	public Transform[] projectiles;
	public AudioClip throwAudio;

	//public Transform projectile1, projectile2, projectile3, projectile4, projectile5;
	//public Transform projectile6, projectile7, projectile8;

	//public int powerX;
	public int powerY;
	//public int powerZ;

	//public int matKind;
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
		if (Random.Range (0, 1000) < (int)(throwRate * 100.0f))
			FireBullet ();
	}


    void FireBullet()
    {
		const int MIN_TORQUE = 0;
		const int MAX_TORQUE = 8;

		int powerZ = Random.Range (2850, 3200);
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

		/*
		if (matKind == 1) 
		{
			Transform obj = Instantiate (projectile1, startPoint.position, Quaternion.identity) as Transform; 
			obj.rigidbody.AddForce (powerX, 0.3f * powerY, powerZ * -1);
		}
		else if (matKind == 2) 
		{
			Transform obj = Instantiate (projectile2, startPoint.position, Quaternion.identity) as Transform; 
			obj.rigidbody.AddForce (powerX, 0.3f * powerY, powerZ * -1);
		}
		
		else if (matKind == 3) 
		{
			Transform obj = Instantiate (projectile3, startPoint.position, Quaternion.identity) as Transform; 
			obj.rigidbody.AddForce (powerX, 0.3f * powerY, powerZ * -1);
		}
		
		else if (matKind == 4) 
		{
			Transform obj = Instantiate (projectile4, startPoint.position, Quaternion.identity) as Transform; 
			obj.rigidbody.AddForce (powerX, 0.3f * powerY, powerZ * -1);
		}
		else if (matKind == 5) 
		{
			Transform obj = Instantiate (projectile5, startPoint.position, Quaternion.identity) as Transform; 
			obj.rigidbody.AddForce (powerX, 0.3f * powerY, powerZ * -1);
		}
		else if (matKind == 6) 
		{
			Transform obj = Instantiate (projectile6, startPoint.position, Quaternion.identity) as Transform; 
			obj.rigidbody.AddForce (powerX, 0.3f * powerY, powerZ * -1);
		}
		else if (matKind == 7) 
		{
			Transform obj = Instantiate (projectile7, startPoint.position, Quaternion.identity) as Transform; 
			obj.rigidbody.AddForce (powerX, 0.3f * powerY, powerZ * -1);
		}
		else if (matKind == 8) 
		{
			Transform obj = Instantiate (projectile8, startPoint.position, Quaternion.identity) as Transform; 
			obj.rigidbody.AddForce (powerX, 0.3f * powerY, powerZ * -1);
		}
		*/
	}
}