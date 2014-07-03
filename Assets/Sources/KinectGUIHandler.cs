using UnityEngine;
using System.Collections;

public class KinectGUIHandler : MonoBehaviour 
{
	public GUITexture[] GUIList;


	void Start ()
	{

	}


	void Update ()
	{

	}


	public void receiveMoveEvent(float x, float y)
	{
		for (int i = 0; i < GUIList.Length; ++i) {
			GUITexture gui = GUIList[i];

			Vector3 screenPoint = Camera.main.ViewportToScreenPoint(new Vector3(x, y, gui.transform.position.z));
			
			if (gui.HitTest(screenPoint)) {
				GUIEvent eventScript = gui.GetComponent<GUIEvent>();

				if (eventScript != null)
					eventScript.onMoveEvent(x, y);
			}
		}
	}


	public void receiveClickEvent(float x, float y)
	{
		for (int i = 0; i < GUIList.Length; ++i) {
			GUITexture gui = GUIList[i];

			Vector3 screenPoint = Camera.main.ViewportToScreenPoint(new Vector3(x, y, gui.transform.position.z));

			if (gui.HitTest(screenPoint)) {
				GUIEvent eventScript = gui.GetComponent<GUIEvent>();
				
				if (eventScript != null)
					eventScript.onClickEvent(x, y);
			}
		}
	}
}
