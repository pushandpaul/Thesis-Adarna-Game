using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour {

	public GameObject BackUIFader;

	void Awake(){
		BackUIFader = GameObject.FindGameObjectWithTag ("Back UI Fader");
	}

	public void fadeScreenToBlack(float duration){
		if(BackUIFader != null){
			BackUIFader.GetComponent<UIFader> ().FadeIn(0, duration, false);
		}
	}

	public void fadeScreenToBlack(float duration, bool autoFadeOut, int autoFadeDelay){
		if(BackUIFader != null){
			BackUIFader.GetComponent<UIFader> ().FadeIn(autoFadeDelay, duration, autoFadeOut);
		}
	}

	public void fadeScreenToClear(float duration){
		if(BackUIFader != null){
			BackUIFader.GetComponent<UIFader> ().FadeOut(0, duration);
		}
	}

	public void fadeScreenToClear(float duration, int delay){
		if(BackUIFader != null){
			BackUIFader.GetComponent<UIFader> ().FadeOut(delay, duration);
		}
	}
}
