using UnityEngine;
using System.Collections;
using System.Reflection;
using EVGame.Data;

namespace EVGame.Module 
{
	public abstract class GameModule : MonoBehaviour 
	{
		private GameData _currentGameData;
		public GameData CurrentGameData {
			get { return _currentGameData; }
		}

		private OnFinishedDelegate _onFinished;
		public OnFinishedDelegate OnFinished {
			set { _onFinished = value; }
		}

		private bool _initialized;
		private bool _pendingStart;
		
		
		public bool isRunning()
		{
			return enabled;
		}
		
		
		void Start ()
		{
			_initialized = true;
			enabled = false;

			if (_pendingStart)
				start ();
		}
		
		
		void Update()
		{
			onUpdate();
		}
		
		
		public void start()
		{
			if (false == _initialized) {
				_pendingStart = true;
				return;
			}

			enabled = true;
			onStart ();
		}
		
		
		public void start(GameData gameData)
		{
			_currentGameData = gameData;

			start ();
		}
		
		
		public void finish()
		{
			enabled = false;
			onFinish ();
			
			if (_onFinished != null)
				_onFinished();
		}
		
		
		protected virtual void onStart() {}
		protected virtual void onUpdate() {}
		protected virtual void onFinish() {}
		
		public delegate void OnFinishedDelegate();
	}
}