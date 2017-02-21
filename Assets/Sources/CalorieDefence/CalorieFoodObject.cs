using UnityEngine;
using System.Collections;
using EVGame.Action;

public class CalorieFoodObject : MonoBehaviour
{
	private static int _prevFoodHash = 0;

	public AudioClip consumeAudio;
	public GameObject consumeParticle;

	public int calorie;
	public int foodId;
	public string imageName;


	void Update() 
	{
		if (transform.position.y <= 1.5 || GameManager.Instance.isGameFinished())
		{
			RunningGameModule.Instance.resetCombo();
			Destroy (gameObject);

			if (RunningGameModule.Instance.isTutorialRunning()) {
				//(!) 접근방법이 좋지 않음.
				TutorialGameModule module = (TutorialGameModule)RunningGameModule.Instance.tutorialModule;
				module.failedDefence();
			}
		}
	}


	void OnCollisionEnter (Collision collision)
	{
		if (!collision.collider.tag.Equals("Non-Explosion"))
		{
			if (foodId == 12) // FeverTime
				RunningGameModule.Instance.startFeverTime();

			if (foodId == 11) // Bomb
				GameAction.invoke(Camera.main.gameObject);

			GameObject obj = (GameObject)Instantiate (consumeParticle, transform.position, Quaternion.identity);
			Destroy(obj, 3);

			AudioSource.PlayClipAtPoint(consumeAudio, transform.position, 1.0f);
		
			// 양 손으로 동시에 충돌했을 때 중복처리를 방지하기 위함.
			if (_prevFoodHash == GetHashCode())
				return;
			else
				_prevFoodHash = GetHashCode();

			if (false == RunningGameModule.Instance.isTutorialRunning()) {
				RunningGameModule.Instance.onFoodDefenced(this);

			} else {
				//(!) 접근방법이 좋지 않음.
				TutorialGameModule module = (TutorialGameModule)RunningGameModule.Instance.tutorialModule;
				module.successDefence();
			}
		}
		Destroy (gameObject);
	}
}