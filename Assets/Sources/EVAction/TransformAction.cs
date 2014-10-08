using UnityEngine;
using System.Collections;

public class TransformAction : EVAction
{
	public Transform from;
	public Transform to;

	public bool translationEnabled = true;
	public bool rotationEnabled = true;
	public bool scaleEnabled = true;


	public override void onAction(GameObject target, float progress)
	{
		if (translationEnabled) {
			Vector3 position = Vector3.Slerp(from.position, to.position, progress);
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
