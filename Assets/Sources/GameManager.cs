using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	private const float PLAY_TIME = 30.0f;

	enum GameState {
		IDLE, 
		OPENING, 
		PAUSE,
		RESUME,
		RUNNING, 
		ENDING, 
		RESULT,
	}
	
	public GameObject gameUILayer;
	public GameObject resultUILayer;
	public GameObject pauseUILayer;

	public GUITexture menuCursor;
	public GUIText scoreText;
	//
	public GUIText [] GUI_Calorie;
	//
	public GUIText textCombo;
	public GUITexture textureCombo;

	public GameModule runningModule;
	public GameModule resultModule;
	public GameModule pauseModule;
	public GameModule openingModule;

	public GUIText timer;  //타이머 텍스트 

	/// <summary>
	public int[] foodIdArray;  //발사할 음식 인덱스 배열 
	public int [] NumberOfCollision;

	public GameObject [] foodBackGround;
	public GameObject[] foodImage;

	private int firstCalorie;
	private int secondCalorie;
	private int thirdCalorie;


	private int foodNum1=0;
	private int foodNum2=1;
	private int foodNum3=2;

	private int sequence=0; //첫번째 물체를 맞추었나? 두번째 물체를 맞추었나? 세번째 물체를 맞추었나?
	public Material feverSkyBox;
	public Material basicSkyBox;

	public Light spotLight1;
	public Light spotLight2;
	public Light spotLight3;
	public Light spotLight4;
	public Light spotLight5;
	public Light spotLight6;

	public GameObject[] feverParticle;
	public GUITexture feverTimeText;

	float time;
	float feverTime;
	float startTime; 
	private float sLightTime;


	private bool isFeverTime;
	public void setFeverTime(bool isfevertime){
		isFeverTime = isfevertime;
	}
	public bool IsFeverTime {
		get { return isFeverTime; }
	}
	private bool isFirst=true;
	/// <summary>

	public Transform[] projectiles;

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
	
	private float currentCalorie;
	public float CurrentCalorie {
		get { return currentCalorie; }
	}

	
	private GameState currentState;


	public bool isGameRunning()
	{
		return (currentState == GameState.RUNNING);
	}


	public bool isGamePause()
	{
		return (currentState == GameState.PAUSE) || (currentState == GameState.RESUME);
	}


	public bool isGameFinished()
	{
		return (currentState == GameState.ENDING) || (currentState == GameState.RESULT);
	}


	public bool isGameResult()
	{
		return (currentState == GameState.RESULT);
	}


	public void showFoodImage(string imageName)
	{
		string texturePath = "file://" + Application.dataPath + "\\Images\\" + imageName;
		WWW textureLoad = new WWW (texturePath);

		startTime = Time.time;

		if (sequence == 1) 
		{
			foodImage[0].GetComponent<GUITexture> ().texture = textureLoad.texture;
		}
		if(sequence == 2)
		{
			foodImage[1].GetComponent<GUITexture>().texture = textureLoad.texture;
		}
		if(sequence == 3)
		{
			foodImage[2].GetComponent<GUITexture>().texture = textureLoad.texture;
		}
		if(sequence >= 4)
		{
			foodImage[foodNum3].GetComponent<GUITexture> ().texture = textureLoad.texture;
		}
	}
	public void showCalorie(int score)
	{
		sequence++;
		if (sequence == 1)
			GUI_Calorie[0].text = score.ToString();
		
		if(sequence == 2)
			GUI_Calorie[1].text = score.ToString();
		
		if(sequence ==3)
			GUI_Calorie[2].text = score.ToString();
		
		if(sequence >=4)
		{
			foodNum1++;
			foodNum2++;
			foodNum3++;
			if(foodNum1>2)
				foodNum1=0;
			if(foodNum2>2)
				foodNum2=0;
			if(foodNum3>2)
				foodNum3=0;

			GUI_Calorie[foodNum3].text = score.ToString();

		}
	}


	public void resetCombo()
	{
		textCombo.enabled = false;
		textureCombo.enabled = false;
		currentCombo = 0;
	}


	public void addCombo()
	{
		currentCombo++;
		textCombo.enabled = true;
		textureCombo.enabled = true;

		EVAction.run (textCombo.gameObject);
		EVAction.run (textureCombo.gameObject);

		textCombo.text = currentCombo.ToString();

		if(currentCombo > currentMaxCombo)
			currentMaxCombo = currentCombo;
	}


	public void foodIdArrayCreate()  //음식인덱스 배열 생성 함수 /// 게임 시작시 음식인덱스를 저장한 배열 생성 
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
		int a = (int)Random.Range (0, 20);
		int b = (int)Random.Range (21, 40);
		int c = (int)Random.Range (41, 60);
		int d = (int)Random.Range (61, 79);
		foodIdArray [a] = 11;
		foodIdArray [b] = 11;
		foodIdArray [c] = 11;
		foodIdArray [d] = 11;
		// 폭탄 생성 

		int e = (int)Random.Range (10, 20);
		int f = (int)Random.Range (80, 100);
		foodIdArray [e] = 12;
		foodIdArray [f] = 12;


	}

	public void addScore(int score)
	{
		currentScore += score;
	}


	public void addMovingDistance(float distance)
	{
		currentCalorie += getCalorieFromDistance(distance);
	}


	public float getCalorieFromDistance(float distance)
	{
		// 임시로 가정된 수치로 계산함

		const float K = 0.000325f;
		const float U = 0.1615f;

		float calorie = 0.0f;
		calorie = ((distance / U) * K);

		return calorie;
	}
	
	public void CheckFoodCrash(int FoodID)
	{
		NumberOfCollision [FoodID] += 1;
	}


	int getMostCollisionFood()
	{
		int max;
		int maxNum=0;
		max=NumberOfCollision[0];
		for(int i=0; i<10; ++i)
		{
			if(max < NumberOfCollision[i])
			{
				max=NumberOfCollision[i];
				maxNum=i;
			}
		}
		return maxNum;
	}


	int getLeastCollisionFood()
	{
		int min;
		int minNum=0;
		min=NumberOfCollision[0];
		for(int i=0; i<10; ++i)
		{
			if(min > NumberOfCollision[i])
			{
				min=NumberOfCollision[i];
				minNum=i;
			}
		}
		return minNum;
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
		//foodImage.enabled = false;
		resetCombo ();
		instance = this;
	}


	/// <summary>
	/// 게임 데이터를 초기화한다.
	/// </summary>
	void initGameData()
	{
		NumberOfCollision = new int[projectiles.Length];
		for (int i=0; i<projectiles.Length; i++) 
			NumberOfCollision[i]=0;

		currentCalorie = 0.0f;
		currentCombo = 0;
		currentMaxCombo = 0;
		time = 0.0f;
		feverTime = 0.0f;

		if(ProjectileThrower.getInstance() != null)
			ProjectileThrower.getInstance ().reset();

		timer.color = Color.white;
		isFeverTime = false;
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
			updateScoreText();
			if(time >= 50.0f && time <= 50.3f)
			{	
				timer.color=Color.red;
				//RenderSettings.skybox=skyBox;
				//feverParticle[0].renderer.enabled = true;
			}
			if (time >= PLAY_TIME) {
				finishGame ();
			}

		}

		if(isFeverTime==true && isFirst == true)
		{
			//backGround.renderer.enabled=false;
			RenderSettings.skybox=feverSkyBox;

			spotLight1.intensity=5;
			spotLight3.intensity=5;
			spotLight5.intensity=5;
			spotLight2.intensity=5;
			spotLight4.intensity=5;
			spotLight6.intensity=5;
			spotLight1.color=Color.magenta;
			spotLight4.color=Color.green;
			spotLight5.color=Color.magenta;
			spotLight2.color=Color.yellow;
			spotLight3.color=Color.red;
			spotLight6.color=Color.cyan;

			ProjectileThrower.getInstance().setWaitTime(0.2f);


			for(int i=0; i<13; i++)
			{
				feverParticle[i].renderer.enabled = true;
				feverParticle[i].particleSystem.Play();
			}

			isFirst=false;
		}

		if (isFeverTime == true) 
		{
			feverTime += Time.deltaTime;

			if(feverTime<=0.5f)
			{
				feverTimeText.enabled=true;
				Vector3 startPoint = new Vector3 (transform.lossyScale.x*0.2f,transform.lossyScale.y*0.2f);
				Vector3 firstPoint = new Vector3 (transform.lossyScale.x*0.4f, transform.lossyScale.y*0.4f);
				float fracComplete = (Time.time - startTime) / 0.4f;

				feverTimeText.transform.localScale=Vector3.Slerp (startPoint,firstPoint,fracComplete);
			}
			else
				feverTimeText.enabled=false;

			/////
			sLightTime +=Time.deltaTime;
			if(sLightTime<=0.05f)
			{
				Debug.Log ("ya");
				/*if(spotLight1.intensity==0)
				{
					spotLight1.intensity=8;
					spotLight3.intensity=8;
					spotLight5.intensity=8;
					spotLight2.intensity=0;
					spotLight4.intensity=0;
					spotLight6.intensity=0;
				}
				else if(spotLight2.intensity==0)
				{
					spotLight1.intensity=0;
					spotLight3.intensity=0;
					spotLight5.intensity=0;
					spotLight2.intensity=8;
					spotLight4.intensity=8;
					spotLight6.intensity=8;
				}*/
				if(spotLight1.color==Color.magenta)
				{
					spotLight1.color=Color.yellow;
					spotLight4.color=Color.red;
					spotLight5.color=Color.cyan;
					spotLight2.color=Color.magenta;
					spotLight3.color=Color.green;
					spotLight6.color=Color.magenta;
				}
				else if(spotLight1.color==Color.yellow)
				{
					spotLight1.color=Color.magenta;
					spotLight4.color=Color.green;
					spotLight5.color=Color.magenta;
					spotLight2.color=Color.yellow;
					spotLight3.color=Color.red;
					spotLight6.color=Color.cyan;
				}
			}
			else if(sLightTime>=0.1f)
				sLightTime=0;
			/////

		}

		if (feverTime >= 5.0f && feverTime <=5.3f) 
		{
			isFeverTime = false;
			//backGround.renderer.enabled=true;
			RenderSettings.skybox=basicSkyBox;

			feverTime=0.0f;
			isFirst=true;
			ProjectileThrower.getInstance().setWaitTime(0.4f);


			for(int i=0; i<13; i++)
				feverParticle[i].renderer.enabled=false;

			spotLight1.intensity=0;
			spotLight3.intensity=0;
			spotLight5.intensity=0;
			spotLight2.intensity=0;
			spotLight4.intensity=0;
			spotLight6.intensity=0;
		}

		////////////
	 	
		backGroundTranslate ();

		//////////////
	}
	void backGroundTranslate()  // 우측 상단 네모칸 이동부분 
	{
		Vector3 startPoint = new Vector3 (1.5f, 0.634f, -1.0f);
		Vector3 firstPoint = new Vector3 (0.87f, 0.634f, -1.0f);
		Vector3 secondPoint = new Vector3 (0.87f, 0.717f, -1.0f);
		Vector3 thirdPoint = new Vector3 (0.87f, 0.8f, -1.0f);
		float fracComplete = (Time.time - startTime) / 0.4f;
		if (sequence == 1) 
		{				
			foodBackGround[0].transform.position = Vector3.Slerp (startPoint, firstPoint, fracComplete);
		}
		if(sequence == 2)
		{
			foodBackGround[0].transform.position = Vector3.Slerp (firstPoint, secondPoint, fracComplete);
			foodBackGround[1].transform.position = Vector3.Slerp (startPoint, firstPoint, fracComplete);
		}
		if(sequence == 3)
		{
			foodBackGround[0].transform.position = Vector3.Slerp (secondPoint, thirdPoint, fracComplete);
			foodBackGround[1].transform.position = Vector3.Slerp (firstPoint, secondPoint, fracComplete);
			foodBackGround[2].transform.position = Vector3.Slerp (startPoint, firstPoint, fracComplete);
		}
		if(sequence >= 4)
		{
			if(foodNum1>2)
				foodNum1=0;
			if(foodNum2>2)
				foodNum2=0;
			if(foodNum3>2)
				foodNum3=0;
			
			foodBackGround[foodNum3].transform.position = Vector3.Slerp (startPoint, firstPoint, fracComplete);
			foodBackGround[foodNum1].transform.position = Vector3.Slerp (secondPoint, thirdPoint, fracComplete);
			foodBackGround[foodNum2].transform.position = Vector3.Slerp (firstPoint, secondPoint, fracComplete);
			
		}
	}


	void OnDisable()
	{
		instance = null;
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
				if (isGamePause())
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
		updateState(GameState.RESUME);
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
		if (openingModule != null) {
			openingModule.OnFinished = new GameModule.OnFinishedDelegate(onOpeningFinished);
			openingModule.start();

		} else {
			int actionIndex = 0;
			StartCoroutine("runOpeningAction", actionIndex);
		}
	}


	private void playEnding()
	{
		int actionIndex = 0;
		StartCoroutine("runEndingAction", actionIndex);
	}


	private void showPauseScreen()
	{
		if (pauseModule == null)
			return;

		Hashtable paramTable = new Hashtable();
		paramTable.Add("hiding", false);

		pauseModule.OnFinished = null;
		pauseModule.start(paramTable);
	}


	private void hidePauseScreen()
	{
		if (pauseModule != null) {
			Hashtable paramTable = new Hashtable();
			paramTable.Add("hiding", true);

			pauseModule.OnFinished = new GameModule.OnFinishedDelegate(onResumeFinished);
			pauseModule.start(paramTable);

		} else {
			onResumeFinished();
		}
	}


	private void onResumeFinished()
	{
		updateState(GameState.RUNNING);
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
			if (currentState == GameState.RESUME) {
				Time.timeScale = 1;
			}
			
			RenderSettings.ambientLight = Color.white;
			break;
			
		case GameState.PAUSE:
			Time.timeScale = 0;
			showPauseScreen();
			break;

		case GameState.RESUME:
			hidePauseScreen();
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
			break;
		
		case GameState.RESUME:
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


	void updateScoreText()
	{
		if (scoreText != null)
			scoreText.text = currentScore.ToString();
	}


	private void startResultModule()
	{
		if (resultModule == null)
			return;
		
		Hashtable paramTable = new Hashtable();
		
		int bestFoodIndex = getMostCollisionFood();

		int grade = calculateGrade(currentScore, currentCalorie);
		string gradeText = getGradeText(grade);
	
		// 소수점 두 번째 자리까지 표시
		float roundedCalorie = Mathf.Round(currentCalorie * 100) / 100;

		paramTable.Add("grade", gradeText);
		paramTable.Add("score", currentScore);
		paramTable.Add("calorie", roundedCalorie);
		paramTable.Add("max_combo", currentMaxCombo);
		paramTable.Add("best_food", bestFoodIndex);
		
		resultModule.start(paramTable);

		if (KinectManager.Instance != null)
			KinectManager.Instance.DisplayColorMap = false;
	}


	private int calculateGrade(int score, float calorie)
	{
		// 임시로 등급을 SSS, SS, S, A, B, C, D, F 8단계로 구분
		return (score / 3000);
	}
	
	
	private string getGradeText(int grade)
	{
		string[] GRADE_TEXT = {
			"F", "D", "C", "B", "A", "S", "SS",
		};

		if (grade < 0)
			grade = 0;

		if (grade >= GRADE_TEXT.Length)
			grade = GRADE_TEXT.Length - 1;

		return GRADE_TEXT[grade];
	}
}
