using UnityEngine;
using System.Collections;

public class TransformAction : EVAction
{
	public Transform from;
	public Transform to;
	
	public bool translationEnabled = true;
	public bool rotationEnabled = true;
	public bool scaleEnabled = true;


	public override void onStart (GameObject target)
	{
		base.onStart (target);

		target.transform.position = from.transform.position;
		target.transform.rotation = from.transform.rotation;
		target.transform.localScale = from.transform.localScale;
	}


	public override void onStop (GameObject target)
	{
		base.onStop (target);
	}


	public override void onAction(GameObject target, float progress)
	{
		Vector3 fromVector = from.transform.position;

		if (translationEnabled) {
			Vector3 position = Vector3.Slerp(fromVector, to.position, progress);
			target.transform.position = position;
		}

		if (rotationEnabled) {
			Quaternion rotation = Quaternion.Slerp(from.rotation, to.rotation, progress);
			target.transform.rotation = rotation;
		}

		if (scaleEnabled) {
			Vector3 scale = Vector3.Slerp(from.localScale, to.localScale, progress);
			target.transform.localScale = scale;
		}
	}
}
