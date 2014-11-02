using UnityEngine;
using System.Collections;
using EVGame.Action;

public class GUIMovingAction : GameAction 
{
	public float fromX, fromY;
	public float toX, toY;


	public override void onAction(GameObject target, float progress)
	{
		GUITexture texture = target.guiTexture;

		float currentX = Mathf.Lerp (fromX, toX, progress);
		float currentY = Mathf.Lerp (fromX, toX, progress);

		if (texture) {
			Rect rect = texture.pixelInset;
			rect.x = currentX;
			rect.y = currentY;
			texture.pixelInset = rect;
		}
	}
}
