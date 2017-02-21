using UnityEngine;
using System.Collections;
using EVGame.Module;

public class SkipButtonGUIEvent : GUIEvent
{
	public override void onMoveEvent(float x, float y)
	{
		base.onMoveEvent (x, y);
	}
	
	
	public override void onClickEvent(float x, float y)
	{
		base.onClickEvent (x, y);

		if (enabled == false)
			return;

		if (GameManager.Instance.isGameOpening ()) {
			ActionGameModule openingModule = (ActionGameModule)GameManager.Instance.openingModule;
			openingModule.skip();
			enabled = false;
		}
	}
}
