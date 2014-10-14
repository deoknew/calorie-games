using UnityEngine;
using System.Collections;

public class EVActionGameModule : GameModule
{
	public EVAction[] actions;

	private int _currentIndex;

	
	protected override void onStart()
	{
		_currentIndex = 0;

		if (actions != null && actions.Length > 0)
			runAction(_currentIndex);
		else
			onModuleFinished();
	}


	protected override void onUpdate()
	{
	
	}


	protected override void onReceiveParams(Hashtable paramTable)
	{
		
	}


	private void onActionFinished()
	{
		_currentIndex++;

		if (_currentIndex < actions.Length) {
			runAction(_currentIndex);
		} else {
			onModuleFinished();
		}
	}


	private void onModuleFinished()
	{
		finish ();
	}


	private void runAction(int index)
	{
		EVAction.onFinished handler = new EVAction.onFinished(onActionFinished);
		EVAction.invoke(actions[index].gameObject, handler);
	}
}
