using UnityEngine;
using System.Collections;
using System.Reflection;

public abstract class GameModule : MonoBehaviour 
{
	private OnFinishedDelegate _onFinished;
	public OnFinishedDelegate OnFinished {
		set { _onFinished = value; }
	}


	public bool isRunning()
	{
		return enabled;
	}


	void Start ()
	{
		enabled = false;
	}


	void Update()
	{
		onUpdate();
	}


	public void start()
	{
		enabled = true;
		onStart ();
	}


	public void start(Hashtable paramTable)
	{
		if (paramTable != null && paramTable.Count > 0)
			onReceiveParams(paramTable);

		start ();
	}


	public void finish()
	{
		enabled = false;

		if (_onFinished != null)
			_onFinished();
	}


	protected abstract void onStart();
	protected abstract void onReceiveParams(Hashtable paramTable);
	protected abstract void onUpdate();


	public delegate void OnFinishedDelegate();
}