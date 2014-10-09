using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GUIScaleAction : EVAction 
{
	public float from;
	public float to;

	private Hashtable _baseSizeTable;


	public override void onStart()
	{
		base.onStart();

		_baseSizeTable = new Hashtable();

		foreach (GameObject target in targetObjects) {
			GUIText text = target.guiText;
			GUITexture texture = target.guiTexture;

			int hashCode = target.GetHashCode();

			if (text != null) {
				_baseSizeTable.Add(hashCode, text.fontSize);
			} else if (texture != null) {
				if (texture.pixelInset.x <= 0.0f && texture.pixelInset.y <= 0.0f)
					_baseSizeTable.Add(hashCode, texture.transform.localScale);
				else
					_baseSizeTable.Add(hashCode, new Rect(texture.pixelInset));
			}
		}
	}

	public override void onStop ()
	{
		base.onStop ();

		if (_baseSizeTable == null)
			return;

		foreach (GameObject target in targetObjects) {
			GUITexture texture = target.guiTexture;
			GUIText text = target.guiText;

			int hashCode = target.GetHashCode();

			if (texture != null) {
				object obj = _baseSizeTable[hashCode];

				if (obj != null) {
					if (obj.GetType() == typeof(Rect)) {
						texture.pixelInset = (Rect)obj;
					} else if (obj.GetType() == typeof(Vector3)) {
						texture.transform.localScale = (Vector3)obj;
					}
				}
				
			} else if (text != null) {
				float size = float.Parse(_baseSizeTable[hashCode].ToString());
				text.fontSize = (int)size;
			}
		}
	}


	public override void onAction(GameObject target, float progress)
	{
		GUITexture texture = target.guiTexture;
		GUIText text = target.guiText;

		int hashCode = target.GetHashCode ();

		float value = Mathf.Lerp (from, to, progress);

		if (texture != null) {
			object obj = _baseSizeTable[hashCode];

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
			float size = float.Parse(_baseSizeTable[hashCode].ToString());
			text.fontSize = (int)(size * value);
		}
	}
}