using UnityEngine;
using System.Collections;

public abstract class EVAction : MonoBehaviour 
{
	public GameObject[] targetObjects;
	public float duration;
	public bool wait;

	private float _currentTime;


	public static void run(GameObject gameObject)
	{
		EVAction[] actions = gameObject.GetComponents<EVAction>();

		foreach (EVAction action in actions) {
			if (action != null) {
				action.run();
			}
		}
	}


	void Start()
	{
		enabled = false;

		if (targetObjects.Length <= 0) {
			if (gameObject != null) {
				targetObjects = new GameObject[1];
				targetObjects[0] = gameObject;
			}
		}
	}


	void Update()
	{
		_currentTime += Time.deltaTime;

		float progress = _currentTime / duration;

		foreach (GameObject target in targetObjects)
			onAction (target, progress);

		if (progress >= 1.0f)
			stop ();
	}


	public GameObject getFirstTarget()
	{
		if (targetObjects.Length > 0)
			return targetObjects[0];

		return null;
	}


	public void run()
	{
		if (isEnabled())
			stop();

		_currentTime = 0.0f;
		enabled = true;

		onStart ();
	}


	public void pause()
	{
		enabled = false;
	}


	public void resume()
	{
		enabled = true;
	}


	public void stop()
	{
		_currentTime = 0.0f;
		enabled = false;

		onStop ();
	}


	public bool isEnabled()
	{
		return enabled;
	}


	public virtual void onStart() {}
	public virtual void onStop() {}
	public abstract void onAction(GameObject target, float progress);
}