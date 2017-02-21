using UnityEngine;
using System.Collections;
using EVGame.Action;

public class TutorialButtonGUIEvent : GUIEvent 
{
	public override void onMoveEvent(float x, float y)
	{
		
	}
	
	
	public override void onClickEvent(float x, float y)
	{
		if (enabled == false)
			return;

		GameAction.onFinished handler = new GameAction.onFinished (onActionFinished);
		GameAction.invoke (this.gameObject, handler);
		enabled = false;
	}


	private void onActionFinished()
	{
		RunningGameModule.Instance.startTutorial ();
	}
}