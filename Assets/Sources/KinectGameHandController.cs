using UnityEngine;
using System.Collections;

public class KinectGameHandController : KinectHandController
{
	private const float JOINT_POSITION_MULTIPLIER = 8.0f;

	public Transform centerPoint;

	public Transform leftHand;
	public Transform rightHand;


	public override void UpdateHand(Vector3 leftHandPos, Vector3 rightHandPos)
	{
		if (false == GameManager.getInstance().isGameRunning())
			return;

		leftHandPos.z *= -1;
		rightHandPos.z *= -1;
		
		leftHandPos *= JOINT_POSITION_MULTIPLIER;
		rightHandPos *= JOINT_POSITION_MULTIPLIER;
		
		leftHandPos.z /= 2.0f;
		rightHandPos.z /= 2.0f;
		
		Vector3 centerPos = centerPoint.position;
		
		leftHandPos += centerPos;
		rightHandPos += centerPos;
		
		leftHandPos.y -= 2.0f;
		rightHandPos.y -= 2.0f;
		
		if (leftHand != null) {
			leftHand.position = leftHandPos;
		}
		
		if (rightHand != null) {
			rightHand.position = rightHandPos;
		}
	}
}
