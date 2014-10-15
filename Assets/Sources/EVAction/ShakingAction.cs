using UnityEngine;
using System.Collections;

public class ShakingAction : EVAction 
{
	private const int MAX_SHAKING_FRAME = 5;

	public float shakingDistance;

	private Vector3 _startPosition;
	private Vector3[] _shakingData;


	public override void onStart (GameObject target)
	{
		base.onStart (target);

		_startPosition = targetObject.transform.position;
		_shakingData = new Vector3[MAX_SHAKING_FRAME];

		Vector3 vector;

		vector = new Vector3();
		vector.x = _startPosition.x -= shakingDistance;
		vector.y = _startPosition.y -= shakingDistance;
		vector.z = _startPosition.z -= shakingDistance;
		_shakingData[0] = vector;
		
		vector = new Vector3();
		vector.x = _startPosition.x += shakingDistance;
		vector.y = _startPosition.y += shakingDistance;
		vector.z = _startPosition.z += shakingDistance;
		_shakingData[1] = vector;
		
		vector = new Vector3();
		vector.x = _startPosition.x -= shakingDistance;
		vector.y = _startPosition.y += shakingDistance;
		vector.z = _startPosition.z -= shakingDistance;
		_shakingData[2] = vector;
		
		vector = new Vector3();
		vector.x = _startPosition.x += shakingDistance;
		vector.y = _startPosition.y -= shakingDistance;
		vector.z = _startPosition.z += shakingDistance;
		_shakingData[3] = vector;
		
		vector = new Vector3();
		vector.x = _startPosition.x;
		vector.y = _startPosition.y;
		vector.z = _startPosition.z;
		_shakingData[4] = vector;
	}


	public override void onStop (GameObject target)
	{
		base.onStop (target);

		target.transform.position = _startPosition;
	}


	public override void onAction(GameObject target, float progress)
	{
		Transform transform = target.transform;

		if (transform != null && progress < 1.0f) {
			int index = (int)(progress / (1.0f / MAX_SHAKING_FRAME));
			transform.position = _shakingData[index];
		}
	}
}