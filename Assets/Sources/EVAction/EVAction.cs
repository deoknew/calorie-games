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
		EVAction action = gameObject.GetComponent<EVAction>();

		if (action != null) {
			action.run();
		}
	}


	void Start()
	{
		enabled = false;
	}


	void Update()
	{
		_currentTime += Time.deltaTime;

		float progress = _currentTime / duration;

		foreach (GameObject target in targetObjects)
			onAction(target, progress);

		if (progress >= 1.0f)
			stop ();
	}


	public void run()
	{
		_currentTime = 0.0f;
		enabled = true;
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
	}


	public bool isEnabled()
	{
		return enabled;
	}


	public abstract void onAction(GameObject target, float progress);
}