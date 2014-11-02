using UnityEngine;
using System.Collections;
using EVGame;
using EVGame.Module;
using EVGame.Action;
using EVGame.Data;

public class GameManager : MonoBehaviour 
{
	public GameData gameData;
	
	//public GameObject gameUILayer;
	//public GameObject resultUILayer;
	//public GameObject pauseUILayer;

	public GameModule runningModule;
	public GameModule openingModule;
	public GameModule endingModule;
	public GameModule pauseModule;
	public GameModule resumeModule;
	public GameModule resultModule;

	private GameState _currentState;

	private GameState _prevState;
	private bool _kinectCheckingLock;


	public bool isGameRunning()
	{
		return (_currentState == GameState.RUNNING);
	}


	public bool isGameOpening()
	{
		return (_currentState == GameState.OPENING);
	}

	
	public bool isGamePause()
	{
		return (_currentState == GameState.PAUSE) || (_currentState == GameState.RESUME);
	}
	
	
	public bool isGameFinished()
	{
		return (_currentState == GameState.ENDING) || (_currentState == GameState.RESULT);
	}
	
	
	public bool isGameResult()
	{
		return (_currentState == GameState.RESULT);
	}


	private static GameManager _instance;
	public static GameManager Instance {
		get { return _instance; }
	}


	private void init()
	{
		_instance = this;
	}

	
	void initGameData()
	{
		gameData.onInitData ();
	}

	
	void Start () 
	{
		init();

		prepareGame();
		startGame();

	    checkKinectCalibration();
	}


	void Update ()
	{
		checkKinectCalibration();
	}
	

	void OnDisable()
	{
		_instance = null;
	}


	void checkKinectCalibration()
	{
		if (_kinectCheckingLock)
			return;

		//if (_currentState == GameState.RUNNING || _currentState == GameState.PAUSE) {
			KinectManager kinectManager = KinectManager.Instance;
			if (kinectManager == null)
				return;
			
			bool isUserDetected = KinectManager.Instance.IsUserDetected();

			if (isUserDetected) {
				if (isGamePause()) {
					resumeGame();
					_kinectCheckingLock = true;
				}
			} else {
				if (!isGamePause()) {
					pauseGame();
					_kinectCheckingLock = true;
				}
			}
		//}
	}


	private void prepareGame()
	{
		updateState(GameState.IDLE);
	}


	private void startGame()
	{
		updateState(GameState.OPENING);
	}


	private void pauseGame()
	{
		updateState(GameState.PAUSE);
	}


	private void resumeGame()
	{
		updateState(GameState.RESUME);
	}

	
	private void finishGame()
	{
		updateState(GameState.ENDING);
	}


	private void restartGame()
	{
		prepareGame();
		startGame();
	}
	

	private void playOpening()
	{
		if (openingModule != null) {
			openingModule.OnFinished = new GameModule.OnFinishedDelegate(onOpeningFinished);
			openingModule.start();

		} else {
			onOpeningFinished();
		}
	}


	private void playEnding()
	{	
		if (endingModule != null) {
			endingModule.OnFinished = new GameModule.OnFinishedDelegate(onEndingFinished);
			endingModule.start();
			
		} else {
			onEndingFinished();
		}
	}


	private void startRunningModule()
	{
		if (runningModule == null)
			return;
		
		runningModule.OnFinished = new GameModule.OnFinishedDelegate(onRunningFinished);
		runningModule.start(gameData);
	}


	private void startPauseModule()
	{
		if (pauseModule == null)
			return;

		pauseModule.OnFinished = new GameModule.OnFinishedDelegate(onPauseFinished);
		pauseModule.start(gameData);
	}


	private void startResumeModule()
	{
		if (resumeModule != null) {
			resumeModule.OnFinished = new GameModule.OnFinishedDelegate(onResumeFinished);
			resumeModule.start(gameData);

		} else {
			onResumeFinished();
		}
	}


	private void startResultModule()
	{
		if (resultModule == null)
			return;
		
		resultModule.start(gameData);
	}


	private void onRunningFinished()
	{
		updateState (GameState.ENDING);
	}


	private void onPauseFinished()
	{
		_kinectCheckingLock = false;
	}


	private void onResumeFinished()
	{
		//updateState(GameState.RUNNING);
		_currentState = _prevState;
		Time.timeScale = 1;

		_kinectCheckingLock = false;
	}
	

	private void onOpeningFinished()
	{
		updateState(GameState.RUNNING);
	}


	private void onEndingFinished()
	{
		updateState(GameState.RESULT);
	}
	

	private void updateState(GameState nextState)
	{
		switch (nextState) {
		case GameState.IDLE:
			initGameData();
			break;
			
		case GameState.OPENING:
			if (_currentState != GameState.IDLE)
				return;
			
			playOpening();
			break;
			
		case GameState.RUNNING:
			startRunningModule();
			break;
			
		case GameState.PAUSE:
			Time.timeScale = 0;
			_prevState = _currentState;

			startPauseModule();
			break;

		case GameState.RESUME:
			startResumeModule();
			break;
			
		case GameState.ENDING:
			playEnding();
			break;
			
		case GameState.RESULT:
			startResultModule();
			break;
		}
		//updateUI(nextState);
		
		_currentState = nextState;
	}


	/*
	private void updateUI(GameState nextState)
	{
		gameUILayer.SetActive(false);
		resultUILayer.SetActive(false);
		pauseUILayer.SetActive(false);

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
			break;
		}
	}
	*/
}
