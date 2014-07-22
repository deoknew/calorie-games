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

	public GameModule runningModule;
	public GameModule resultModule;

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


	private int currentMaxCombo;
	public int CurrentMaxCombo {
		get { return currentMaxCombo; }
	}
	
	private int currentCombo;
	public int CurrentCombo {
		get { return currentCombo; }
	}
	
	private int currentScore;
	public int CurrentScore {
		get { return currentScore; }
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


	/// <summary>
	/// 게임 데이터를 초기화한다.
	/// </summary>
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


	void OnDisable()
	{
		instance = null;
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
		int time1 = 59 - a;
		int time2 = 99 - b;
		timeStr = time1.ToString("00") + ":" + time2.ToString("00");
		timer.text = timeStr;
	}


	/// <summary>
	/// 키넥트에서 사용자를 인식 중인지 확인하여, 아닐 경우 Pause 상태로 만든다.
	/// </summary>
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
			startResultModule();
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
		
		menuCursor.enabled = false;
		
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
			break;
			
		case GameState.RUNNING:
			gameUILayer.SetActive(true);
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


	private void startResultModule()
	{
		if (resultModule == null)
			return;
		
		Hashtable paramTable = new Hashtable();
		
		int bestFoodIndex = 3;	//TODO: 테스트용으로 0 할당
		int worstFoodIndex = 2; //TODO: 테스트용으로 1 할당
		
		int grade = calculateGrade(currentScore, currentCalorie);
		string gradeText = getGradeText(grade);
		
		paramTable.Add("grade", gradeText);
		paramTable.Add("score", currentScore);
		paramTable.Add("calorie", currentCalorie);
		paramTable.Add("max_combo", currentMaxCombo);
		paramTable.Add("best_food", bestFoodIndex);
		paramTable.Add("worst_food", worstFoodIndex);
		
		resultModule.startModule(paramTable);
	}


	private int calculateGrade(int score, int calorie)
	{
		//TODO: 사용자의 점수와 칼로리 소비량에 따라 등급을 매기고 이를 반환한다.
		return 0;
	}
	
	
	private string getGradeText(int grade)
	{
		//TODO: 등급에 따른 텍스트를 반환한다.
		return "Grade Text";
	}
}
