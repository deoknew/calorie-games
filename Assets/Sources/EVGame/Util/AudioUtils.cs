using UnityEngine;
using System.Collections;

public class AudioUtils : MonoBehaviour
{
	void onStart()
	{
		enabled = false;
	}


	public static void stopAllAudioSources()
	{
		AudioSource[] allAudioSources = (AudioSource[])FindObjectsOfType<AudioSource> ();
		foreach (AudioSource source in allAudioSources) {
			source.Stop();
		}
	}
}
