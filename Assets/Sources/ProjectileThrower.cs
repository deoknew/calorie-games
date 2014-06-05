using UnityEngine;
using System.Collections;

public class ProjectileThrower : MonoBehaviour 
{
	private const string THROWING_POINT_NAME = "Throwing_Point";

	public Transform projectile1,projectile2,projectile3,projectile4,projectile5;

	public int powerX;
	public int powerY;
	public int powerZ;

	public int matKind;
	public float throwRate;

	private Transform startPoint;


	void Start () 
	{
		startPoint = GameObject.Find (THROWING_POINT_NAME).transform;
	}


	void Update () 
	{
		powerZ = Random.Range (2400, 3000);
		powerX = Random.Range (-700, 700);
		matKind = (int)Random.Range (1, 6);
		if (Random.Range (0, 1000) < (int)(throwRate * 100.0f))
			FireBullet ();
	}


    void FireBullet()
    {
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

	}
}