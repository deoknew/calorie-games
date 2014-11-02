using UnityEngine;
using System.Collections;
using EVGame.Module;
using EVGame.Action;

public class RankingGameModule : GameModule 
{
	public float photoWidth = 480.0f;
	public float photoHeight = 360.0f;
	public GameObject guiLayer;
	public GameAction openAction; 
	public GameAction closeAction;

	public GUIText countdownGuiText;


	protected override void onStart ()
	{
		base.onStart ();

		if (guiLayer != null)
			guiLayer.SetActive (true);

		Camera.allCameras [1].enabled = false;
	}


	protected override void onFinish ()
	{
		base.onFinish ();

		Camera.allCameras [1].enabled = true;
	}


	protected override void onUpdate ()
	{
		base.onUpdate ();
	}

	
	void OnGUI()
	{
		if (GameManager.Instance.isGamePause ())
			return;

		float x = (Screen.width - photoWidth) / 2;
		float y = (Screen.height - photoHeight) / 2;
		GUI.DrawTexture (new Rect (x, y, photoWidth, photoHeight), KinectManager.Instance.GetUsersClrTex());
	}
}
