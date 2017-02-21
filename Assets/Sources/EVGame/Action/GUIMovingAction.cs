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
		GUIText text = target.guiText;

		float currentX = Mathf.Lerp (fromX, toX, progress);
		float currentY = Mathf.Lerp (fromY, toY, progress);

		if (texture) {
			Rect rect = texture.pixelInset;
			rect.x = currentX;
			rect.y = currentY;
			texture.pixelInset = rect;
		} 

		if (text) {
			text.pixelOffset = new Vector2(currentX, currentY);
		}
	}
}
