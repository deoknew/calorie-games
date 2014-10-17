using UnityEngine;
using System.Collections;

namespace EVGame.Data
{
	public abstract class GameData : MonoBehaviour
	{
		void Start()
		{
			enabled = false;
		}


		public abstract void onInitData ();
	}
}