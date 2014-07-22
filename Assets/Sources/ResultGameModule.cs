using UnityEngine;
using System.Collections;

public class ResultGameModule : GameModule 
{
	public GUIText scoreText;
	public GUIText calorieText;
	public GUIText maxComboText;
	public GUIText gradeText;
	public Transform bestFoodPoint;
	public Transform worstFoodPoint;

	private GameObject _bestFoodObject;
	private GameObject _worstFoodObject;

	private string _calorieValue;
	private string _scoreValue;
	private string _maxComboValue;
	private string _bestFoodIndex;
	private string _worstFoodIndex;
	private string _gradeValue;


	void Update()
	{
		if (_bestFoodObject != null) {
			_bestFoodObject.transform.Rotate(0, 1, 0);
		}

		if (_worstFoodObject != null) {
			_worstFoodObject.transform.Rotate(0, 1, 0);
		}
	}


	public override void startModule(Hashtable paramTable)
	{
		if (paramTable == null)
			return;

		if (paramTable.Count == 0)
			return;

		_calorieValue = paramTable["calorie"].ToString();
		_scoreValue = paramTable["score"].ToString();
		_maxComboValue = paramTable["max_combo"].ToString();
		_bestFoodIndex = paramTable["best_food"].ToString();
		_worstFoodIndex = paramTable["worst_food"].ToString();
		_gradeValue = paramTable["grade"].ToString();

		showResult();
	}


	private void showResult()
	{
		scoreText.text = _scoreValue;
		calorieText.text = _calorieValue;
		maxComboText.text = _maxComboValue;
		gradeText.text = _gradeValue;

		_bestFoodObject = instantiateFoodObject(bestFoodPoint, int.Parse(_bestFoodIndex));
		_worstFoodObject = instantiateFoodObject(worstFoodPoint, int.Parse(_worstFoodIndex));
	}


	private GameObject instantiateFoodObject(Transform place, int foodIndex)
	{
		Vector3 position = Camera.allCameras[1].ViewportToWorldPoint(place.position);
		position.z = 0.0f;

		GameObject foodObject = GameManager.getInstance().projectiles[foodIndex].gameObject;
		GameObject createdObject = (GameObject)Instantiate(foodObject, position, Quaternion.identity);

		createdObject.layer = LayerMask.NameToLayer("GUI");
		createdObject.rigidbody.useGravity = false;
		createdObject.GetComponent<CalorieFoodObject>().enabled = false;
		createdObject.transform.localScale *= 1.5f;

		return createdObject;
	}
}