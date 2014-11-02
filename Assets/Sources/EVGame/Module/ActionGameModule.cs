using UnityEngine;
using System.Collections;
using EVGame.Action;

namespace EVGame.Module
{
	public class ActionGameModule : GameModule
	{
		public GameAction[] actions;
		public GameAction skipAction;
		public bool loop = false;

		protected int _currentIndex;
		protected bool _skipped;
		
		
		protected override void onStart()
		{
			_currentIndex = 0;
			_skipped = false;

			if (actions != null && actions.Length > 0)
				runAction(_currentIndex);
			else
				onModuleFinished();
		}
		
		
		protected override void onUpdate()
		{
			
		}


		public void skip()
		{
			if (_skipped)
				return;

			_skipped = true;

			foreach (GameAction action in actions)
				GameAction.interrupt (action.gameObject);

			if (skipAction != null) {
				GameAction.onFinished handler = new GameAction.onFinished (onModuleFinished);
				GameAction.invoke (skipAction.gameObject, handler);
			} else {
				onModuleFinished();
			}
		}

		
		protected virtual void onActionFinished()
		{
			if (_skipped)
				return;

			_currentIndex++;
			
			if (_currentIndex < actions.Length) {
				runAction(_currentIndex);
			} else {
				onModuleFinished();
			}
		}

		
		private void runAction(int index)
		{
			if (index < actions.Length) {
				GameAction.onFinished handler = new GameAction.onFinished (onActionFinished);
				GameAction.invoke (actions [index].gameObject, handler);
			} else {
				onModuleFinished();
			}
		}


		protected virtual void onModuleFinished()
		{
			if (loop) {
				start();

			} else {
				finish ();
			}
		}
	}
}