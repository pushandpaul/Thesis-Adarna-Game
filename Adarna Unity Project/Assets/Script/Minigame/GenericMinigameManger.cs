using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Fungus;

public class GenericMinigameManger : MonoBehaviour {

	public string minigameName;
	public string minigameSceneName;
	public bool allowMinigameStart = false;
	public string title;
	public string description;
	private LayuninUIManager layuninUI;
	private Flowchart flowchart;
	public AudioClip minigameAudio;

	public LosingScreen losingScreen;

	public bool showInstruction = true;

	void Awake () {
		layuninUI = FindObjectOfType<LayuninUIManager>();
		FindObjectOfType<GameManager> ().currentScene = minigameSceneName;
		losingScreen = FindObjectOfType<LosingScreen> ();

		Flowchart[] flowcharts = FindObjectsOfType<Flowchart> ();

		foreach(Flowchart _flowchart in flowcharts){
			if (_flowchart.tag != "Global Flowchart"){
				flowchart = _flowchart;
				break;
			}
		}
	}

	void Start () {
		if(LayuninUIManager.lastTriggered != minigameName && showInstruction){
			layuninUI.Launch(title, description, LayuninUIManager.CloseControl.PressAnywhere, true);
			LayuninUIManager.lastTriggered = minigameName;
			if(minigameAudio != null){
				layuninUI.songAfter = minigameAudio;
			}

		}
		else
			allowMinigameStart = true;
	}

	void Update () {
		if(layuninUI.layuninUI.canvasGroup.alpha > 0){
			return;
		}
		else{
			allowMinigameStart = true;
			this.enabled = false;
		}
	}

	public bool checkCanStart(){
		return allowMinigameStart;
	}

	public void lose(){
		flowchart.ExecuteBlock ("Lose");
	}

	public void showLosingScreen(bool show){
		losingScreen.show (show);
	}

	public void loseResponded(){
		FindObjectOfType<GameManager> ().pause (false);
		showLosingScreen (false);
	}		
}
