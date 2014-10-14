using UnityEngine;
using System.Collections;

public abstract class EVAction : MonoBehaviour 
{
	public GameObject targetObject;
	public float duration;
	public bool wait;

	private float _currentTime;
	private onFinished _finishHandler;

	
	public static void invoke(GameObject gameObject)
	{
		EVAction.invoke(gameObject, null);
	}


	public static void invoke(GameObject gameObject, onFinished finishHandler)
	{
		EVAction[] actions = gameObject.GetComponents<EVAction>();
		
		if (actions == null)
			return;

		if (actions.Length > 0) {
			EVAction action = actions[0];

			action.setOnFinished(finishHandler);
			action.startActions(actions);
		}
	}


	IEnumerator runAction(EVAction action)
	{
		if (action == null)
			yield break;

		action.run();
		
		float delay = (action.wait) ? action.duration : 0.0f;
		yield return new WaitForSeconds(delay);
	}


	IEnumerator runActions(EVAction[] actions)
	{
		for (int i = 0; i < actions.Length; ++i) {
			yield return StartCoroutine(runAction(actions[i]));
		}

		if (_finishHandler != null)
			_finishHandler();
	}


	private void startActions(EVAction[] actions)
	{
		StartCoroutine(runActions(actions));
	}
	
	
	private void startAction(EVAction action)
	{
		EVAction[] actions = new EVAction[1];
		actions[0] = action;
		
		StartCoroutine(runActions(actions));
	}

	
	private void run()
	{
		if (isEnabled())
			stop();
		
		_currentTime = 0.0f;
		enabled = true;
		
		onStart ();
	}


	private void stop()
	{
		_currentTime = 0.0f;
		enabled = false;
		
		onStop ();
	}
	

	private void startAction()
	{
		startAction(this);
	}


	public void invoke()
	{
		startAction();
	}


	public static void interrupt(GameObject gameObject)
	{
		EVAction[] actions = gameObject.GetComponents<EVAction>();
		
		foreach (EVAction action in actions) {
			if (action != null) {
				action.stop();
			}
		}
	}


	public void setOnFinished(onFinished finishHandler)
	{
		_finishHandler = finishHandler;
	}


	void Start()
	{
		enabled = false;

		if (targetObject == null) {
			if (gameObject != null)
				targetObject = gameObject;
		}
	}


	void Update()
	{
		_currentTime += Time.deltaTime;

		float progress = _currentTime / duration;
		onAction (targetObject, progress);

		if (progress >= 1.0f)
			stop ();
	}
	

	public void pause()
	{
		enabled = false;
	}


	public void resume()
	{
		enabled = true;
	}


	public bool isEnabled()
	{
		return enabled;
	}


	public virtual void onStart() {}
	public virtual void onStop() {}
	public abstract void onAction(GameObject target, float progress);

	public delegate void onFinished();
}