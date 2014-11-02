using UnityEngine;
using System.Collections;
using EVGame.Module;
using EVGame.Action;

public class TutorialGameModule : ActionGameModule 
{
	public GameObject guiLayer;

	public GameObject leftHandObject;
	public GameObject rightHandObject;

	public GameAction successAction;
	public GameAction failedAction;

	public AudioClip bgAudio;

	private int _defenceCount = 0;


	protected override void onUpdate ()
	{
		base.onUpdate ();

		if (Input.GetKeyUp (KeyCode.P)) {
			_currentIndex = 15;
			onActionFinished();
		}
	}


	protected override void onStart ()
	{
		base.onStart ();

		if (guiLayer != null)
			guiLayer.SetActive (true);

		leftHandObject.SetActive (true);
		rightHandObject.SetActive (true);

		if (bgAudio) {
			AudioUtils.stopAllAudioSources();
			AudioSource.PlayClipAtPoint(bgAudio, transform.position, 0.8f);
		}
	}


	protected override void onFinish ()
	{
		base.onFinish ();

		if (guiLayer != null)
			guiLayer.SetActive (false);
	}


	protected override void onActionFinished()
	{
		//(!) 임시로 인덱스 고정해서 사용
		if (_currentIndex == 14) {
			if (_defenceCount < 3) {
				ProjectileThrower.getInstance().fireBulletForTutorial();
				return;
			}
		}
		base.onActionFinished ();
	}


	public void successDefence()
	{
		_defenceCount++;

		if (successAction != null) {
			if (_defenceCount < 3) {
				GameAction.invoke (successAction.gameObject, new GameAction.onFinished (onActionFinished));
			} else {
				onActionFinished ();
			}
		}
	}


	public void failedDefence()
	{
		if (failedAction != null)
			GameAction.invoke (failedAction.gameObject, new GameAction.onFinished(onActionFinished));
	}
}
