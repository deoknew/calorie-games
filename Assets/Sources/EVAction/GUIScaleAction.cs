using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GUIScaleAction : EVAction 
{
	public float from;
	public float to;

	private Hashtable _baseSizeTable;


	public override void run()
	{
		base.run();

		_baseSizeTable = new Hashtable();

		foreach (GameObject target in targetObjects) {
			GUIText text = target.guiText;
			GUITexture texture = target.guiTexture;

			if (text != null) {
				_baseSizeTable.Add(text.GetHashCode(), text.fontSize);
			} else if (texture != null) {
				_baseSizeTable.Add(texture.GetHashCode(), new Rect(texture.pixelInset));
			}
		}
	}

	public override void stop ()
	{
		base.stop ();

		if (_baseSizeTable == null)
			return;

		foreach (GameObject target in targetObjects) {
			GUITexture texture = target.guiTexture;
			GUIText text = target.guiText;

			if (texture != null) {
				Rect rect = (Rect)_baseSizeTable[texture.GetHashCode()];

				if (rect == null)
					return;

				texture.pixelInset = rect;
				
			} else if (text != null) {
				float size = float.Parse(_baseSizeTable[text.GetHashCode()].ToString());
				text.fontSize = (int)size;
			}
		}
	}


	public override void onAction(GameObject target, float progress)
	{
		GUITexture texture = target.guiTexture;
		GUIText text = target.guiText;

		float value = Mathf.Lerp (from, to, progress);

		if (texture != null) {
			Rect rect = (Rect)_baseSizeTable[texture.GetHashCode()];

			if (rect == null)
				return;

			rect.width *= value;
			rect.height *= value;
			texture.pixelInset = rect;

		} else if (text != null) {
			float size = float.Parse(_baseSizeTable[text.GetHashCode()].ToString());
			text.fontSize = (int)(size * value);
		}
	}
}