using UnityEngine;
using System.Collections;

namespace EVGame.Action
{
	public abstract class GameAction : MonoBehaviour 
	{
		public GameObject targetObject;
		public float duration;
		public bool wait;
		
		protected float _currentTime;
		protected onFinished _finishHandler;
		
		
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
				
				if (finishHandler != null)
					action.setOnFinished(finishHandler);
				
				action.startActions(actions);
			}
		}
		
		
		IEnumerator runAction(GameAction action)
		{
			if (action == null)
				yield break;
			
			action.run();
			
			float delay = (action.wait) ? action.duration : 0.0f;
			yield return new WaitForSeconds(delay);
		}
		
		
		IEnumerator runActions(GameAction[] actions)
		{
			for (int i = 0; i < actions.Length; ++i) {
				yield return StartCoroutine(runAction(actions[i]));
			}
			
			if (_finishHandler != null)
				_finishHandler();
		}
		
		
		private void startActions(GameAction[] actions)
		{
			StartCoroutine(runActions(actions));
		}
		
		
		private void startAction(GameAction action)
		{
			GameAction[] actions = new GameAction[1];
			actions[0] = action;
			
			StartCoroutine(runActions(actions));
		}
		
		
		private void run()
		{
			if (isEnabled())
				stop();
			
			_currentTime = 0.0f;
			enabled = true;
			
			onStart (targetObject);
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
		
		
		public static void interrupt(GameObject gameObject)
		{
			GameAction[] actions = gameObject.GetComponents<GameAction>();
			
			foreach (GameAction action in actions) {
				if (action != null) {
					action.stop();
				}
			}
		}
		
		
		public void setOnFinished(onFinished finishHandler)
		{
			_finishHandler = finishHandler;
		}
		
		
		void Start()
		{
			enabled = false;
			
			if (targetObject == null) {
				if (gameObject != null)
					targetObject = gameObject;
			}
		}
		
		
		void Update()
		{
			_currentTime += Time.deltaTime;
			
			float progress = _currentTime / duration;
			onAction (targetObject, progress);
			
			if (progress >= 1.0f)
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
		public abstract void onAction(GameObject target, float progress);
		
		public delegate void onFinished();
	}
}