using UnityEngine;
using System.Collections;

namespace EVGame.Action
{
	public class TalkDialogAction : GameAction 
	{
		public GUIText nameGUIText;
		public GUIText talkGUIText;
		public GUITexture portraitGUITexture;
		public GUITexture bgGUITexture;
		
		public string nameText;
		public string talkText;
		public Sprite portraitSprite;
		
		public GameAction appearAction;
		public GameAction disappearAction;
		
		
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
				GameAction.invoke(appearAction.gameObject);
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
				GameAction.invoke (disappearAction.gameObject);
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
}
