using UnityEngine;
using System.Collections;

public class AudioAction : EVAction
{
	public AudioClip audioClip; 


	public override void onStart(GameObject target) 
	{
		AudioSource.PlayClipAtPoint(audioClip, transform.position, 1.0f);
	}


	public override void onStop(GameObject target) 
	{

	}


	public override void onAction(GameObject target, float progress)
	{
			
	}
}
