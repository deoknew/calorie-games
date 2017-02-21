using UnityEngine;
using System.Collections;

using EVGame.Data;

public class SampleGameData : GameData 
{
	public int score;


	public override void onInitData ()
	{
		score = 0;
	}
}
