using UnityEngine;
using System.Collections;
using EVGame.Data;

public class CalorieDefenceGameData : GameData
{
	public int maxCombo;
	public int combo;
	public int score;
	public float distance;
	public float calorie;
	public int bestFoodIndex;
	public string rank;


	public override void onInitData ()
	{
		distance = 0.0f;
		calorie = 0.0f;
		combo = 0;
		maxCombo = 0;
		score = 0;
		bestFoodIndex = -1;
		rank = "";
	}
}
