using UnityEngine;
using System.Collections;

public class TalkAction : EVAction
{
	private Hashtable _startTextTable;


	public override void onStart ()
	{
		base.onStart ();

		_startTextTable = new Hashtable();

		foreach (GameObject target in targetObjects) {
			int hashCode = target.GetHashCode();
			
			if (target.guiText != null) {
				_startTextTable.Add(hashCode, target.guiText.text);
				target.guiText.enabled = false;
			}
		}
	}


	public override void onStop ()
	{
		base.onStop ();

		foreach (GameObject target in targetObjects) {
			int hashCode = target.GetHashCode();
			
			if (target.guiText != null)
				target.guiText.text = (string)_startTextTable[hashCode];
		}
	}


	public override void onAction(GameObject target, float progress)
	{
		if (target.guiText == null)
			return;

		int hashCode = target.GetHashCode();
		string talkText = (string)_startTextTable[hashCode];

		if (talkText == null)
			return;

		int textLength = (int)(talkText.Length * progress);
		target.guiText.text = talkText.Substring(0, textLength);

		if (!target.guiText.enabled)
			target.guiText.enabled = true;
	}
}