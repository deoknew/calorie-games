using UnityEngine;
using System.Collections;

namespace EVGame.Action
{
	public abstract class GameAction : MonoBehaviour 
	{
		public GameObject targetObject;
		public float duration;
		public bool wait;
		public bool ignoreTimescale = false;
		
		protected float _currentTime;
		//protected onFinished _finishHandler;
		private bool _interrupted;

		private bool _initialized;
		private bool _pendingStart;

		
		public static void invoke(GameObject gameObject)
		{
			GameAction.invoke(gameObject, null);
		}
		
		
		public static void invoke(GameObject gameObject, onFinished finishHandler)
		{
			GameAction[] actions = gameObject.GetComponents<GameAction>();
			
			if (actions == null)
				return;
			
			if (actions.Length > 0) {
				GameAction action = actions[0];

				//if (finishHandler != null)
				//	action.setOnFinished(finishHandler);

				action.startActions(actions, finishHandler);
			}
		}


		public static void interrupt(GameObject gameObject)
		{
			GameAction[] actions = gameObject.GetComponents<GameAction>();

			if (actions == null)
				return;
			
			foreach (GameAction action in actions) {
				if (action != null && action.enabled) {
					action.interrupt();
				}
			}
		}
		
		
		IEnumerator runAction(GameAction action)
		{
			if (action == null || _interrupted)
				yield break;

			action.run();

			float delay = (action.wait) ? action.duration : 0.0f;
			//yield return new WaitForSeconds(delay);

			yield return StartCoroutine (delayForSeconds (delay));
		}


		IEnumerator delayForSeconds(float seconds)
		{
			float currentSeconds = 0.0f;

			while (currentSeconds < seconds) {
				if (ignoreTimescale)	
					currentSeconds += Time.unscaledDeltaTime;
				else
					currentSeconds += Time.deltaTime;

				yield return null;
			}
		}
		
		
		IEnumerator runActions(GameAction[] actions, onFinished finishHandler)
		{
			for (int i = 0; i < actions.Length; ++i) {
				if (_interrupted)
					yield break;

				yield return StartCoroutine(runAction(actions[i]));
			}

			if (finishHandler != null && !_interrupted) {
				finishHandler ();
			}
		}
		
		
		private void startActions(GameAction[] actions, onFinished finishHandler)
		{
			StartCoroutine(runActions(actions, finishHandler));
		}
		
		
		private void startAction(GameAction action)
		{
			GameAction[] actions = new GameAction[1];
			actions[0] = action;
			
			StartCoroutine(runActions(actions, null));
		}
		
		
		private void run()
		{
			if (false == _initialized) {
				_pendingStart = true;
				return;
			}

			if (isEnabled())
				interrupt();

			_currentTime = 0.0f;
			_interrupted = false;
			enabled = true;

			onStart (targetObject);
		}


		private void interrupt()
		{
			//_finishHandler = null;
			_interrupted = true;
			stop ();
		}
		
		
		private void stop()
		{
			_currentTime = 0.0f;
			enabled = false;
			
			onStop (targetObject);
		}
		
		
		private void startAction()
		{
			startAction(this);
		}
		
		
		public void invoke()
		{
			startAction();
		}
		
		
		//public void setOnFinished(onFinished finishHandler)
		//{
	//		_finishHandler = finishHandler;
	//	}
		
		
		void Start()
		{
			_interrupted = false;
			enabled = false;

			if (targetObject == null) {
				if (gameObject != null)
					targetObject = gameObject;
			}
			_initialized = true;

			if (_pendingStart) {
				_pendingStart = false;
				run ();
			}
		}
		
		
		void Update()
		{
			if (ignoreTimescale)
				_currentTime += Time.unscaledDeltaTime;
			else
				_currentTime += Time.deltaTime;

			float progress = _currentTime / duration;
			bool finished = false;

			if (progress >= 1.0f) {
				progress = 1.0f;
				finished = true;
			}
			 
			onAction (targetObject, progress);

			if (finished)
				stop ();
		}
		
		
		public void pause()
		{
			enabled = false;
		}
		
		
		public void resume()
		{
			enabled = true;
		}
		
		
		public bool isEnabled()
		{
			return enabled;
		}
		
		
		public virtual void onStart(GameObject target) {}
		public virtual void onStop(GameObject target) {}
		public virtual void onAction(GameObject target, float progress) {}
		
		public delegate void onFinished();
	}
}