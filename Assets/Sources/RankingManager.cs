using UnityEngine;
using System.Collections;

public class RankingManager : MonoBehaviour 
{
	public static float getRankScore()
	{
		return PlayerPrefs.GetFloat ("RANK_SCORE");
	}


	public static float getRankCalorie()
	{
		return PlayerPrefs.GetFloat ("RANK_CALORIE");
	}


	public static void setRankScore(float score)
	{
		PlayerPrefs.SetFloat ("RANK_SCORE", score);
	}


	public static void setRankCalorie(float calorie)
	{
		PlayerPrefs.SetFloat ("RANK_CALORIE", calorie);
	}
}