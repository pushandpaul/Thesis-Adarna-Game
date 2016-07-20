using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	private GameManager gameManager;
	private UIFader myUIFader;
	private bool inControl = false;

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
		if(isLaunch){
			myUIFader.canvasGroup.alpha = 1f;
			if(!gameManager.blurredCam.gameObject.activeInHierarchy){
				gameManager.blurredCam.gameObject.SetActive(isLaunch);
				Debug.Log("Pause menu will handle the movement of the player");
				inControl = true;
			}
			else{
				inControl = false;
			}
		}
			
		else{
			myUIFader.canvasGroup.alpha = 0f;
			if(inControl){
				gameManager.blurredCam.gameObject.SetActive(isLaunch);
			}
		}
			
		gameManager.pauseRan = isLaunch;
		myUIFader.canvasGroup.blocksRaycasts = isLaunch;
		myUIFader.canvasGroup.interactable = isLaunch;
		gameManager.pause(isLaunch);
	}
}
