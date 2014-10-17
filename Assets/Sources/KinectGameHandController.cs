using UnityEngine;
using System.Collections;

public class KinectGameHandController : KinectHandController
{
	private const float JOINT_POSITION_MULTIPLIER = 8.0f;

	public Transform centerPoint;

	public Transform leftHand;
	public Transform rightHand;

	private Vector3 prevLeftHandPosition;
	private Vector3 prevRightHandPosition;


	public override void onUpdateHand(Vector3 leftHandPos, Vector3 rightHandPos)
	{
		bool gameRunning = GameManager.Instance.isGameRunning();

		setVisibleGameHands(gameRunning);

		if (false == gameRunning)
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
	
		float distance = 0.0f;

		if (prevLeftHandPosition.magnitude == 0) {
			prevLeftHandPosition = leftHandPos;
		}

		if (prevRightHandPosition.magnitude == 0) {
			prevRightHandPosition = rightHandPos;
		}

		distance += 
			Mathf.Sqrt(
				Mathf.Pow(Mathf.Abs(prevLeftHandPosition.x - leftHandPos.x), 2.0f) 
				+ Mathf.Pow(Mathf.Abs(prevLeftHandPosition.y - leftHandPos.y), 2.0f)
			);

		distance += 
			Mathf.Sqrt(
				Mathf.Pow(Mathf.Abs(prevRightHandPosition.x - rightHandPos.x), 2.0f) 
				+ Mathf.Pow(Mathf.Abs(prevRightHandPosition.y - rightHandPos.y), 2.0f)
			);

		RunningGameModule.Instance.addMovingDistance(distance);

		prevLeftHandPosition = leftHandPos;
		prevRightHandPosition = rightHandPos;
	}


	private void setVisibleGameHands(bool visible)
	{
		leftHand.gameObject.SetActive(visible);
		rightHand.gameObject.SetActive(visible);
	}
}
