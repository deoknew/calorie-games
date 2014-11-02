using UnityEngine;
using System.Collections;

namespace EVGame.Action
{
	public class GradientAction : GameAction
	{
		public Color from;
		public Color to;
		
		
		public override void onAction(GameObject target, float progress)
		{
			GUITexture texture = target.guiTexture;
			GUIText text = target.guiText;
			Renderer renderer = target.renderer;

			Color color = Color.Lerp(from, to, progress);
			
			if (texture != null)
				texture.color = color;
			
			if (text != null)
				text.color = color;
			
			if (renderer != null)
				renderer.material.color = color;
		}
	}
}