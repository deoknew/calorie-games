using UnityEngine;
using System.Collections;

public class TalkAction : EVAction
{
	private string _startText;


	public override void onStart (GameObject target)
	{
		base.onStart (target);

		if (target.guiText != null) {
			_startText = targetObject.guiText.text;
			target.guiText.enabled = false;
		}
	}


	public override void onStop (GameObject target)
	{
		base.onStop (target);

		if (target.guiText != null)
			target.guiText.text = _startText;
	}


	public override void onAction(GameObject target, float progress)
	{
		if (target.guiText == null)
			return;

		string talkText = _startText;

		if (talkText == null)
			return;

		int textLength = (int)(talkText.Length * progress);
		target.guiText.text = talkText.Substring(0, textLength);
	}
}