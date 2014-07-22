using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KinectCursorHandController : KinectHandController
{
	private const float NULL_POSITION = 2014.7f;
	private const float CHECK_TIME = 0.15f;
	private const float CLICK_THRESHOLD = 0.2f;

	private float _prevZ;
	private float _tickTime;

	public GameObject kinectGUIHandler;


	void Start()
	{
		_prevZ = NULL_POSITION;
	}


	public override void onUpdateHand(Vector3 leftHandPos, Vector3 rightHandPos)
	{
		if (false == isPlayerCalibrated()) {
			setVisibleGameCursor(false);
		}

		KinectGUIHandler handlerScript = kinectGUIHandler.GetComponent<KinectGUIHandler>();

		_tickTime += Time.deltaTime;

		rightHandPos.x += 0.3f;
		rightHandPos.y -= 0.5f;
		
		if (_prevZ == NULL_POSITION && rightHandPos.z > 0.0f) {
			_prevZ = rightHandPos.z;

		} else if (_prevZ != NULL_POSITION) {
			if (_tickTime >= CHECK_TIME) {
				if (_prevZ - rightHandPos.z > CLICK_THRESHOLD) {
					if (handlerScript != null) {
						handlerScript.receiveClickEvent(rightHandPos.x, rightHandPos.y);
					}
				}
				_tickTime = 0.0f;
				_prevZ = rightHandPos.z;
			}
		}

		if (handlerScript != null) {
			handlerScript.receiveMoveEvent(rightHandPos.x, rightHandPos.y);
		}
		
		transform.position = rightHandPos;
	}


	private void setVisibleGameCursor(bool visible)
	{
		gameObject.guiTexture.enabled = visible;
	}
}