using UnityEngine;
using System.Collections;
using EVGame.Action;

namespace EVGame.Module
{
	public class ActionGameModule : GameModule
	{
		public GameAction[] actions;
		
		private int _currentIndex;
		
		
		protected override void onStart()
		{
			_currentIndex = 0;
			
			if (actions != null && actions.Length > 0)
				runAction(_currentIndex);
			else
				onModuleFinished();
		}
		
		
		protected override void onUpdate()
		{
			
		}

		
		private void onActionFinished()
		{
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


		private void onModuleFinished()
		{
			finish ();
		}
	}
}