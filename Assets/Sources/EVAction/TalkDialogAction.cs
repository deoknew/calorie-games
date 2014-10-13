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


	public override void onStart() 
	{
		if (nameGUIText != null) {
			if (nameText != null && nameText.Length > 0) {
				nameGUIText.text = nameText;

				EVAction.run(nameGUIText.gameObject);
			}
		}

		if (talkGUIText != null) {
			if (talkText != null && talkText.Length > 0) {
				talkGUIText.text = talkText;

				EVAction.run(talkGUIText.gameObject);
			}
		}

		if (portraitGUITexture != null) {
			if (portraitSprite != null) {
				portraitGUITexture.texture = portraitSprite.texture;

				EVAction.run(portraitGUITexture.gameObject);
			}
		}

		if (bgGUITexture != null) {
			EVAction.run(bgGUITexture.gameObject);
		}
	}


	public override void onStop()
	{
		if (nameGUIText != null) {
			EVAction.stop(nameGUIText.gameObject);
			nameGUIText.enabled = false;
		}
		
		if (talkGUIText != null) {
			EVAction.stop(talkGUIText.gameObject);
			talkGUIText.enabled = false;
		}
		
		if (portraitGUITexture != null) {
			EVAction.stop(portraitGUITexture.gameObject);
			portraitGUITexture.enabled = false;
		}
		
		if (bgGUITexture != null) {
			EVAction.stop(bgGUITexture.gameObject);
			bgGUITexture.enabled = false;
		}
	}


	public override void onAction(GameObject target, float progress)
	{
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
