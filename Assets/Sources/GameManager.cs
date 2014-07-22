using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	enum GameState {
		IDLE, 
		OPENING, 
		PAUSE, 
		RUNNING, 
		ENDING, 
		RESULT,
	}
	
	public GameObject gameUILayer;
	public GameObject resultUILayer;
	public GameObject pauseUILayer;

	public GUITexture menuCursor;
	public GUIText calorieText;
	public GUIText MaterialCalorie;
	public GUIText textHit;  //물체 충돌시 물체 위치에 바로 표시되는 칼로리량 

	public GUIText timer;  //타이머 텍스트 

	public int[] foodIdArray;  //발사할 음식 인덱스 배열 

	public Transform[] projectiles;
	public GUITexture ImagePanel;
	public Transform text_Hit;


	float mseconds=0.0f;
	float time=0.0f;
	int minute=0,seconds=0;

	private GameObject _foodImageObject;

	private static GameManager instance;

	public static GameManager getInstance() {
		return instance;
	}


	private int currentCalorie;
	public int CurrentCalorie {
		get { return currentCalorie; }

	}
	
	private GameState currentState;


	public bool isGameRunning()
	{
		return (currentState == GameState.RUNNING);
	}


	public bool isGamePause()
	{
		return (currentState == GameState.PAUSE);
	}


	public bool isGameFinished()
	{
		return (currentState == GameState.ENDING) || (currentState == GameState.RESULT);
	}


	public bool isGameResult()
	{
		return (currentState == GameState.RESULT);
	}


	public void showFoodImage(int foodId)
	{
		if (_foodImageObject != null)
			Destroy (_foodImageObject);

		GameObject foodObject = (GameObject)findFoodObject(foodId);

		if (foodObject != null) {
			Transform imageLocation = GameObject.Find ("ImageLocation").transform;

			_foodImageObject = (GameObject)Instantiate (foodObject, imageLocation.position, Quaternion.identity);
			_foodImageObject.rigidbody.useGravity = false;

			_foodImageObject.transform.parent = gameUILayer.transform;
		}
	}

	
	public void showText(Transform transform,int calorie)
	{
		//text_Hit.transform.guiText.text=calorie.ToString ();

		//textHit.text = calorie.ToString ();
		//Transform text=(Transform) Instantiate(textHit.transform, transform.position, Quaternion.identity);
		Transform text=(Transform) Instantiate(text_Hit, transform.position, Quaternion.identity);
		Destroy (text.gameObject, 0.5f);

	}
	public void foodIdArrayCreate()
	{

		foodIdArray = new int[150];
		bool reverse = false;
		int j = (int)Random.Range(0,10);
		for (int i=0; i< 150; i++) {
			if(j>10)
			{
				reverse=true;
				j=j-2;
			}
			if(j<0)
			{
				reverse=false;
				j=j+2;
			}
			if(reverse==false)
			{
				foodIdArray[i]=j;
				j++;
			}
			else
			{
				foodIdArray[i]=j;
				j--;
			}

		}
	}

	public void addCalorie(int calorie)
	{
		currentCalorie += calorie;
	}


	public void showCalorie(int calorie)
	{
		if (MaterialCalorie != null)
			MaterialCalorie.text = calorie.ToString();
	}


	public void restart()
	{
		restartGame();
	}


	private GameObject findFoodObject(int foodId)
	{
		for (int i = 0; i < projectiles.Length; ++i) {
			CalorieFoodObject currentObject = projectiles[i].GetComponent<CalorieFoodObject>();

			if (currentObject.foodId == foodId) {
				return projectiles[i].gameObject;
			}
		}
		return null;
	}


	void init()
	{
		instance = this;
	}

	
	void initGameData()
	{
		currentCalorie = 0;
		
		time = 0;
		mseconds = 0;
		minute = 0;
		seconds = 0;
	}

	
	void Start () 
	{
		init();

		prepareGame();
		foodIdArrayCreate();
		startGame();

	    checkKinectCalibration();   


	}


	void Update ()
	{
		checkKinectCalibration();

		if (currentState == GameState.RUNNING) {
			timerUI ();
			updateCalorieText();
			if(time>=10)
			{	timer.color=Color.red;
				timer.fontSize=34;

			}
			if (time>=60) {
				finishGame ();
			}
		}
	}
	void Init()
	{
		ProjectileThrower.getInstance ().InitK();
		timer.color = Color.white;
		timer.fontSize = 32;
	}
	void timerUI()
	{
		time += Time.deltaTime;
		string timeStr;
		timeStr = "" + time.ToString ("00.00");
		//timeStr = timeStr.Replace (".", ":");

		int a = int.Parse(timeStr.Substring (0, 2));
		int b = int.Parse(timeStr.Substring(3,2));
		int time1 = 60 - a;
		int time2 = 99 - b;
		timeStr = time1.ToString("00") + ":" + time2.ToString("00");
		timer.text = timeStr;
	}

	void checkKinectCalibration()
	{
		if (currentState == GameState.RUNNING || currentState == GameState.PAUSE) {
			KinectManager kinectManager = KinectManager.Instance;
			if (kinectManager == null)
				return;
			
			bool isUserDetected = KinectManager.Instance.IsUserDetected();

			if (isUserDetected) {
				resumeGame();
			} else {
				pauseGame();
			}
		}
	}


	void prepareGame()
	{


		updateState(GameState.IDLE);
	}


	void startGame()
	{
		updateState(GameState.OPENING);

	}


	void pauseGame()
	{
		updateState(GameState.PAUSE);
	}


	void resumeGame()
	{
		updateState(GameState.RUNNING);

	}

	
	void finishGame()
	{
		updateState(GameState.ENDING);
	}


	void restartGame()
	{
		prepareGame();
		startGame();
	}


	IEnumerator runOpeningAction(int actionIndex)
	{
		float delay = 1.0f;
		bool finished = false;

		switch (actionIndex) {
		case 0:
			break;

		case 1:
			finished = true;
			break;
		}

		if (finished) {
			onOpeningFinished();
			yield break;
		}

		else {
			yield return new WaitForSeconds(delay);
			StartCoroutine("runOpeningAction", ++actionIndex);
		}
	}


	IEnumerator runEndingAction(int actionIndex)
	{
		float delay = 1.0f;
		bool finished = false;
		
		switch (actionIndex) {
		case 0:
			break;
			
		case 1:
			finished = true;
			break;
		}
		
		if (finished) {
			onEndingFinished();
			yield break;
		}
		
		else {
			yield return new WaitForSeconds(delay);
			StartCoroutine("runEndingAction", ++actionIndex);
		}
	}


	private void playOpening()
	{
		int actionIndex = 0;
		StartCoroutine("runOpeningAction", actionIndex);
	}


	private void playEnding()
	{
		int actionIndex = 0;
		StartCoroutine("runEndingAction", actionIndex);
	}
	

	private void onOpeningFinished()
	{
		updateState(GameState.RUNNING);
	}


	private void onEndingFinished()
	{
		updateState(GameState.RESULT);
	}
	

	void updateState(GameState nextState)
	{
		switch (nextState) {
		case GameState.IDLE:
			initGameData();
			break;

		case GameState.OPENING:
			if (currentState != GameState.IDLE)
				return;

			playOpening();
			break;

		case GameState.RUNNING:
			if (currentState == GameState.PAUSE) {
				Time.timeScale = 1;
			}

			RenderSettings.ambientLight = Color.white;
			break;

		case GameState.PAUSE:
			Time.timeScale = 0;
			RenderSettings.ambientLight = Color.black;
			break;

		case GameState.ENDING:
			playEnding();
			break;
			
		case GameState.RESULT:
			RenderSettings.ambientLight = Color.black;
			break;
		}
		updateUI(nextState);

		currentState = nextState;
	}


	void updateUI(GameState nextState)
	{
		gameUILayer.SetActive(false);
		resultUILayer.SetActive(false);
		pauseUILayer.SetActive(false);

		switch (nextState) {
		case GameState.OPENING:
		case GameState.ENDING:
			break;

		case GameState.IDLE:
			if (_foodImageObject != null) {
				DestroyObject(_foodImageObject);
			}
			break;

		case GameState.PAUSE:
			pauseUILayer.SetActive(true);
			menuCursor.enabled = false;
			break;
		
		case GameState.RUNNING:
			gameUILayer.SetActive(true);
			menuCursor.enabled = false;
			break;
		
		case GameState.RESULT:
			resultUILayer.SetActive(true);
			menuCursor.enabled = true;
			break;
		}
	}


	void updateCalorieText()
	{
		if (calorieText != null)
			calorieText.text = currentCalorie.ToString();	
	}


	void OnDisable()
	{
		instance = null;
	}
}
