using UnityEngine;
using System.Collections;

public class Ball_fire : MonoBehaviour {
	public Transform ball;
	Transform spPoint;
	public int power=1000;
	// Use this for initialization
	void Start () {
		spPoint = GameObject.Find ("spPoint").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
		   FireBullet();
	}
    void FireBullet()
    {
			Transform obj = Instantiate (ball, spPoint.position, Quaternion.identity) as Transform; 
		obj.rigidbody.AddForce (-1 * power, 0.3f*power, 0);

		
	}
}
