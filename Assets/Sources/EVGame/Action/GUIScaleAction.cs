using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EVGame.Action
{
	public class GUIScaleAction : GameAction 
	{
		public float from;
		public float to;
		
		private object _startSize;
		
		
		public override void onStart(GameObject target)
		{
			base.onStart(target);
			
			GUIText text = target.guiText;
			GUITexture texture = target.guiTexture;
			
			if (text != null) {
				_startSize = text.fontSize.ToString();
				
			} else if (texture != null) {
				if (texture.pixelInset.x <= 0.0f && texture.pixelInset.y <= 0.0f)
					_startSize = texture.transform.localScale;
				else
					_startSize = new Rect(texture.pixelInset);
			}
		}
		
		
		public override void onStop (GameObject target)
		{
			base.onStop (target);
			
			if (_startSize == null)
				return;
			
			GUITexture texture = target.guiTexture;
			GUIText text = target.guiText;
			
			if (texture != null) {
				if (_startSize != null) {
					if (_startSize.GetType() == typeof(Rect)) {
						texture.pixelInset = (Rect)_startSize;
					} else if (_startSize.GetType() == typeof(Vector3)) {
						texture.transform.localScale = (Vector3)_startSize;
					}
				}
				
			} else if (text != null) {
				float size = float.Parse(_startSize.ToString());
				text.fontSize = (int)size;
			}
		}
		
		
		public override void onAction(GameObject target, float progress)
		{
			GUITexture texture = target.guiTexture;
			GUIText text = target.guiText;
			
			float value = Mathf.Lerp (from, to, progress);
			
			if (texture != null) {
				object obj = _startSize;
				
				if (obj.GetType() == typeof(Rect)) {
					Rect rect = (Rect)obj;
					rect.width *= value;
					rect.height *= value;
					
					texture.pixelInset = rect;
				} else if (obj.GetType() == typeof(Vector3)) {
					Vector3 vector = (Vector3)obj;
					vector.x *= value;
					vector.y *= value;
					vector.z *= value;
					
					texture.transform.localScale = vector;
				}
				
			} else if (text != null) {
				float size = float.Parse(_startSize.ToString());
				text.fontSize = (int)(size * value);
			}
		}
	}
}