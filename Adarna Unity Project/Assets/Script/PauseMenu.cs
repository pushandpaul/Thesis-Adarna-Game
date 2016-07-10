using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	private GameManager gameManager;
	private UIFader myUIFader;

	void Awake () {
		gameManager = FindObjectOfType<GameManager>();
		myUIFader = this.GetComponent<UIFader>();
	}

	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape)){
			if(!gameManager.isPaused && !gameManager.pauseRan){
				ControlPauseMenu(true);
			}
			else if(gameManager.isPaused){
				ControlPauseMenu(false);
			}
		}
	}

	public void ControlPauseMenu(bool isLaunch){
		//StartCoroutine(PauseMenuController(isLaunch));
		if(isLaunch)
			myUIFader.canvasGroup.alpha = 1f;
		else
			myUIFader.canvasGroup.alpha = 0f;
		
		gameManager.pauseRan = isLaunch;
		gameManager.blurredCam.gameObject.SetActive(isLaunch);
		myUIFader.canvasGroup.blocksRaycasts = isLaunch;
		myUIFader.canvasGroup.interactable = isLaunch;
		gameManager.pause(isLaunch);
	}
}
