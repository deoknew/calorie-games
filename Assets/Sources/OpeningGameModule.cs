using UnityEngine;
using System.Collections;

public class OpeningGameModule : GameModule
{
	private const float COVER_ALPHA_SPEED = 0.01f;


	public Transform[] cameraPoints;
	public GUITexture screenCover;
	
	private OpeningFinishHandler _openingFinishHandler;
	private float _startTime;


	void Update () 
	{
		if (!Running)
			return;

		if (screenCover != null) {
			Color color = screenCover.color;
			color.a -= COVER_ALPHA_SPEED;

			if (color.a >= 1.0f)
				color.a = 1.0f;

			screenCover.color = color;
		}

		Camera mainCamera = Camera.main;

		float fracComplete = (Time.time - _startTime) / 8.0f;
		mainCamera.transform.position = Vector3.Slerp (cameraPoints[0].position, cameraPoints[1].position, fracComplete);

		Vector3 dist = (mainCamera.transform.position - cameraPoints[1].position);

		if (dist.sqrMagnitude <= 0.5f) {
			if (_openingFinishHandler != null)
				_openingFinishHandler();

			Running = false;
		}
	}


	public override void startModule(Hashtable paramTable)
	{
		if (paramTable == null)
			return;

		if (paramTable.Count == 0)
			return;

		_openingFinishHandler = (OpeningFinishHandler)paramTable["delegate"];

		Camera.main.transform.position = cameraPoints[0].position;
		_startTime = Time.time;

		Running = true;
	}


	public delegate void OpeningFinishHandler();
}
