using UnityEngine;
using System.Collections;

namespace EVGame.Action
{
	public class GenerateAction : GameAction 
	{
		public GameObject generationObject;
		public bool removeWhenFinish = true;
		
		private GameObject _generatedObject;
		
		
		public override void onStart(GameObject target) 
		{
			base.onStart (target);
			
			_generatedObject = 
				(GameObject)Instantiate (generationObject, target.transform.position, target.transform.rotation);
		}
		
		
		public override void onStop(GameObject target) 
		{
			base.onStop (target);
			
			if (_generatedObject)
				Destroy (_generatedObject);
		}
		
		
		public override void onAction(GameObject target, float progress)
		{
			
		}
	}
}