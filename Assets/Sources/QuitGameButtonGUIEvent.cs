using UnityEngine;
using System.Collections;

public class QuitGameButtonGUIEvent : GUIEvent 
{
	public override void onMoveEvent(float x, float y)
	{
		
	}
	
	
	public override void onClickEvent(float x, float y)
	{
		Application.LoadLevel("MainScene");
	}
}
