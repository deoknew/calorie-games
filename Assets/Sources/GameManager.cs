using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	enum GameState {
		IDLE, PAUSE, RUNNING
	}

	public GUIText pauseDisplayText;
	
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
	}


	void Update () 
	{
		checkKinectCalibration();
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
		pauseDisplayText.enabled = false;
		currentState = GameState.RUNNING;
		Time.timeScale = 1;
	}


	void pauseGame()
	{
		pauseDisplayText.enabled = true;
		currentState = GameState.PAUSE;
		Time.timeScale = 0;
	}


	void resumeGame()
	{
		if (currentState == GameState.PAUSE) {
			pauseDisplayText.enabled = false;
			currentState = GameState.RUNNING;
			Time.timeScale = 1;
		}
	}


	void OnDisable()
	{
		instance = null;
	}
}
