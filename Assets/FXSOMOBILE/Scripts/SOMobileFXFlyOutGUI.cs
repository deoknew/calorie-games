using UnityEngine;
using System.Collections;

public class SOMobileFXFlyOutGUI : SOmobileFX_Abstract {
	public GameObject animatedObject;
	public void FlyOutEnded(){
		animatedObject.animation.Play();
		Destroy(gameObject,2f);
	}

}
