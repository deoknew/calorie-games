using UnityEngine;
using System.Collections;
using EVGame.Module;
using EVGame.Action;

public class RunningGameModule : GameModule
{
	private CalorieDefenceGameData _gameData;

	public GameObject guiLayer;

	public int playTime = 60;

	public bool tutorialEnabled;

	public AudioClip bgAudio;

	public GUIText scoreText;
	public GUIText timer;  //타이머 텍스트 
	public GUITexture feverTimeText;

	public Material feverSkyBox;
	public Material basicSkyBox;
	
	public Light spotLight1;
	public Light spotLight2;
	public Light spotLight3;
	public Light spotLight4;
	public Light spotLight5;
	public Light spotLight6;

	public Transform[] projectiles;
	public GameObject[] feverParticle;
	
	public GameObject [] foodBackGround;
	public GameObject[] foodImage;

	public GUITexture menuCursor;
	//
	public GUIText [] GUI_Calorie;
	//
	public GUIText textCombo;
	public GUITexture textureCombo;
	
	public int[] foodIdArray;  //발사할 음식 인덱스 배열 
	public int [] NumberOfCollision;	

	public GameModule tutorialModule;
	public GameObject tutorialMenuGuiLayer;
	public GameAction tutorialMenuAction;

	public GameAction readyAction;

	private int firstCalorie;
	private int secondCalorie;
	private int thirdCalorie;

	private int foodNum1=0;
	private int foodNum2=1;
	private int foodNum3=2;
	
	private int sequence=0; //첫번째 물체를 맞추었나? 두번째 물체를 맞추었나? 세번째 물체를 맞추었나?

	private float time;
	private float feverTime;
	private float startTime;
	
	private Hashtable _foodImageCache;
	
	private float sLightTime;
	private Vector3 startFeverTextScale;

	private bool isFirst=true;
	
	private bool _isFeverTime;
	public bool IsFeverTime {
		get { return _isFeverTime; }
	}
	
	private bool _tutorialRunning;
	public bool isTutorialRunning()
	{
		return _tutorialRunning;
	}
	
	private static RunningGameModule _instance;
	public static RunningGameModule Instance {
		get { return _instance;}
	}


	protected override void onStart ()
	{
		_instance = this;
		_gameData = (CalorieDefenceGameData) CurrentGameData;

		time = 0.0f;
		feverTime = 0.0f;
		_isFeverTime = false;

		NumberOfCollision = new int[projectiles.Length];
		for (int i=0; i<projectiles.Length; i++) 
			NumberOfCollision[i]=0;
		
		if(ProjectileThrower.getInstance() != null)
			ProjectileThrower.getInstance ().reset();
		
		timer.color = Color.white;
		resetCombo ();

		foodIdArrayCreate();
		loadFoodImages ();

		showTutorialMenu ();
	}


	protected override void onFinish ()
	{
		base.onFinish ();
		
		int bestFoodIndex = getMostCollisionFood();
		_gameData.bestFoodIndex = bestFoodIndex;
		
		int grade = calculateGrade(_gameData.score, _gameData.calorie);
		string gradeText = getGradeText(grade);
		_gameData.rank = gradeText;

		_gameData.calorie = getCalorieFromDistance (_gameData.distance);

		// 소수점 두 번째 자리까지 표시
		float roundedCalorie = Mathf.Round(_gameData.calorie * 100) / 100;
		_gameData.calorie = roundedCalorie;

		if (KinectManager.Instance != null)
			KinectManager.Instance.DisplayColorMap = false;

		if (guiLayer)
			guiLayer.SetActive (false);
	}


	public void showTutorialMenu()
	{
		_tutorialRunning = true;

		if (tutorialMenuGuiLayer != null) {
			GameAction.invoke(tutorialMenuAction.gameObject);
			tutorialMenuGuiLayer.SetActive (true);
		}

		if (KinectManager.Instance != null)
			KinectManager.Instance.DisplayColorMap = false;
	}


	public void startTutorial()
	{
		if (tutorialModule != null && tutorialEnabled) {
			tutorialModule.OnFinished = new GameModule.OnFinishedDelegate(onTutorialFinished);
			tutorialModule.start ();

			if (KinectManager.Instance != null)
				KinectManager.Instance.DisplayColorMap = true;

		} else {
			_tutorialRunning = false;
			onTutorialFinished();
		}
	}


	public void skipTutorial()
	{
		if (tutorialMenuGuiLayer != null)
			tutorialMenuGuiLayer.SetActive (true);

		onTutorialFinished ();
	}


	private void onTutorialFinished()
	{	
		AudioUtils.stopAllAudioSources ();
		AudioSource.PlayClipAtPoint(bgAudio, transform.position, 1.0f);

		GameAction.invoke (readyAction.gameObject, new GameAction.onFinished(onReadyActionFinished));
	}


	private void onReadyActionFinished()
	{
		if (guiLayer)
			guiLayer.SetActive (true);
		
		_tutorialRunning = false;

		if (KinectManager.Instance != null)
			KinectManager.Instance.DisplayColorMap = true;
	}


	protected override void onUpdate ()
	{
		if (isTutorialRunning ())
			return;

		timerUI ();
		updateScoreText();
		if(time >= 50.0f && time <= 50.3f)
		{	
			timer.color=Color.red;
			//RenderSettings.skybox=skyBox;
			//feverParticle[0].renderer.enabled = true;
		}
		
		if (time >= playTime) {
			finish ();
		}

		///////////////////////
		
		if(IsFeverTime==true && isFirst == true)
		{
			/////////////
			
			//////////////////
			
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
		
		if (IsFeverTime == true) 
		{
			
			feverTime += Time.deltaTime;
			
			/*
			if(feverTime <= 1.0f)
			{
				feverTimeText.enabled = true;

				Vector3 startPoint = new Vector3 (startFeverTextScale.x*0.0f,startFeverTextScale.y*0.0f);
				Vector3 firstPoint = new Vector3 (startFeverTextScale.x*2.5f, startFeverTextScale.y*2.5f);
				float fracComplete = (Time.time - startTime) / 1.0f;

				feverTimeText.transform.localScale = Vector3.Slerp (startPoint,firstPoint,fracComplete);
			}
			else
				feverTimeText.enabled=false;
			*/
			
			/////
			sLightTime += Time.deltaTime;

			if (sLightTime <= 0.2f)
			{
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
			else if(sLightTime > 0.2f)
				sLightTime = 0;
			/////
			
		}
		
		if (feverTime >= 5.0f && feverTime <=5.3f) 
		{
			stopFeverTime();
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
	
	
	private void loadFoodImages()
	{
		if (_foodImageCache == null)
			_foodImageCache = new Hashtable();
		
		foreach (Transform obj in projectiles) {
			CalorieFoodObject foodObject = obj.gameObject.GetComponent<CalorieFoodObject>();
			string imageName = "texture/Food/" + foodObject.imageName.Replace(".png", "");
			Texture2D texture = Resources.Load<Texture2D>(imageName);

			_foodImageCache.Add (foodObject.imageName, texture);
		}
	}

	
	public void startFeverTime()
	{
		startFeverTextScale = feverTimeText.transform.localScale;
		
		GameAction.invoke (feverTimeText.gameObject);
		
		feverTimeText.enabled = true;
		_isFeverTime = true;
	}


	public void stopFeverTime()
	{
		feverTimeText.transform.localScale = startFeverTextScale;
		
		feverTimeText.enabled = false;
		_isFeverTime = false;
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


	void updateScoreText()
	{
		if (scoreText != null)
			scoreText.text = _gameData.score.ToString();
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


	public void onFoodDefenced(CalorieFoodObject foodObject)
	{
		addCombo ();
		showCalorie (foodObject.calorie);
		addScore (foodObject.calorie);
		showFoodImage (foodObject.imageName);
		CheckFoodCrash (foodObject.foodId);
	}


	public void showFoodImage(string imageName)
	{
		Texture foodTexture = (Texture)_foodImageCache[imageName];
		
		if (foodTexture == null)
			return;
		
		startTime = Time.time;
		
		if (sequence < 4) {
			foodImage[sequence-1].GetComponent<GUITexture> ().texture = foodTexture;
			
		} else {
			foodImage[foodNum3].GetComponent<GUITexture> ().texture = foodTexture;
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

		_gameData.combo = 0;
	}
	
	
	public void addCombo()
	{
		int currentCombo = _gameData.combo;
		int currentMaxCombo = _gameData.maxCombo;

		currentCombo++;
		textCombo.enabled = true;
		textureCombo.enabled = true;
		
		GUIScaleAction actionFromTexture = textureCombo.GetComponent<GUIScaleAction> ();
		actionFromTexture.from = 1.5f + (currentCombo * 0.2f);
		GUIScaleAction actionFromText = textCombo.GetComponent<GUIScaleAction> ();
		actionFromText.from = 1.5f + (currentCombo * 0.2f);
		
		GameAction.invoke(textureCombo.gameObject);
		GameAction.invoke(textCombo.gameObject);
		
		textCombo.text = currentCombo.ToString();
		
		if(currentCombo > currentMaxCombo)
			currentMaxCombo = currentCombo;

		_gameData.combo = currentCombo;
		_gameData.maxCombo = currentMaxCombo;
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
		_gameData.score += score;
	}
	
	
	public void addMovingDistance(float distance)
	{
		_gameData.distance += distance;
	}
	
	
	public float getCalorieFromDistance(float distance)
	{
		// 임시로 가정된 수치로 계산함
		
		const float K = 0.000487f;
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


	private int calculateGrade(int score, float calorie)
	{
		return (score / 4000);
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
}