using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LosingScreen : MonoBehaviour {

	public UIFader canvasFader;
	public Text promptUI;
	public Image avatar;

	private LevelLoader leveLoader;
	private GameManager gameManager;

	void Awake(){
		canvasFader = GetComponent<UIFader> ();
		gameManager = FindObjectOfType<GameManager> ();
	}

	void OnLevelWasLoaded(){
		leveLoader = FindObjectOfType<LevelLoader> ();
	}

	public void setPrompt(string prompt){
		promptUI.text = prompt;
	}

	public void reset(){
		show (false);
		if(leveLoader == null){
			leveLoader = this.gameObject.AddComponent<LevelLoader>();
		}

		leveLoader.reloadScene();
			
	}

	public void quit(){
		show (false);

		if(leveLoader == null){
			leveLoader = this.gameObject.AddComponent<LevelLoader>();
		}

		leveLoader.launchScene("Main Menu");

	}

	public void show(bool show){

		canvasFader.canvasGroup.blocksRaycasts = show;
		canvasFader.canvasGroup.interactable = show;
		gameManager.pause (show);
		gameManager.setPauseMenu (!show);
		if (show) {
			canvasFader.canvasGroup.alpha = 1f;
		} else{
			canvasFader.canvasGroup.alpha = 0f;
		}
			
	}

}
