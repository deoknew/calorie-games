using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour 
{
	private const float JOINT_POSITION_MULTIPLIER = 10.0f;

	public Transform centerPoint;

	public Transform leftHand;
	public Transform rightHand;

	private KinectManager _kinectManager;


	void Start()
	{

	}


	void Update()
	{
		if (_kinectManager == null) {
			_kinectManager = KinectManager.Instance;
		}
		
		if (_kinectManager == null)
			return;

		uint playerID = KinectManager.Instance != null ? KinectManager.Instance.GetPlayer1ID() : 0;

		Vector3 leftHandPos = _kinectManager.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.WristLeft);
		Vector3 rightHandPos = _kinectManager.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.WristRight);
		//Quaternion leftHandRot = _kinectManager.GetJointOrientation(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.WristLeft, true);
		//Quaternion rightHandRot = _kinectManager.GetJointOrientation(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.WristRight, true);

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
