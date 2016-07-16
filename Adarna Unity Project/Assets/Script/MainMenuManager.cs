using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	private GameManager gameManager;
	private ConfirmationBox confirmationBox;

	private TalasalitaanManager talasalitaanManager;
	private LocationMarker locationMarker;

	void Awake(){
		gameManager = FindObjectOfType<GameManager>();
		confirmationBox = gameManager.GetComponentInChildren<ConfirmationBox>(true);
		talasalitaanManager = FindObjectOfType<TalasalitaanManager>();
	}

	void Start(){
		gameManager.setPauseMenu(false);
		gameManager.pauseButton.SetActive(false);
		gameManager.setHUDs(false);
	}

	public void play(){
		FindObjectOfType<LevelLoader>().launchScene("Chapter Selection");
	}

	public void openBook(){
		talasalitaanManager.ShowBook();
	}

	public void close(){
		confirmationBox.show("close");
	}
}
