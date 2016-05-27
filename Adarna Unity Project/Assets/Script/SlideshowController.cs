using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlideshowController : MonoBehaviour{

	Sprite[] slideshowImages;
	Sprite currentImage;
	int currentImageIndex = 0;
	public int initialImageIndex = 0;

	public UIFader backUIFader;
	private Image slideShowHolder;

	public float transitionDuration;
	public int fadeOutDelay;

	void Awake(){
		GameObject BackUIFaderGO = GameObject.FindGameObjectWithTag("Back UI Fader");
		GameObject slideshowHolderGO = GameObject.FindGameObjectWithTag("Slideshow Holder");

		backUIFader = BackUIFaderGO.GetComponent<UIFader>();
		slideShowHolder = slideshowHolderGO.GetComponent<Image>();
	}

	public void Begin(SlideshowImages images){
		slideshowImages = images.images;
		currentImage = slideshowImages[initialImageIndex];
		TransitionImage(false);
	}

	public void Next(){
		currentImageIndex ++;
		if(currentImageIndex < slideshowImages.Length){
			currentImage = slideshowImages[currentImageIndex];
			TransitionImage(false);
		}
		else 
			Stop();
	}

	public void Stop(){
		TransitionImage(true);
	}

	void TransitionImage(bool toEnd){
		StopAllCoroutines();
		StartCoroutine(StartTransition(toEnd));
	}

	IEnumerator StartTransition(bool toEnd){
		backUIFader.FadeIn(fadeOutDelay, transitionDuration, true);
		while(backUIFader.canvasGroup.alpha < 1){
			yield return null;
		}

		if(!toEnd){
			slideShowHolder.enabled = true;
			slideShowHolder.sprite = currentImage;
		}
		else
			slideShowHolder.enabled = false;
	}
}
