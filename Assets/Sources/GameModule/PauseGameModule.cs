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
	
	private float _currentTextAlpha = 0.0f;
	private float _currentCoverAlpha = 0.0f;


	protected override void onStart()
	{

	}
	
	
	protected override void onUpdate()
	{		
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
				finish ();
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
				finish ();
			}
		}
	}
	
	
	protected override void onReceiveParams(Hashtable paramTable)
	{
		Hiding = bool.Parse(paramTable["hiding"].ToString());
	}
}
