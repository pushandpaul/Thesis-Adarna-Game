using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ConfirmationBox : MonoBehaviour {

	public Text prompt;
	public Button yesBtn;
	public Button noBtn;

	private GameManager gameManger;

	public PauseMenu pauseMenu;

	public enum Kind{
		Close,
		ReturnToMainMenu,
		DeleteSave,
	}

	private Kind kind;

	void Awake(){
		pauseMenu = FindObjectOfType<PauseMenu>();
		gameManger = FindObjectOfType<GameManager>();
	}

	public void show(string kindString){
		this.gameObject.SetActive(true);
		this.kind = kind;

		pauseMenu.enabled = false;

		switch(kindString.ToLower()){
		case("close"): 
			kind = Kind.Close;
			prompt.text = "Sigurado ka bang gusto mong isara ang game?";
			yesBtn.GetComponentInChildren<Text>().text = "Oo, paalam!";
			break;
		case("return to main menu"): 
			kind = Kind.ReturnToMainMenu;
			prompt.text = "Sigurado ka bang gusto mong bumalik sa main menu?";
			yesBtn.GetComponentInChildren<Text>().text = "Oo";
			break;
		case("delete save"):
			kind = Kind.DeleteSave;
			prompt.text = "Sigurado ka bang gusto mong umulit mula sa simula?";
			yesBtn.GetComponentInChildren<Text>().text = "Umulit";
			break;
		}

	}

	void close(){
		this.gameObject.SetActive(false);
		pauseMenu.enabled = true;
	}

	public void buttonClicked(bool isYes){
		LevelLoader levelLoader = FindObjectOfType<LevelLoader>();

		if(isYes){
			switch(kind){
			case(Kind.Close): 
				Debug.Log("Close");
				Application.Quit();
				break;
			case(Kind.ReturnToMainMenu): 
				Debug.Log("Go to main menu.");

				gameManger.pauseMenu.GetComponent<PauseMenu>().ControlPauseMenu(false);
				FindObjectOfType<LevelLoader>().launchScene("Chapter Selection");
				break;
			case(Kind.DeleteSave):
				Debug.Log("Delete saves");
				gameManger.deleteAllSavedData();
				break;
			}
		}
		close();
	}
}
