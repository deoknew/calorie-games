using UnityEngine;
using System.Collections;

public class MouseObjectController : MonoBehaviour 
{
	private const float DEFAULT_Z_POSITION = 17.0f;

	private Vector3 prevPosition;


	void Update ()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = DEFAULT_Z_POSITION;
		
		Vector3 objectPos = Camera.main.ScreenToWorldPoint (mousePos);
		objectPos.z = transform.position.z;
		transform.position = objectPos;

		//(!) 테스트를 위해 마우스로 이동한 거리도 전송

		if (GameManager.Instance.isGameRunning()) {
			if (prevPosition.magnitude == 0) {
				prevPosition = objectPos;
			}

			float distance = Mathf.Sqrt(
				Mathf.Pow(Mathf.Abs(prevPosition.x - objectPos.x), 2.0f) 
				+ Mathf.Pow(Mathf.Abs(prevPosition.y - objectPos.y), 2.0f)
			);

			RunningGameModule.Instance.addMovingDistance(distance);

			prevPosition = objectPos;
		}
	}
}
