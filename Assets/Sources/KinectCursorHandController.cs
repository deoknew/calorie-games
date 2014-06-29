using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KinectCursorHandController : KinectHandController 
{
	public override void UpdateHand(Vector3 leftHandPos, Vector3 rightHandPos)
	{
		if (false == GameManager.getInstance().isGameResult())
			return;

		rightHandPos.x += 0.3f;
		rightHandPos.y -= 0.5f;
		rightHandPos.z = 0.0f;

		transform.position = rightHandPos;
	}
}