using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KinectCursorHandController : KinectHandController
{
	private const float NULL_POSITION = 2014.7f;
	private const float CHECK_TIME = 0.35f;
	private const float CLICK_THRESHOLD = 0.15f;
	private const float OFFSET_X = 0.5f;
	private const float OFFSET_Y = 1.5f;
	private const float SCALE_X = 1.5f;
	private const float SCALE_Y = 1.3f;


	private float _prevZ;
	private float _tickTime;

	public GameObject kinectGUIHandler;
	public GameObject clickParticle;
	public bool pushClickEnabled = false;
	public bool ignoreGameManager = false;


	void Start()
	{
		_prevZ = NULL_POSITION;
	}


	private void showClickParticle(Vector3 position)
	{
		if (clickParticle != null) {
			Vector3 realPosition = Camera.allCameras[1].ViewportToWorldPoint(position);
			realPosition.z = 0.0f;

			GameObject obj = (GameObject)Instantiate (clickParticle, realPosition, Quaternion.identity);
			obj.layer = LayerMask.NameToLayer("GUI");
			Destroy(obj, 2);
		}
	}


	public override void onUpdateHand(Vector3 leftHandPos, Vector3 rightHandPos)
	{
		if (!ignoreGameManager) {
			if (GameManager.Instance == null)
				return;

			if (GameManager.Instance.isGameOpening())
				setVisibleGameCursor(true);

			if (GameManager.Instance.isGameRunning ()) {
				if (RunningGameModule.Instance.tutorialModule.enabled) {
					setVisibleGameCursor (false);
					return;
				} else if (!RunningGameModule.Instance.isTutorialRunning ()) {
					setVisibleGameCursor (false);
					return;
				}
			}
			
			if (GameManager.Instance.isGameResult ())
				setVisibleGameCursor (true);
		}

		if (false == isPlayerCalibrated()) {
			setVisibleGameCursor(false);
		}

		KinectGUIHandler handlerScript = kinectGUIHandler.GetComponent<KinectGUIHandler>();

		_tickTime += Time.deltaTime;

		rightHandPos.x *= SCALE_X;
		rightHandPos.y *= SCALE_Y;

		rightHandPos.x += OFFSET_X;
		rightHandPos.y -= OFFSET_Y;
		
		if (_prevZ == NULL_POSITION && rightHandPos.z > 0.0f) {
			_prevZ = rightHandPos.z;

		} else if (_prevZ != NULL_POSITION) {
			if (pushClickEnabled) {
				if (_tickTime >= CHECK_TIME) {
					if (_prevZ - rightHandPos.z > CLICK_THRESHOLD) {
						if (handlerScript != null) {
							handlerScript.receiveClickEvent(rightHandPos.x, rightHandPos.y);
							showClickParticle(rightHandPos);
						}
					}
					_tickTime = 0.0f;
					_prevZ = rightHandPos.z;
				}
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