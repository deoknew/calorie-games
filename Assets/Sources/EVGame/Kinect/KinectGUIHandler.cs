using UnityEngine;
using System.Collections;

public class KinectGUIHandler : MonoBehaviour 
{
	public float chargeTime = 2.0f;
	public Texture chargeTexture;
	public Color chargeTextureColor;
	public AudioClip clickAudio;
	public bool mouseInputEnabled = false;

	public GUITexture[] GUIList;


	void Update ()
	{
		if (false == mouseInputEnabled)
			return;

		float x = Input.mousePosition.x;
		float y = Input.mousePosition.y;
		
		if (Input.GetMouseButtonUp(0)) {
			receiveClickEvent(x, y, true);
		}
	}


	public void receiveMoveEvent(float x, float y, bool mouseInputMode = false)
	{
		for (int i = 0; i < GUIList.Length; ++i) {
			GUITexture gui = GUIList[i];

			if (gui.enabled == false || gui.gameObject.activeInHierarchy == false)
				continue;

			Vector3 screenPoint = new Vector3(x, y, gui.transform.position.z);

			if (false == mouseInputMode) {
				screenPoint = Camera.main.ViewportToScreenPoint(screenPoint);
			}

			GUIEvent guiEvent = gui.GetComponent<GUIEvent>();

			if (guiEvent == null)
				return;

			if (gui.HitTest(screenPoint)) {
				if (guiEvent.Charge >= 1.0f)
					return;

				guiEvent.Charge += (Time.deltaTime / chargeTime);

				guiEvent.ChargeTexture = chargeTexture;
				guiEvent.ChargeTextureColor = chargeTextureColor;

				if (guiEvent.Charge > 1.0f) {
					guiEvent.Charge = 1.0f;
					guiEvent.onClickEvent(x, y);
					if (clickAudio)
						AudioSource.PlayClipAtPoint(clickAudio, transform.position, 1.0f);
				}
				guiEvent.onMoveEvent(x, y);

			} else {
				guiEvent.Charge = 0.0f;
			}
		}
	}


	public void receiveClickEvent(float x, float y, bool mouseInputMode = false)
	{
		for (int i = 0; i < GUIList.Length; ++i) {
			GUITexture gui = GUIList[i];

			if (gui.enabled == false || gui.gameObject.activeInHierarchy == false)
				continue;

			Vector3 screenPoint = new Vector3(x, y, gui.transform.position.z);

			if (false == mouseInputMode) {
				screenPoint = Camera.main.ViewportToScreenPoint(screenPoint);
			}

			if (gui.HitTest(screenPoint)) {
				GUIEvent guiEvent = gui.GetComponent<GUIEvent>();
				
				if (guiEvent != null) {
					guiEvent.onClickEvent(x, y);

					if (clickAudio)
						AudioSource.PlayClipAtPoint(clickAudio, transform.position, 1.0f);
				}
			}
		}
	}
}
