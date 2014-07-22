using UnityEngine;
using System.Collections;

public abstract class GameModule : MonoBehaviour 
{
	private bool _running;
	public bool isRunning {
		set { _running = value; }
		get { return _running; }
	}


	void Start () 
	{
		_running = false;
	}


	public abstract void startModule(Hashtable paramTable);
}
