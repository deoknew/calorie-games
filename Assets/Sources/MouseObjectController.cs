using UnityEngine;
using System.Collections;

public class MouseObjectController : MonoBehaviour 
{
	private const float DEFAULT_Z_POSITION = 17.0f;

	void Update ()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = DEFAULT_Z_POSITION;
		
		Vector3 objectPos = Camera.main.ScreenToWorldPoint (mousePos);
		transform.position = objectPos;
	}
}
