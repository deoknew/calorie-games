using UnityEngine;
using System.Collections;

public class PauseGameModule : GameModule 
{
	private const float TEXT_ALPHA_SPEED = 0.03f;
	private const float COVER_ALPHA_SPEED = 0.03f;


	public GUITexture pauseScreenCover;
	public GUITexture pauseStandImage;
	public GUIText pauseText;

	private bool _hiding;
	public bool Hiding {
		set { _hiding = value; }
		get { return _hiding; }
	}

	private HidingFinishHandler _hidingFinishHandler;

	private float _currentTextAlpha = 0.0f;
	private float _currentCoverAlpha = 0.0f;


	void Update () 
	{
		if (!Running)
			return;

		if (Hiding) {
			if (pauseScreenCover) {
				_currentCoverAlpha -= COVER_ALPHA_SPEED;

				if (_currentCoverAlpha <= 0.0f)
					_currentCoverAlpha = 0.0f;

				Color color = pauseScreenCover.color;
				color.a = _currentCoverAlpha;
				pauseScreenCover.color = color;
			}

			if (pauseText) {
				_currentTextAlpha -= TEXT_ALPHA_SPEED;
				
				if (_currentTextAlpha <= 0.0f)
					_currentTextAlpha = 0.0f;


				Color color = pauseText.color;
				color.a = _currentTextAlpha;
				pauseText.color = color;
				pauseStandImage.color = color;
			}

			if (_currentTextAlpha == 0.0f && _currentCoverAlpha == 0.0f) {
				if (_hidingFinishHandler != null)
					_hidingFinishHandler();
			    
			    Running = false;
			}
		} else {
			if (pauseScreenCover) {
				_currentCoverAlpha += COVER_ALPHA_SPEED;
				
				if (_currentCoverAlpha >= 1.0f)
					_currentCoverAlpha = 1.0f;
				
				Color color = pauseScreenCover.color;
				color.a = _currentCoverAlpha;
				pauseScreenCover.color = color;
			}
			
			if (pauseText) {
				_currentTextAlpha += TEXT_ALPHA_SPEED;
				
				if (_currentTextAlpha >= 1.0f)
					_currentTextAlpha = 1.0f;
				
				Color color = pauseText.color;
				color.a = _currentTextAlpha;
				pauseText.color = color;
				pauseStandImage.color = color;
			}
			
			if (_currentTextAlpha == 1.0f && _currentCoverAlpha == 1.0f) {
				Running = false;
			}
		}
	}


	public override void startModule(Hashtable paramTable)
	{
		if (paramTable == null)
			return;
		
		if (paramTable.Count == 0)
			return;
		
		Hiding = bool.Parse(paramTable["hiding"].ToString());
		_hidingFinishHandler = (HidingFinishHandler)paramTable["delegate"];

		Running = true;
	}


	public delegate void HidingFinishHandler();
}
