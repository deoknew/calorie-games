using UnityEngine;
using System.Collections;

public class ProjectileThrower : MonoBehaviour 
{
	private const string THROWING_POINT_NAME = "Throwing_Point";

	public Transform projectile;

	public int powerX;
	public int powerY;
	public int powerZ;

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

		if (Random.Range (0, 1000) < (int)(throwRate * 100.0f))
			FireBullet ();
	}


    void FireBullet()
    {
		Transform obj = Instantiate (projectile, startPoint.position, Quaternion.identity) as Transform; 
		obj.rigidbody.AddForce (powerX, 0.3f * powerY, powerZ * -1);
	}
}