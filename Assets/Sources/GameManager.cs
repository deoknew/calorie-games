using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	enum GameState {
		IDLE, PAUSE, RUNNING, RESULT,
	}
	
	public GameObject gameUILayer;
	public GameObject resultUILayer;
	public GameObject pauseUILayer;

	public GUITexture menuCursor;
	public GUIText calorieText;
	public GUIText MaterialCalorie;
	public GUIText textHit;  //물체 충돌시 물체 위치에 바로 표시되는 칼로리량 

	public GUIText timer;
	float time,mseconds=0.0f;
	int minute=0,seconds=0;

	public Transform[] projectiles;
	private Transform ImageLocation;
	public GUITexture ImagePanel;
	public Transform text_Hit;
	private bool isCreated = false;
	private Transform obj;

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

	public bool isGameResult()
	{
		return (currentState == GameState.RESULT);
	}
	public void showImage(int index)
	{   
		if(isCreated==true)
			Destroy (obj.gameObject);

		obj = Instantiate (projectiles[index-1], ImageLocation.position, Quaternion.identity) as Transform; 
		isCreated = true;

	}
	public void showText(Transform transform,int calorie)
	{
		//text_Hit.transform.guiText.text=calorie.ToString ();

		//textHit.text = calorie.ToString ();
		//Transform text=(Transform) Instantiate(textHit.transform, transform.position, Quaternion.identity);
		Transform text=(Transform) Instantiate(text_Hit, transform.position, Quaternion.identity);
		Destroy (text.gameObject, 0.5f);

	}

	public void addCalorie(int calorie)
	{
		currentCalorie += calorie;
	}
	public void showCalorie(int calorie)
	{
		if(MaterialCalorie!=null)
		MaterialCalorie.text =calorie.ToString();
	}

	void init()
	{
		instance = this;
		currentState = GameState.IDLE;
	}


	void Start () 
	{
		init();
		startGame();

		checkKinectCalibration();
		ImageLocation = GameObject.Find ("ImageLocation").transform;
	}


	void Update () 
	{
		timerUI ();
		checkKinectCalibration();
		updateCalorieText();

		if (currentCalorie >= 3000) 
						finishGame ();
				

	}
	void timerUI()
	{
		time += Time.deltaTime;
		seconds = (int)time;
		mseconds = time - seconds;
		if (time >= 60) 
		{			
			minute += 1;
			time = 0;
		}
		timer.text =minute+":"+seconds;
		//timer.text =string.Format("{00:00}",minute,seconds);
		
		//time = Time.fixedTime;
		//timer.text = Time.fixedTime.ToString();
		//timer.text=string.Format("{0:##:(.)##}", time);
	}


	void checkKinectCalibration()
	{
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


	void startGame()
	{
		currentState = GameState.RUNNING;
		Time.timeScale = 1;
		RenderSettings.ambientLight = Color.white;

		updateUI();
	}


	void pauseGame()
	{
		currentState = GameState.PAUSE;
		Time.timeScale = 0;
		RenderSettings.ambientLight = Color.black;

		updateUI();
	}


	void resumeGame()
	{
		if (currentState == GameState.PAUSE) {
			currentState = GameState.RUNNING;
			Time.timeScale = 1;
			RenderSettings.ambientLight = Color.white;
		}

		updateUI();
	}


	void restartGame()
	{
		//TODO:
	}


	void showGameResult()
	{
		RenderSettings.ambientLight = Color.black;

		//TODO:
	}
	void finishGame()
	{
		RenderSettings.ambientLight = Color.black;
		
		currentState = GameState.RESULT;
		updateUI ();
	}

	void updateUI()
	{
		gameUILayer.SetActive(false);
		resultUILayer.SetActive(false);
		pauseUILayer.SetActive(false);

		switch (currentState) {
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
