using UnityEngine;
using System.Collections;

public class StartButtonGUIEvent : GUIEvent
{
	public override void onMoveEvent(float x, float y)
	{

	}


	public override void onClickEvent(float x, float y)
	{
		Application.LoadLevel("GameScene");
	}
}
