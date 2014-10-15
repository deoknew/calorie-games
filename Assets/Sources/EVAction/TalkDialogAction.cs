using UnityEngine;
using System.Collections;

public class TalkDialogAction : EVAction 
{
	public GUIText nameGUIText;
	public GUIText talkGUIText;
	public GUITexture portraitGUITexture;
	public GUITexture bgGUITexture;

	public string nameText;
	public string talkText;
	public Sprite portraitSprite;

	public EVAction appearAction;
	public EVAction disappearAction;


	public override void onStart(GameObject target) 
	{
		base.onStart (target);

		if (nameGUIText != null) {
			if (nameText != null && nameText.Length > 0) {
				nameGUIText.enabled = false;
				nameGUIText.text = nameText;
			}
		}

		if (talkGUIText != null) {
			if (talkText != null && talkText.Length > 0) {
				talkGUIText.enabled = false;
				talkGUIText.text = talkText;
			}
		}

		if (portraitGUITexture != null) {
			if (portraitSprite != null) {
				portraitGUITexture.enabled =false;
				portraitGUITexture.texture = portraitSprite.texture;
			}
		}

		if (appearAction != null) {
			EVAction.invoke(appearAction.gameObject);
		}

		if (disappearAction != null) {
			if (_finishHandler != null)
				disappearAction.setOnFinished (_finishHandler);

			_finishHandler = null;
		}
	}


	public override void onStop(GameObject target)
	{
		base.onStop (target);

		if (disappearAction != null)
			EVAction.invoke (disappearAction.gameObject);
	}


	public override void onAction(GameObject target, float progress)
	{
		if (progress < 0.05f)
			return;

		if (nameGUIText != null && !nameGUIText.enabled)
			nameGUIText.enabled = true;

		if (talkGUIText != null && !talkGUIText.enabled)
			talkGUIText.enabled = true;

		if (portraitGUITexture != null && !portraitGUITexture.enabled)
			portraitGUITexture.enabled = true;

		if (bgGUITexture != null && !bgGUITexture.enabled)
			bgGUITexture.enabled = true;
	}
}
