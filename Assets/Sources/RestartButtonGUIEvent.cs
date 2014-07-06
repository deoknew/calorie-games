using UnityEngine;
using System.Collections;

public class RestartButtonGUIEvent : GUIEvent 
{
	public override void onMoveEvent(float x, float y)
	{
		
	}
	
	
	public override void onClickEvent(float x, float y)
	{
		GameManager.getInstance().restart();
	}
}
