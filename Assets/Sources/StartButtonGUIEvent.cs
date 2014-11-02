using UnityEngine;
using System.Collections;
using EVGame.Action;

public class StartButtonGUIEvent : GUIEvent
{
	public GUIText tipGuiText;
	public string tipText;


	public override void onMoveEvent(float x, float y)
	{
		if (tipGuiText != null)
			tipGuiText.text = tipText;
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
		Application.LoadLevel("GameScene");
	}
}