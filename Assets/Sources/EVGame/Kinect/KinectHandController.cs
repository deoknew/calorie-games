using UnityEngine;
using System.Collections;

public abstract class KinectHandController : MonoBehaviour 
{
	private KinectManager _kinectManager;


	public bool isKinectConnected() 
	{
		return (_kinectManager != null);
	}


	public bool isPlayerCalibrated()
	{
		if (isKinectConnected()) {
			uint playerID = KinectManager.Instance != null ? KinectManager.Instance.GetPlayer1ID() : 0;
			return _kinectManager.IsPlayerCalibrated(playerID);
		}
		return false;
	}


	void Update()
	{
		if (_kinectManager == null) {
			_kinectManager = KinectManager.Instance;
		}

		if (false == isKinectConnected()) {
			return;
		}

		if (false == isPlayerCalibrated()) {
			return;
		}

		uint playerID = KinectManager.Instance != null ? KinectManager.Instance.GetPlayer1ID() : 0;

		Vector3 leftHandPos = _kinectManager.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.WristLeft);
		Vector3 rightHandPos = _kinectManager.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.WristRight);

		onUpdateHand(leftHandPos, rightHandPos);
	}

	
	public abstract void onUpdateHand(Vector3 leftHandPos, Vector3 rightHandPos);
}
