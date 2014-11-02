using UnityEngine;
using System.Collections;
using EVGame.Action;

public class QuitGameButtonGUIEvent : GUIEvent 
{
	public override void onMoveEvent(float x, float y)
	{
		
	}

	public override void onClickEvent(float x, float y)
	{
		if (enabled == false)
			return;

		Camera guiCamera = Camera.allCameras[1];
		if (guiCamera != null)
			guiCamera.enabled = false;

		GameAction.onFinished handler = new GameAction.onFinished (onActionFinished);
		GameAction.invoke (this.gameObject, handler);
		enabled = false;
	}
	
	
	private void onActionFinished()
	{
		Application.LoadLevel("MainScene");
	}
}
