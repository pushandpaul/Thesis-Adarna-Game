using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFader : MonoBehaviour {

	public CanvasGroup canvasGroup;

	void Start(){
		this.canvasGroup = GetComponent<CanvasGroup>();
	}

	public void FadeIn(int outDelay, float duration, bool autoFadeOut){
		StartCoroutine(InFade(outDelay, duration, autoFadeOut));
	}

	public void FadeOut(int delay, float duration){
		StartCoroutine(OutFade(delay, duration));
	}

	public void FadeTo(float duration, float target){
		StartCoroutine(ToFade(duration, canvasGroup.alpha, target));
	}

	IEnumerator InFade(int outDelay, float duration, bool autoFadeOut){
		while(canvasGroup.alpha < 1){
			canvasGroup.alpha += Time.deltaTime*duration;
			//Debug.Log("Fading in");
			yield return null;
		}
		canvasGroup.interactable = true;
		if(autoFadeOut)
			StartCoroutine(OutFade(outDelay, duration));
	}

	IEnumerator OutFade(int delay , float duration){
		yield return new WaitForSeconds(delay);
		while(canvasGroup.alpha > 0){
			//Debug.Log("Fading out with delay");
			canvasGroup.alpha -= Time.deltaTime*duration;
			yield return null;
		}
		canvasGroup.interactable = false;
	}

	IEnumerator ToFade(float duration, float initial, float target){

		if(initial > target){
			while(canvasGroup.alpha > target){
				canvasGroup.alpha -= Time.deltaTime*duration;
				yield return null;
			}
		}
		else if(initial < target)
			while(canvasGroup.alpha < target){
				canvasGroup.alpha += Time.deltaTime*duration;
				yield return null;
			}
	}
}
