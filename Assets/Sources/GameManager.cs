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


	public void addCalorie(int calorie)
	{
		currentCalorie += calorie;
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
	}


	void Update () 
	{
		checkKinectCalibration();
		updateCalorieText();
	}


	void checkKinectCalibration()
	{
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
		RenderSettings.ambientLight = Color.gray;

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
