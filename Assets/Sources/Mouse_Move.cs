using UnityEngine;
using System.Collections;

public class Mouse_Move : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}
	void Move()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 17;
		
		Vector3 objectPos = Camera.main.ScreenToWorldPoint (mousePos);
		
		transform.position = objectPos;

	}
}
