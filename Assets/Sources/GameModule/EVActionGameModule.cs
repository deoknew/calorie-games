using UnityEngine;
using System.Collections;

public class EVActionGameModule : GameModule
{
	public EVActionGroup actionGroup;

	
	protected override void onStart()
	{
		EVActionGroup.onFinished handler = new EVActionGroup.onFinished(onPlayerFinished);
		actionGroup.run(handler);
	}


	protected override void onUpdate()
	{
	
	}


	protected override void onReceiveParams(Hashtable paramTable)
	{
		
	}


	public void onPlayerFinished()
	{
		finish ();
	}
}
