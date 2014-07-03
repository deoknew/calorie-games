using UnityEngine;
using System.Collections;

public abstract class GUIEvent : MonoBehaviour 
{
	public abstract void onMoveEvent(float x, float y);
	public abstract void onClickEvent(float x, float y);
}
