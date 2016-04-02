using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFader : MonoBehaviour {

	public CanvasGroup canvasGroup;

	void Start(){
		this.canvasGroup = GetComponent<CanvasGroup>();
	}

	public void FadeIn(int outDelay){
		StartCoroutine(InFade(outDelay));
	}

	public void FadeOut(int delay){
		StartCoroutine(OutFade(delay));
	}

	IEnumerator InFade(int outDelay){
		while(canvasGroup.alpha < 1){
			canvasGroup.alpha += Time.deltaTime/1.5f;
			//Debug.Log("Fading in");
			yield return null;
		}
		canvasGroup.interactable = true;
		StartCoroutine(OutFade(outDelay));
	}

	IEnumerator OutFade(int delay){
		yield return new WaitForSeconds(delay);
		while(canvasGroup.alpha > 0){
			//Debug.Log("Fading out with delay");
			canvasGroup.alpha -= Time.deltaTime/1.5f;
			yield return null;
		}
		canvasGroup.interactable = false;
	}
}
