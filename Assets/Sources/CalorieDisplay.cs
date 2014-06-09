using UnityEngine;
using System.Collections;


public class CalorieDisplay : MonoBehaviour
{
	private GameManager gameManager;


	void Start ()
	{
		gameManager = GameManager.getInstance();
	}


	void Update ()
	{
		if (!gameManager)
			gameManager = GameManager.getInstance();

		if (gameManager) {
			GUIText guiText = (GUIText)gameObject.guiText;
			if (guiText != null) {
				guiText.text = gameManager.CurrentCalorie.ToString();
			}
		}
	}
}
