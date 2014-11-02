using UnityEngine;
using System.Collections;

namespace EVGame.Action
{
	public class TalkAction : GameAction
	{
		public string talkText;
		public float talkDuration;
		
		
		public override void onStart (GameObject target)
		{
			base.onStart (target);
		}
		
		
		public override void onStop (GameObject target)
		{
			base.onStop (target);
		}
		
		
		public override void onAction(GameObject target, float progress)
		{
			if (target.guiText == null)
				return;

			if (talkText == null)
				return;

			float talkProgress = progress * (duration / talkDuration);
			if (talkProgress > 1.0f)
				talkProgress = 1.0f;

			int textLength = (int)(talkText.Length * talkProgress);
			target.guiText.text = talkText.Substring(0, textLength);
		}
	}
}