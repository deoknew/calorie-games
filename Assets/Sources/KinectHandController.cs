using UnityEngine;
using System.Collections;

public class KinectHandController : MonoBehaviour 
{
	private KinectManager _kinectManager;


	void Update()
	{
		if (_kinectManager == null) {
			_kinectManager = KinectManager.Instance;
		}
		
		if (_kinectManager == null)
			return;

		uint playerID = KinectManager.Instance != null ? KinectManager.Instance.GetPlayer1ID() : 0;

		if (false == _kinectManager.IsPlayerCalibrated(playerID))
			return;

		Vector3 leftHandPos = _kinectManager.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.WristLeft);
		Vector3 rightHandPos = _kinectManager.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.WristRight);

		UpdateHand(leftHandPos, rightHandPos);
	}


	public virtual void UpdateHand(Vector3 leftHandPos, Vector3 rightHandPos)
	{
		return;
	}
}
