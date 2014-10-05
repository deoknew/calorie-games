using UnityEngine;
using System.Collections;

public class jumping2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.Range(0, 1000)< 10)
			transform.rigidbody.AddForce(Vector3.up * 300); 

	
	}
}
