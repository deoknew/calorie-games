using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EVActionGroup : MonoBehaviour 
{
	public EVAction[] actions;

	private int _currentIndex;
	private onFinished _finishHandler;

	private List<EVAction> _waitingPool;


	public void run()
	{
		if (isRunning())
			return;

		_currentIndex = -1;
		enabled = true;
	}


	public void run(onFinished finishHandler)
	{
		_finishHandler = finishHandler;

		run();
	}


	public bool isRunning()
	{
		return enabled;
	}


	public void stop()
	{
		if (_finishHandler != null)
			_finishHandler();

		_waitingPool.Clear();

		_currentIndex = -1;
		enabled = false;
	}


	void Start ()
	{
		_waitingPool = new List<EVAction>();
		enabled = false;
	}


	void Update () 
	{
		if (_waitingPool.Count > 0) {
			int index = 0;

			while(index < _waitingPool.Count) {
				EVAction action = _waitingPool[index];

				if (false == action.isEnabled()) {
					_waitingPool.Remove(action);
				} else {
					index++;
				}
			}
		} else {
			if (++_currentIndex >= actions.Length) {
				stop();
				return;
			}

			EVAction action = actions[_currentIndex];
			action.run();

			if (action.wait)
				_waitingPool.Add(action);
		}
	}


	public delegate void onFinished();
}
