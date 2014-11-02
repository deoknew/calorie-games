using UnityEngine;
using System.Collections;
using EVGame.Action;

public class ActiveAction : GameAction 
{
	public bool active;


	public override void onStart(GameObject target)
	{
		target.SetActive (active);
	}
}
