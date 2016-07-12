using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlideshowController : MonoBehaviour{

	Sprite[] slideshowImages;
	Sprite currentImage;
	int currentImageIndex = 0;

	public UIFader backUIFader;
	private Image slideShowHolder;
	private GameManager gameManager;

	[Tooltip("Transition duration in seconds")]
	public float transitionDuration = 2;
	public int fadeOutDelay;

	void Awake(){
		GameObject BackUIFaderGO = GameObject.FindGameObjectWithTag("Back UI Fader");
		GameObject slideshowHolderGO = GameObject.FindGameObjectWithTag("Slideshow Holder");
		gameManager = FindObjectOfType<GameManager> ();

		backUIFader = BackUIFaderGO.GetComponent<UIFader>();
		slideShowHolder = slideshowHolderGO.GetComponent<Image>();

		transitionDuration = 1/transitionDuration;
	}

	public void Begin(SlideshowImages images, bool enableHUD, bool enablePause){
		gameManager.setHUDs (enableHUD);
		gameManager.setPauseMenu (enablePause);
		slideshowImages = images.images;
		currentImage = slideshowImages[0];
		currentImageIndex = 0;
		TransitionImage(false);
	}

	public void Begin(SlideshowImages images, int startingIndex, bool enablePause, bool enableHUD){
		gameManager.setHUDs (enableHUD);
		gameManager.setPauseMenu (enablePause);
		slideshowImages = images.images;
		currentImage = slideshowImages[startingIndex];
		currentImageIndex = startingIndex;
		TransitionImage(false);
	}

	public void Next(){
		currentImageIndex++;
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
		backUIFader.StopAllCoroutines ();
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
		else{
			slideShowHolder.enabled = false;
			gameManager.setHUDs (true);
			gameManager.setPauseMenu (true);
		}
			
	}
}
