using UnityEngine;
using System.Collections;

public class TalkAction : EVAction
{
	private string _startText;


	public override void onStart ()
	{
		base.onStart ();

		if (targetObject.guiText != null) {
			_startText = targetObject.guiText.text;
			targetObject.guiText.enabled = false;
		}
	}


	public override void onStop ()
	{
		base.onStop ();

		if (targetObject.guiText != null)
			targetObject.guiText.text = _startText;
	}


	public override void onAction(GameObject target, float progress)
	{
		if (targetObject.guiText == null)
			return;

		string talkText = _startText;

		if (talkText == null)
			return;

		int textLength = (int)(talkText.Length * progress);
		targetObject.guiText.text = talkText.Substring(0, textLength);

		if (!targetObject.guiText.enabled)
			targetObject.guiText.enabled = true;
	}
}