using UnityEngine;
using System.Collections;

namespace EVGame.Action
{
	public class AudioAction : GameAction
	{
		public AudioClip audioClip;
		public float volume = 1.0f;
		public bool stopAllAudioWhenPlay = false;
		
		
		public override void onStart(GameObject target) 
		{
			if (stopAllAudioWhenPlay)
				AudioUtils.stopAllAudioSources ();

			AudioSource.PlayClipAtPoint(audioClip, transform.position, volume);
		}
		
		
		public override void onStop(GameObject target) 
		{
			
		}
		
		
		public override void onAction(GameObject target, float progress)
		{
			
		}
	}
}