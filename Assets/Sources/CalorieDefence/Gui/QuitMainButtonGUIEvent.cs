using UnityEngine;
using System.Collections;

public class QuitMainButtonGUIEvent : GUIEvent 
{
	public override void onMoveEvent(float x, float y)
	{
		
	}
	
	
	public override void onClickEvent(float x, float y)
	{
		Application.Quit();
	}
}
