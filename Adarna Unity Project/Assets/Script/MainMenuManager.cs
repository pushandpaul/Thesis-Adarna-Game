using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	private GameManager gameManager;
	private ConfirmationBox confirmationBox;

	private TalasalitaanManager talasalitaanManager;
	private LocationMarker locationMarker;

	public SpriteRenderer adarnaSprite;

	public AudioClip mainMenuBGM;

	void Awake(){
		gameManager = FindObjectOfType<GameManager>();
		confirmationBox = gameManager.GetComponentInChildren<ConfirmationBox>(true);
		talasalitaanManager = FindObjectOfType<TalasalitaanManager>();
	}

	void Start(){
		BGMManager bgmManager = FindObjectOfType<BGMManager> ();
		if(bgmManager != null){
			bgmManager.overridePlay (mainMenuBGM);
		}
		gameManager.setPauseMenu(false);
		gameManager.setHUDs(false);

		if (gameManager.latestPartIndex > 1) {
			adarnaSprite.enabled = true;
		} else
			adarnaSprite.enabled = false;
	}

	public void play(){
		LevelLoader levelLoader = FindObjectOfType<LevelLoader> ();
		if (gameManager.watchedIntro) {
			levelLoader.launchScene ("Chapter Selection");
		} else{
			levelLoader.launchScene ("Intro");
			gameManager.watchedIntro = true;
		}
	}

	public void openBook(){
		talasalitaanManager.ShowBook();
	}

	public void close(){
		confirmationBox.show("close");
	}
}
