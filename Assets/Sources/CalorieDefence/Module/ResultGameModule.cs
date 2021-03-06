using UnityEngine;
using System.Collections;
using EVGame.Module;
using System.IO;

public class ResultGameModule : GameModule
{
	private const float MAX_BG_POSITION = 0.60f;

	private CalorieDefenceGameData _gameData;

	public GameObject guiLayer;

	public GUIText scoreText;
	public GUIText calorieText;
	public GUIText maxComboText;
	public GUIText gradeText;
	public Transform bestFoodPoint;
	public GameObject resultBackground;
	public GameObject bestFoodParticle;

	public GameObject effectParticle1;
	public GameObject effectParticle2;
	public GameObject effectParticle3;
	public GameObject effectParticle4;
	public GameObject effectParticle5;
	public GameObject effectParticle6;
	public ParticleRenderer effectParticle;

	public AudioClip finishAudio;

	public GameModule rankingModule;

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

	private bool _rankingModuleRunning;

	protected override void onStart()
	{
		base.onStart ();

		_gameData = (CalorieDefenceGameData) CurrentGameData;

		_calorieValue = _gameData.calorie;
		_scoreValue = _gameData.score;
		_maxComboValue = _gameData.maxCombo;
		_bestFoodIndex = _gameData.bestFoodIndex;
		_gradeValue = _gameData.rank;

		_rankingModuleRunning = false;

		_currentPosition = resultBackground.transform.position.y;

		if (finishAudio != null) {
			AudioSource.PlayClipAtPoint(finishAudio, transform.position, 1.0f);
		}

		if (guiLayer)
			guiLayer.SetActive (true);

		saveGameRecord ();
	}


	protected override void onFinish ()
	{
		base.onFinish ();

		if (guiLayer)
			guiLayer.SetActive (false);
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
			if (_currentScore > _scoreValue) {
				_currentScore = _scoreValue;

				// 스코어 표시가 끝나면 신기록 표시
				if (rankingModule != null) {
					if (!_rankingModuleRunning) {
						if (_currentScore > RankingManager.getRankScore()) {
							startRankingModule();
							_rankingModuleRunning = true;
						}
					}
				}
			}
			
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
				_currentPosition -= 0.06f;
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


	private void showResult()
	{
		gradeText.text = _gradeValue;
		_bestFoodObject = instantiateFoodObject(bestFoodPoint, _bestFoodIndex);

		if (bestFoodParticle != null) {
			GameObject obj = (GameObject)Instantiate (bestFoodParticle, _bestFoodObject.transform.position, Quaternion.identity);
			obj.layer = LayerMask.NameToLayer("GUI");
			Destroy(obj, 2);
		}

		effectParticle1.renderer.enabled = true;
		effectParticle1.particleSystem.Play ();
		effectParticle2.renderer.enabled = true;
		effectParticle2.particleSystem.Play ();
		effectParticle3.renderer.enabled = true;
		effectParticle3.particleSystem.Play ();
		effectParticle4.renderer.enabled = true;
		effectParticle4.particleSystem.Play ();
		effectParticle5.renderer.enabled = true;
		effectParticle5.particleSystem.Play ();
		effectParticle6.renderer.enabled = true;
		effectParticle6.particleSystem.Play ();

		effectParticle.enabled = true;
	}


	private GameObject instantiateFoodObject(Transform place, int foodIndex)
	{
		Vector3 position = Camera.allCameras[1].ViewportToWorldPoint(place.position);
		position.z = 0.0f;

		GameObject foodObject = RunningGameModule.Instance.projectiles[foodIndex].gameObject;

		// 모델 설정에 따라 달라지는 위치를 보정하기 위함
		position.x += foodObject.transform.position.x;
		position.y += foodObject.transform.position.y;

		GameObject createdObject = (GameObject)Instantiate(foodObject, position, Quaternion.identity);

		createdObject.layer = LayerMask.NameToLayer("GUI");
		createdObject.rigidbody.useGravity = false;
		createdObject.GetComponent<CalorieFoodObject>().enabled = false;
		createdObject.transform.localScale *= 1.5f;

		Shader shader = Shader.Find ("Unlit/Texture");
		if (shader != null)
			createdObject.renderer.material.shader = shader;

		TrailRenderer trail = createdObject.GetComponent<TrailRenderer>();
		if (trail != null)
			trail.time = 0;

		return createdObject;
	}


	private void startRankingModule()
	{
		if (guiLayer != null)
			guiLayer.SetActive (false);

		if (rankingModule != null)
			rankingModule.start ();
	}


	private void onRankingModuleFinished()
	{
		if (guiLayer != null)
			guiLayer.SetActive (true);
	}


	private void saveGameRecord()
	{
		System.DateTime origin = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
		System.TimeSpan diff = System.DateTime.Now - origin;
		int totalSeconds = (int)System.Math.Floor(diff.TotalSeconds);

		StreamWriter sw = new StreamWriter("./" + totalSeconds + ".log");
		sw.WriteLine (System.DateTime.Now);
		sw.WriteLine(_gameData.distance);
		sw.WriteLine(_gameData.calorie);
		sw.WriteLine(_gameData.score);
		sw.Flush();
		sw.Close();
	}
}