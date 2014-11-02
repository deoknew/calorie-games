using UnityEngine;
using System.Collections;
using EVGame.Module;
using EVGame.Action;

public class MainScreenManager : MonoBehaviour 
{
	public GameModule effectModule;
	public GameAction calibratedAction;
	public GameAction idleAction;
	public GameAction openingAction;

	private bool _calibrated;
	private bool _kinectUpdateLock;


	void Start () {
		if (effectModule != null) {
			effectModule.start ();
		}

		if (openingAction != null)
			GameAction.invoke (openingAction.gameObject);
	}


	void Update () 
	{
		if (_kinectUpdateLock)
			return;

		if (KinectManager.Instance != null) {
			if (KinectManager.Instance.IsUserDetected()) {
				if (!_calibrated) {
					GameAction.invoke(calibratedAction.gameObject, new GameAction.onFinished(onActionFinished));
					_kinectUpdateLock = true;
					_calibrated = true;
				}
			} else {
				GameAction.invoke(idleAction.gameObject, new GameAction.onFinished(onActionFinished));

				_kinectUpdateLock = true;
				_calibrated = false;
			}
		}
	}


	private void onActionFinished()
	{
		_kinectUpdateLock = false;
	}
}
