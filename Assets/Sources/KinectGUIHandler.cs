using UnityEngine;
using System.Collections;

public class KinectGUIHandler : MonoBehaviour 
{
	public GUITexture[] GUIList;
	private bool _mouseInputMode;


	void Update ()
	{
		if (KinectManager.Instance == null) {
			float x = Input.mousePosition.x;
			float y = Input.mousePosition.y;

			if (Input.GetMouseButton(0)) {
				receiveClickEvent(x, y, true);
			} else {
				receiveMoveEvent(x, y, true);
			}
		}
	}


	public void receiveMoveEvent(float x, float y, bool mouseInputMode = false)
	{
		for (int i = 0; i < GUIList.Length; ++i) {
			GUITexture gui = GUIList[i];

			Vector3 screenPoint = new Vector3(x, y, gui.transform.position.z);

			if (false == mouseInputMode) {
				screenPoint = Camera.main.ViewportToScreenPoint(screenPoint);
			}
			
			if (gui.HitTest(screenPoint)) {
				GUIEvent eventScript = gui.GetComponent<GUIEvent>();

				if (eventScript != null)
					eventScript.onMoveEvent(x, y);
			}
		}
	}


	public void receiveClickEvent(float x, float y, bool mouseInputMode = false)
	{
		for (int i = 0; i < GUIList.Length; ++i) {
			GUITexture gui = GUIList[i];

			Vector3 screenPoint = new Vector3(x, y, gui.transform.position.z);

			if (false == mouseInputMode) {
				screenPoint = Camera.main.ViewportToScreenPoint(screenPoint);
			}

			if (gui.HitTest(screenPoint)) {
				GUIEvent eventScript = gui.GetComponent<GUIEvent>();
				
				if (eventScript != null)
					eventScript.onClickEvent(x, y);
			}
		}
	}
}
