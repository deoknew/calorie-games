using UnityEngine;
using System.Collections;

public class ColorChanger : MonoBehaviour 
{
	void Start () 
	{
	
	}


	void Update () 
	{
		float pos = transform.position.z - 25.0f;
		if (pos > 25.0f)
			pos = 25.0f;
		
		pos = 25.0f - pos;
		
		int p = (int)((pos / 25.0f) * 100.0f);
		float value = p * 0.01f;
		
		if (value >= 1.0f)
			value = 1.0f;
		
		renderer.material.color = new Color(value, 1.0f - value, 0.0f);
	}
}
