using UnityEngine;
using System.Collections;

public abstract class GUIEvent : MonoBehaviour 
{
	private float _charge;
	public float Charge {
		get { return _charge; }
		set { _charge = value; }
	}
	
	private Texture _chargeTexture;
	public Texture ChargeTexture {
		get { return _chargeTexture; }
		set { _chargeTexture = value; }
	}


	private Color _chargeTextureColor;
	public Color ChargeTextureColor {
		get { return _chargeTextureColor; }
		set { _chargeTextureColor = value; }
	}


	private GUITexture _chargeGuiTexture;


	public GUIEvent()
	{
		_charge = 0.0f;
		_chargeGuiTexture = null;
	}


	void Update()
	{
		if (_charge > 0.0f) {
			const float MARGIN = 20;

			GUITexture guiTexture = gameObject.guiTexture;
			if (guiTexture != null) {
				if (_chargeGuiTexture == null) {
					_chargeGuiTexture = (GUITexture)GUITexture.Instantiate(guiTexture);

					GUIText[] childArray = _chargeGuiTexture.GetComponentsInChildren<GUIText>();
					foreach (GUIText child in childArray)
						GameObject.Destroy(child);

					Vector3 pos = _chargeGuiTexture.transform.position;
					pos.z = 2.0f;
					_chargeGuiTexture.transform.position = pos;

					Color color = _chargeGuiTexture.color;
					_chargeGuiTexture.color = _chargeTextureColor;

					Rect pixelInset = _chargeGuiTexture.pixelInset;
					pixelInset.x += MARGIN;
					pixelInset.y += MARGIN;
					pixelInset.width -= (MARGIN * 2);
					pixelInset.height -= (MARGIN * 2);
					_chargeGuiTexture.pixelInset = pixelInset;

					if (_chargeTexture != null)
						_chargeGuiTexture.texture = _chargeTexture;
				}
				_chargeGuiTexture.enabled = true;

				Rect currentInset = _chargeGuiTexture.pixelInset;
				currentInset.width = Mathf.Lerp(0.0f, guiTexture.pixelInset.width - (MARGIN * 2), _charge);
				_chargeGuiTexture.pixelInset = currentInset;
			}
		} else {
			if (_chargeGuiTexture != null)
				_chargeGuiTexture.enabled = false;
 		}
	}


	void OnDisable()
	{
		if (_chargeGuiTexture != null)
			_chargeGuiTexture.enabled = false;
	}


	public virtual void onMoveEvent(float x, float y) {}
	public virtual void onClickEvent(float x, float y) {}
}
