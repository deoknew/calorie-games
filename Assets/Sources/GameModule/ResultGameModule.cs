using UnityEngine;
using System.Collections;

public class ResultGameModule : GameModule 
{
	private const float MAX_BG_POSITION = 0.60f;

	public GUIText scoreText;
	public GUIText calorieText;
	public GUIText maxComboText;
	public GUIText gradeText;
	public Transform bestFoodPoint;
	public GameObject resultBackground;
	public GameObject bestFoodParticle;

	private GameObject _bestFoodObject;

	private float _calorieValue;
	private int _scoreValue;
	private int _maxComboValue;
	private int _bestFoodIndex;
	private string _gradeValue;

	private float _currentCalorie;
	private int _currentCombo;
	private int _currentScore;
	private float _currentPosition;


	protected override void onStart()
	{
		_currentPosition = resultBackground.transform.position.y;
	}
	
	
	protected override void onUpdate()
	{
		if (_bestFoodObject != null) {
			Vector3 point = _bestFoodObject.collider.bounds.center;
			_bestFoodObject.transform.RotateAround(point, new Vector3(0.0f, 1.0f, 0.0f), 1.0f);
		}
		
		if (maxComboText != null && _currentCombo < _maxComboValue) {
			_currentCombo += 1;
			if (_currentCombo > _maxComboValue)
				_currentCombo = _maxComboValue;
			
			maxComboText.text = string.Format("{0}", _currentCombo);
		}
		
		if (scoreText != null && _currentScore < _scoreValue) {
			_currentScore += (_scoreValue / 40);
			if (_currentScore > _scoreValue)
				_currentScore = _scoreValue;
			
			scoreText.text = string.Format("{0}", _currentScore);
		}
		
		if (calorieText != null && _currentCalorie < _calorieValue) {
			_currentCalorie += (_calorieValue / 40.0f);
			if (_currentCalorie > _calorieValue)
				_currentCalorie = _calorieValue;
			
			calorieText.text = string.Format("{0:F2}", _currentCalorie);
		}
		
		if (resultBackground != null) {
			if (_currentPosition > MAX_BG_POSITION) {
				_currentPosition -= 0.05f;
				if (_currentPosition < MAX_BG_POSITION) {
					_currentPosition = MAX_BG_POSITION;
					showResult();
				}
				
				Vector3 position = resultBackground.transform.position;
				position.y = _currentPosition;
				resultBackground.transform.position = position;
			}
		}
	}
	
	
	protected override void onReceiveParams(Hashtable paramTable)
	{
		_calorieValue = float.Parse(paramTable["calorie"].ToString());
		_scoreValue = int.Parse(paramTable["score"].ToString());
		_maxComboValue = int.Parse(paramTable["max_combo"].ToString());
		_bestFoodIndex = int.Parse(paramTable["best_food"].ToString());
		_gradeValue = paramTable["grade"].ToString();
	}


	private void showResult()
	{
		gradeText.text = _gradeValue;
		_bestFoodObject = instantiateFoodObject(bestFoodPoint, _bestFoodIndex);

		if (bestFoodParticle != null) {
			GameObject obj = (GameObject)Instantiate (bestFoodParticle, _bestFoodObject.transform.position, Quaternion.identity);
			obj.layer = LayerMask.NameToLayer("GUI");
			Destroy(obj, 2);
		}
	}


	private GameObject instantiateFoodObject(Transform place, int foodIndex)
	{
		Vector3 position = Camera.allCameras[1].ViewportToWorldPoint(place.position);
		position.z = 0.0f;

		GameObject foodObject = GameManager.getInstance().projectiles[foodIndex].gameObject;

		// 모델 설정에 따라 달라지는 위치를 보정하기 위함
		position.x += foodObject.transform.position.x;
		position.y += foodObject.transform.position.y;

		GameObject createdObject = (GameObject)Instantiate(foodObject, position, Quaternion.identity);

		createdObject.layer = LayerMask.NameToLayer("GUI");
		createdObject.rigidbody.useGravity = false;
		createdObject.GetComponent<CalorieFoodObject>().enabled = false;
		createdObject.transform.localScale *= 1.5f;

		TrailRenderer trail = createdObject.GetComponent<TrailRenderer>();
		if (trail != null)
			trail.time = 0;

		return createdObject;
	}
}